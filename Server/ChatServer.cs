using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text.Json;

using Common.Models;
using Common.Network;
using Server.Database;

namespace Server
{
    internal class ChatServer
    {
        private static readonly ChatDatabase database = new ChatDatabase();

        // Active clients and their StreamWriters
        private static readonly ConcurrentDictionary<TcpClient, StreamWriter> clients
            = new ConcurrentDictionary<TcpClient, StreamWriter>();

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
                    TcpClient client = await server.AcceptTcpClientAsync();
                    Console.WriteLine($"Client connected: {client.Client.RemoteEndPoint}");

                    // Handle client asynchronously with error logging
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await HandleClient(client);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Client handler error: {ex.Message}");
                        }
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error accepting client: {ex.Message}");
                }
            }
        }

        private static async Task HandleClient(TcpClient client)
        {
            try
            {
                using (NetworkStream stream = client.GetStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

                    clients.TryAdd(client, writer);

                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        try
                        {
                            var msg = JsonSerializer.Deserialize<NetworkMessage>(line);
                            if (msg == null) continue;

                            switch (msg.MessageType)
                            {
                                case NetworkMessageType.Signup:
                                    await HandleSignUp(client, msg.GetPayload<User>());
                                    break;

                                case NetworkMessageType.Login:
                                    await HandleLogIn(client, msg.GetPayload<User>());
                                    break;

                                case NetworkMessageType.Logout:
                                    await HandleLogout(client);
                                    return;

                                case NetworkMessageType.ChatMessage:
                                case NetworkMessageType.ListChats:
                                case NetworkMessageType.CreateChat:
                                case NetworkMessageType.AddMember:
                                    Console.WriteLine($"Unhandled message type: {msg.MessageType}");
                                    break;

                                default:
                                    Console.WriteLine($"Unknown message type: {msg.MessageType}");
                                    break;
                            }
                        }
                        catch (JsonException)
                        {
                            await SendError(client, "Invalid message format.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error processing message: {ex.Message}");
                            await SendError(client, "Internal server error.");
                        }
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine($"Client disconnected abruptly: {client.Client.RemoteEndPoint}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with client {client.Client.RemoteEndPoint}: {ex.Message}");
            }
            finally
            {
                clients.TryRemove(client, out _);
                client.Close();
                Console.WriteLine($"Client disconnected: {client.Client.RemoteEndPoint}");
            }
        }

        private static async Task HandleSignUp(TcpClient client, User user)
        {
            var existing = database.GetUserByName(user.Name);
            if (existing != null)
            {
                await SendError(client, "User already exists!");
                return;
            }

            user.HashedPassword = Security.HashPassword(user.HashedPassword);
            await database.InsertUser(user);

            await SendResponse(client, new NetworkMessage
            {
                MessageType = NetworkMessageType.Signup,
                Payload = JsonSerializer.Serialize("Signup successful")
            });

            Console.WriteLine($"User signed up: {user.Name}");
        }

        private static async Task HandleLogIn(TcpClient client, User user)
        {
            var storedUser = database.GetUserByName(user.Name);
            if (storedUser == null)
            {
                await SendError(client, "User not found");
                return;
            }

            bool valid = Security.VerifyPassword(user.HashedPassword, storedUser.HashedPassword);
            if (!valid)
            {
                await SendError(client, "Invalid password");
                return;
            }

            await SendResponse(client, new NetworkMessage
            {
                MessageType = NetworkMessageType.Login,
                Payload = JsonSerializer.Serialize(storedUser)
            });

            Console.WriteLine($"User logged in: {user.Name}");
        }

        private static async Task HandleLogout(TcpClient client)
        {
            clients.TryRemove(client, out _);
            client.Close();
            Console.WriteLine($"Client logged out: {client.Client.RemoteEndPoint}");
        }

        private static async Task BroadcastMessage(NetworkMessage msg)
        {
            string serialized = JsonSerializer.Serialize(msg);

            foreach (var kvp in clients)
            {
                try
                {
                    await kvp.Value.WriteLineAsync(serialized);
                }
                catch
                {
                    clients.TryRemove(kvp.Key, out _);
                    kvp.Key.Close();
                }
            }

            Console.WriteLine("Broadcasted message to all clients");
        }

        private static async Task SendResponse(TcpClient client, NetworkMessage msg)
        {
            if (clients.TryGetValue(client, out StreamWriter writer))
            {
                string json = JsonSerializer.Serialize(msg);
                try
                {
                    await writer.WriteLineAsync(json);
                }
                catch (IOException)
                {
                    clients.TryRemove(client, out _);
                    client.Close();
                }
            }
        }

        private static async Task SendError(TcpClient client, string error)
        {
            await SendResponse(client, new NetworkMessage
            {
                MessageType = NetworkMessageType.Error,
                Payload = JsonSerializer.Serialize(error)
            });
        }
    }
}
