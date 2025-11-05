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

        // Use TcpClient as key (consistent with original approach) but store a richer session value
        private static readonly ConcurrentDictionary<TcpClient, ClientSession> clients
            = new ConcurrentDictionary<TcpClient, ClientSession>();

        // New: logged-in users dictionary keyed by User.Id for fast lookup of currently authenticated sessions.
        // Value is the ClientSession associated with that logged-in user.
        private static readonly ConcurrentDictionary<int, ClientSession> loggedInUsers
            = new ConcurrentDictionary<int, ClientSession>();

        static async Task Main()
        {
            IPAddress localAddr = IPAddress.Any; // Listen on all interfaces
            int port = 6969;

            TcpListener server = new TcpListener(localAddr, port);
            server.Start();
            Console.WriteLine($"Chat Server listening on {localAddr}:{port}");

            while (true)
            {
                try
                {
                    TcpClient tcpClient = await server.AcceptTcpClientAsync();
                    string ep = tcpClient.Client?.RemoteEndPoint?.ToString() ?? "Unknown";
                    Console.WriteLine($"Client connected: {ep}");

                    // Fire-and-forget the handler (we're logging inside)
                    _ = Task.Run(() => HandleClient(tcpClient));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error accepting client: {ex}");
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
                                    // After logout we break the loop and cleanup (client closed in logout)
                                    return;
                                }

                            case NetworkMessageType.ListChats:
                                {
                                    var user = msg.GetPayload<User>();
                                    await HandleListChats(session, user);
                                    break;
                                }

                            case NetworkMessageType.ListUsers:
                                {
                                    await HandleListUsers(session);
                                    break;
                                }

                            case NetworkMessageType.CreateChat:
                                {
                                    // Payload is a composite: [ Chat, List<User> ]
                                    if (string.IsNullOrEmpty(msg.Payload))
                                    {
                                        await SendError(session, "CreateChat payload missing");
                                        break;
                                    }

                                    JsonElement element = JsonSerializer.Deserialize<JsonElement>(msg.Payload);

                                    if (element.ValueKind != JsonValueKind.Array || element.GetArrayLength() < 2)
                                    {
                                        await SendError(session, "Invalid CreateChat payload format");
                                        break;
                                    }

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
                                        Console.WriteLine($"CreateChat JSON parse error: {jex}");
                                        await SendError(session, "Invalid CreateChat payload");
                                    }

                                    break;
                                }

                            case NetworkMessageType.ChatMessage:
                            case NetworkMessageType.AddMember:
                                {
                                    Console.WriteLine($"Unhandled message type: {msg.MessageType}");
                                    break;
                                }

                            default:
                                Console.WriteLine($"Unknown message type: {msg.MessageType}");
                                break;
                        }
                    }
                    catch (JsonException jex)
                    {
                        Console.WriteLine($"JSON deserialization error from {ep}: {jex}");
                        if (clients.TryGetValue(tcpClient, out var c)) await SendError(c, "Invalid message format.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing message from {ep}: {ex}");
                        if (clients.TryGetValue(tcpClient, out var c)) await SendError(c, "Internal server error.");
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine($"Client disconnected abruptly: {ep}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with client {ep}: {ex}");
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
                        Console.WriteLine($"Removed user {removed.LoggedUser.Name} (id={uid}) from loggedInUsers due to disconnect.");
                    }
                }

                try
                {
                    tcpClient.Close();
                }
                catch { /* ignore */ }

                Console.WriteLine($"Client disconnected: {ep}");
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

            Console.WriteLine($"User signed up: {user.Name}");
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
                Console.WriteLine($"Login blocked for {storedUser.Name}: already logged in elsewhere.");
                return;
            }

            await SendResponse(session, new NetworkMessage
            {
                MessageType = NetworkMessageType.Login,
                Payload = JsonSerializer.Serialize(storedUser)
            });

            Console.WriteLine($"User logged in: {storedUser.Name} (from {session.Client.Client?.RemoteEndPoint})");
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
                Console.WriteLine($"Error sending logout response to {endpoint}: {ex.Message}");
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
                        Console.WriteLine($"Removed user {session.LoggedUser.Name} (id={uid}) from loggedInUsers on logout.");
                    }
                    session.LoggedUser = null;
                }

                clients.TryRemove(session.Client, out _);

                try
                {
                    session.Client.Close(); // safe even if already closed
                }
                catch { /* ignore */ }

                Console.WriteLine($"Client logged out: {endpoint}");
            }
        }

        private static async Task HandleListChats(ClientSession session, User user)
        {
            if (user == null)
            {
                await SendError(session, "Invalid user for ListChats");
                return;
            }

            var chats = database.GetUserChats(user.Id);
            await SendResponse(session, new NetworkMessage
            {
                MessageType = NetworkMessageType.ListChats,
                Payload = JsonSerializer.Serialize(chats)
            });
        }

        private static async Task HandleListUsers(ClientSession session)
        {
            var users = database.GetAllUsers();
            await SendResponse(session, new NetworkMessage
            {
                MessageType = NetworkMessageType.ListUsers,
                Payload = JsonSerializer.Serialize(users)
            });

            Console.WriteLine($"Sent user list to {session.Client.Client?.RemoteEndPoint}");
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

            await SendResponse(session, new NetworkMessage
            {
                MessageType = NetworkMessageType.CreateChat,
                Payload = JsonSerializer.Serialize(chat)
            });

            Console.WriteLine($"Chat created: {chat.Name} (id={chat.Id}) by {session.LoggedUser?.Name ?? "unknown"}");
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
                    Console.WriteLine($"Broadcast write error to {session.Client.Client?.RemoteEndPoint}: {ex}");
                    clients.TryRemove(session.Client, out _);
                    try { session.Client.Close(); } catch { }
                }
            }

            Console.WriteLine("Broadcasted message to clients");
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
                Console.WriteLine($"Write IO error to {session.Client.Client?.RemoteEndPoint}: {ioex}");
                clients.TryRemove(session.Client, out _);
                try { session.Client.Close(); } catch { }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Write error to {session.Client.Client?.RemoteEndPoint}: {ex}");
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
