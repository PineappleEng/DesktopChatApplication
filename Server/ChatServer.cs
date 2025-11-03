using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using Common.Models;
using Common.Network;
using Server.Database;

namespace Server
{
    internal class ChatServer
    {
        private static 
            ConcurrentDictionary<TcpClient, StreamWriter> clients 
            = new ConcurrentDictionary<TcpClient, StreamWriter>();
        static async Task Main()
        {
            ChatDatabase database = new ChatDatabase();
            IPAddress localAddr = IPAddress.Parse("10.0.2.15");
            int port = 6969;

            TcpListener server = new TcpListener(localAddr, port);
            server.Start();
            Console.Write($"Chat Server Listening on {port}\r\n");

            while (true)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                Console.Write($"Client Connected: {client.Client.RemoteEndPoint}\r\n");
                _ = Task.Run(() => HandleClient(client));
            }
        }

        static async Task HandleClient(TcpClient client)
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
                        // I'll write this later cause I'm lazy
                    }
                }
            }
            catch(Exception ex)
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
    }
}
