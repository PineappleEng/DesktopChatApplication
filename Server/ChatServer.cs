using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;

using Common.Models;
using Common.Network;
using Server.Database;

namespace Server
{
    internal class ChatServer
    {
        private static readonly ChatDatabase database = new ChatDatabase();

        // Client sessions keyed by TcpClient
        private class ClientSession
        {
            public TcpClient Client { get; set; }
            public StreamWriter Writer { get; set; }
            public StreamReader Reader { get; set; }
            public object WriteLock { get; } = new object();
            public User LoggedUser { get; set; }
        }

        private static readonly ConcurrentDictionary<TcpClient, ClientSession> clients
            = new ConcurrentDictionary<TcpClient, ClientSession>();

        private static readonly ConcurrentDictionary<int, ClientSession> loggedInUsers
            = new ConcurrentDictionary<int, ClientSession>();

        static async Task Main()
        {
            IPAddress localAddr = IPAddress.Any;
            int port = 6969;

            TcpListener server = new TcpListener(localAddr, port);
            server.Start();
            Console.Write($"Chat Server listening on {localAddr}:{port}\r\n");

            while (true)
            {
                try
                {
                    TcpClient tcpClient = await server.AcceptTcpClientAsync();
                    string ep = tcpClient.Client?.RemoteEndPoint?.ToString() ?? "Unknown";
                    Console.Write($"Client connected: {ep}\r\n");

                    _ = Task.Run(() => HandleClient(tcpClient));
                }
                catch (Exception ex)
                {
                    Console.Write($"Error accepting client: {ex}\r\n");
                }
            }
        }

        private static async Task HandleClient(TcpClient tcpClient)
        {
            string ep = tcpClient.Client?.RemoteEndPoint?.ToString() ?? "Unknown";

            try
            {
                NetworkStream stream = tcpClient.GetStream();
                var reader = new StreamReader(stream);
                var writer = new StreamWriter(stream) { AutoFlush = true };

                var session = new ClientSession
                {
                    Client = tcpClient,
                    Reader = reader,
                    Writer = writer
                };

                clients.TryAdd(tcpClient, session);

                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    NetworkMessage msg = null;
                    try
                    {
                        msg = JsonSerializer.Deserialize<NetworkMessage>(line);
                        if (msg == null) continue;

                        switch (msg.MessageType)
                        {
                            case NetworkMessageType.Signup:
                            {
                                var user = msg.GetPayload<User>();
                                await HandleSignUp(session, user);
                                break;
                            }

                            case NetworkMessageType.Login:
                            {
                                var user = msg.GetPayload<User>();
                                await HandleLogIn(session, user);
                                break;
                            }

                            case NetworkMessageType.Logout:
                            {
                                await HandleLogout(session);
                                return;
                            }

                            case NetworkMessageType.GetChats:
                            {
                                var user = msg.GetPayload<User>();
                                await HandleGetChats(session, user);
                                break;
                            }

                            case NetworkMessageType.GetUsers:
                            {
                                await HandleGetUsers(session);
                                break;
                            }

                            case NetworkMessageType.CreateChat:
                            {
                                // Payload is a composite: [ Chat, List<User> ]
                                JsonElement element = JsonSerializer.Deserialize<JsonElement>(msg.Payload);

                                try
                                {
                                    JsonElement chatEl = element[0];
                                    JsonElement usersEl = element[1];

                                    Chat chat = chatEl.Deserialize<Chat>();
                                    List<User> users = usersEl.Deserialize<List<User>>();

                                    if (chat == null || users == null)
                                    {
                                        await SendError(session, "Failed to parse chat or users");
                                        break;
                                    }

                                    await HandleCreateChat(session, chat, users);
                                }
                                catch (JsonException jex)
                                {
                                    Console.Write($"CreateChat JSON parse error: {jex}\r\n");
                                    await SendError(session, "Invalid CreateChat payload");
                                }

                                break;
                            }

                            case NetworkMessageType.GetMessages:
                            {
                                var chat = msg.GetPayload<Chat>();
                                await HandleGetMessages(session, chat);
                                break;
                            }

                            case NetworkMessageType.ChatMessage:
                            {
                                var message = msg.GetPayload<KeyValuePair<string, Message>>();
                                await HandleChatMessage(message.Key, message.Value);
                                break;
                            }

                            case NetworkMessageType.AddMember:
                            {
                                Console.Write($"Unhandled message type: {msg.MessageType}\r\n");
                                break;
                            }

                            default:
                                Console.Write($"Unknown message type: {msg.MessageType}\r\n");
                                break;
                        }
                    }
                    catch (JsonException jex)
                    {
                        Console.Write($"JSON deserialization error from {ep}: {jex}\r\n");
                        if (clients.TryGetValue(tcpClient, out var c)) await SendError(c, "Invalid message format.");
                    }
                    catch (Exception ex)
                    {
                        Console.Write($"Error processing message from {ep}: {ex}\r\n");
                        if (clients.TryGetValue(tcpClient, out var c)) await SendError(c, "Internal server error.");
                    }
                }
            }
            catch (IOException)
            {
                Console.Write($"Client disconnected abruptly: {ep}\r\n");
            }
            catch (Exception ex)
            {
                Console.Write($"Error with client {ep}: {ex}\r\n");
            }
            finally
            {
                // Clean up session if still present
                clients.TryRemove(tcpClient, out var removed);

                // If this session had a logged-in user, remove from loggedInUsers as well
                if (removed?.LoggedUser != null)
                {
                    int uid = removed.LoggedUser.Id;
                    // Only remove if the current mapped session is the same session to avoid stomping another connection
                    if (loggedInUsers.TryGetValue(uid, out var existing) && existing == removed)
                    {
                        loggedInUsers.TryRemove(uid, out _);
                        Console.Write($"Removed user {removed.LoggedUser.Name} (id={uid}) from loggedInUsers due to disconnect.\r\n");
                    }
                }

                try
                {
                    tcpClient.Close();
                }
                catch { /* ignore */ }

                Console.Write($"Client disconnected: {ep}\r\n");
            }
        }

