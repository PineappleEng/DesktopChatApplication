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
        private static ChatDatabase database = new ChatDatabase();

        // Concurrent dictionary to store active clients and their writers
        private static ConcurrentDictionary<TcpClient, StreamWriter> clients
            = new ConcurrentDictionary<TcpClient, StreamWriter>();

        static async Task Main()
        {
            IPAddress localAddr = IPAddress.Parse("10.0.2.15"); // Replace with your IP
            int port = 6969;

            TcpListener server = new TcpListener(localAddr, port);
            server.Start();
            Console.Write($"Chat Server listening on {localAddr}:{port}\r\n");

            while (true)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                Console.Write($"Client connected: {client.Client.RemoteEndPoint}\r\n");
                _ = Task.Run(() => HandleClient(client));
            }
        }

        private static async Task HandleClient(TcpClient client)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                StreamReader reader = new StreamReader(stream);
                StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

                clients.TryAdd(client, writer);

                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    try
                    {
                        NetworkMessage msg = JsonSerializer.Deserialize<NetworkMessage>(line);
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
                                clients.TryRemove(client, out _);
                                client.Close();
                                Console.Write($"Client logged out: {client.Client.RemoteEndPoint}\r\n");
                                return;

                            // Placeholder for future features
                            case NetworkMessageType.ChatMessage:
                            case NetworkMessageType.ListChats:
                            case NetworkMessageType.CreateChat:
                            case NetworkMessageType.AddMember:
                                Console.Write($"Unhandled message type: {msg.MessageType}\r\n");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write($"Error parsing message: {ex.Message}\r\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write($"Error with {client.Client.RemoteEndPoint}: {ex.Message}\r\n");
            }
            finally
            {
                clients.TryRemove(client, out _);
                client.Close();
                Console.Write($"Client disconnected: {client.Client.RemoteEndPoint}\r\n");
            }
        }

        private static async Task HandleSignUp(TcpClient client, User user)
        {
            var existing = database.GetUserByName(user.Name);
            if (existing != null)
            {
                await SendResponse(client, new NetworkMessage
                {
                    MessageType = NetworkMessageType.Error,
                    Payload = JsonSerializer.Serialize("User already exists!")
                });
                return;
            }

            user.HashedPassword = Security.HashPassword(user.HashedPassword);
            await database.InsertUser(user);
            await SendResponse(client, new NetworkMessage
            {
                MessageType = NetworkMessageType.Signup,
                Payload = JsonSerializer.Serialize("Signup successful")
            });

            Console.Write($"User signed up: {user.Name}\r\n");
        }

        private static async Task HandleLogIn(TcpClient client, User user)
        {
            var storedUser = database.GetUserByName(user.Name);
            if (storedUser == null)
            {
                await SendResponse(client, new NetworkMessage
                {
                    MessageType = NetworkMessageType.Error,
                    Payload = JsonSerializer.Serialize("User not found")
                });
                return;
            }

            bool valid = Security.VerifyPassword(user.HashedPassword, storedUser.HashedPassword);
            if (!valid)
            {
                await SendResponse(client, new NetworkMessage
                {
                    MessageType = NetworkMessageType.Error,
                    Payload = JsonSerializer.Serialize("Invalid password")
                });
                return;
            }

            await SendResponse(client, new NetworkMessage
            {
                MessageType = NetworkMessageType.Login,
                Payload = JsonSerializer.Serialize(storedUser)
            });

            Console.Write($"User logged in: {user.Name}\r\n");
        }

        private static async Task BroadcastMessage(NetworkMessage msg)
        {
            string serialized = JsonSerializer.Serialize(msg);

            foreach (var clientWriter in clients.Values)
            {
                try
                {
                    await clientWriter.WriteLineAsync(serialized);
                }
                catch
                {
                    /* Ignore */
                }
            }

            Console.Write("Broadcasted message to all clients\r\n");
        }

        private static async Task SendResponse(TcpClient client, NetworkMessage msg)
        {
            if (clients.TryGetValue(client, out StreamWriter writer))
            {
                string json = JsonSerializer.Serialize(msg);
                await writer.WriteLineAsync(json);
            }
        }
    }
}