        #region Handlers

        private static async Task HandleSignUp(ClientSession session, User user)
        {
            if (user == null)
            {
                await SendError(session, "Invalid user object for signup");
                return;
            }

            var existing = database.GetUserByName(user.Name);
            if (existing != null)
            {
                await SendError(session, "User already exists!");
                return;
            }

            user.HashedPassword = Security.HashPassword(user.HashedPassword);
            await database.InsertUser(user);

            await SendResponse(session, new NetworkMessage
            {
                MessageType = NetworkMessageType.Signup,
                Payload = JsonSerializer.Serialize("Signup successful")
            });

            Console.Write($"User signed up: {user.Name}\r\n");
        }

        private static async Task HandleLogIn(ClientSession session, User user)
        {
            if (user == null)
            {
                await SendError(session, "Invalid user object for login");
                return;
            }

            var storedUser = database.GetUserByName(user.Name);
            if (storedUser == null)
            {
                await SendError(session, "User not found");
                return;
            }

            bool valid = Security.VerifyPassword(user.HashedPassword, storedUser.HashedPassword);
            if (!valid)
            {
                await SendError(session, "Invalid password");
                return;
            }

            // Attempt to register the logged-in user atomically.
            // Set the session's LoggedUser first, then try to add to the loggedInUsers map.
            session.LoggedUser = storedUser;

            if (!loggedInUsers.TryAdd(storedUser.Id, session))
            {
                // someone else already has a session for this user id
                session.LoggedUser = null;
                await SendError(session, "User already logged in from another session");
                Console.Write($"Login blocked for {storedUser.Name}: already logged in elsewhere.\r\n");
                return;
            }

            await SendResponse(session, new NetworkMessage
            {
                MessageType = NetworkMessageType.Login,
                Payload = JsonSerializer.Serialize(storedUser)
            });

            Console.Write($"User logged in: {storedUser.Name} (from {session.Client.Client?.RemoteEndPoint})\r\n");
        }

        private static async Task HandleLogout(ClientSession session)
        {
            string endpoint = "<unknown>";

            try
            {
                endpoint = session.Client?.Client.RemoteEndPoint?.ToString() ?? "<unknown>";

                // Send logout acknowledgment before closing
                await SendResponse(session, new NetworkMessage
                {
                    MessageType = NetworkMessageType.Logout,
                    Payload = JsonSerializer.Serialize("Logout successful")
                });
            }
            catch (Exception ex)
            {
                Console.Write($"Error sending logout response to {endpoint}: {ex.Message}\r\n");
            }
            finally
            {
                // Remove from logged-in users map if necessary
                if (session.LoggedUser != null)
                {
                    int uid = session.LoggedUser.Id;
                    if (loggedInUsers.TryGetValue(uid, out var existing) && existing == session)
                    {
                        loggedInUsers.TryRemove(uid, out _);
                        Console.Write($"Removed user {session.LoggedUser.Name} (id={uid}) from loggedInUsers on logout.\r\n");
                    }
                    session.LoggedUser = null;
                }

                clients.TryRemove(session.Client, out _);

                try
                {
                    session.Client.Close(); // safe even if already closed
                }
                catch { /* ignore */ }

                Console.Write($"Client logged out: {endpoint}\r\n");
            }
        }

        private static async Task HandleGetChats(ClientSession session, User user)
        {
            if (user == null)
            {
                await SendError(session, "Invalid user for GetChats");
                return;
            }

            var chats = database.GetUserChats(user.Id);
            await SendResponse(session, new NetworkMessage
            {
                MessageType = NetworkMessageType.GetChats,
                Payload = JsonSerializer.Serialize(chats)
            });
        }

        private static async Task HandleGetUsers(ClientSession session)
        {
            var users = database.GetAllUsers();
            await SendResponse(session, new NetworkMessage
            {
                MessageType = NetworkMessageType.GetUsers,
                Payload = JsonSerializer.Serialize(users)
            });

            Console.Write($"Sent user list to {session.Client.Client?.RemoteEndPoint}\r\n");
        }

        private static async Task HandleCreateChat(ClientSession session, Chat chat, List<User> users)
        {
            if (chat == null || users == null)
            {
                await SendError(session, "Invalid chat creation data");
                return;
            }

            // Persist chat (assuming InsertChat sets chat.Id)
            await database.InsertChat(chat);

            foreach (var user in users)
            {
                // Defensive: ensure user.Id is present (DB should have created/validated this)
                await database.InsertUserChat(new UserChat
                {
                    ChatId = chat.Id,
                    UserId = user.Id
                });
            }

            await BroadcastMessage(new NetworkMessage
            {
                MessageType = NetworkMessageType.CreateChat,
                Payload = JsonSerializer.Serialize(chat)
            });

            Console.Write($"Chat created: {chat.Name} (id={chat.Id}) by {session.LoggedUser?.Name ?? "unknown"}\r\n");
        }

        private static async Task HandleGetMessages(ClientSession session, Chat chat)
        {
            List<KeyValuePair<string, Message>> messages = database.GetChatMessages(chat);
            await SendResponse(session, new NetworkMessage
            {
                MessageType = NetworkMessageType.GetMessages,
                Payload = JsonSerializer.Serialize(messages)
            });
            Console.Write($"Sent message list to {session.Client.Client?.RemoteEndPoint}\r\n");
        }

        private static async Task HandleChatMessage(string sender, Message message)
        {
            await database.InsertMessage(message);
            await BroadcastMessage(new NetworkMessage
            {
                MessageType = NetworkMessageType.ChatMessage,
                Payload = JsonSerializer.Serialize(new KeyValuePair<string, Message>(sender, message))
            });
        }

        #endregion

        #region Messaging utilities

        private static async Task BroadcastMessage(NetworkMessage msg)
        {
            string serialized = JsonSerializer.Serialize(msg);

            // Take a snapshot to avoid concurrent modification problems.
            var snapshot = clients.ToArray();

            foreach (var kvp in snapshot)
            {
                var session = kvp.Value;
                try
                {
                    lock (session.WriteLock)
                    {
                        var writeTask = session.Writer.WriteLineAsync(serialized);
                        writeTask.Wait();
                    }
                }
                catch (Exception ex)
                {
                    Console.Write($"Broadcast write error to {session.Client.Client?.RemoteEndPoint}: {ex}\r\n");
                    clients.TryRemove(session.Client, out _);
                    try { session.Client.Close(); } catch { }
                }
            }

            Console.Write("Broadcasted message to clients\r\n");
            await Task.CompletedTask;
        }

        private static async Task SendResponse(ClientSession session, NetworkMessage msg)
        {
            if (session == null) return;

            string json = JsonSerializer.Serialize(msg);
            try
            {
                lock (session.WriteLock)
                {
                    var t = session.Writer.WriteLineAsync(json);
                    t.Wait(); // ensure the write completes while under the lock for ordering
                }
            }
            catch (IOException ioex)
            {
                Console.Write($"Write IO error to {session.Client.Client?.RemoteEndPoint}: {ioex}\r\n");
                clients.TryRemove(session.Client, out _);
                try { session.Client.Close(); } catch { }
            }
            catch (Exception ex)
            {
                Console.Write($"Write error to {session.Client.Client?.RemoteEndPoint}: {ex}\r\n");
            }

            await Task.CompletedTask;
        }

        private static async Task SendError(ClientSession session, string error)
        {
            await SendResponse(session, new NetworkMessage
            {
                MessageType = NetworkMessageType.Error,
                Payload = JsonSerializer.Serialize(error)
            });
        }

        #endregion
    }
}
