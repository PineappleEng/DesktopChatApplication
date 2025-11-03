using Common.Network;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.Network
{
    public class ChatClient
    {
        private TcpClient _client;
        private StreamWriter _writer;
        private StreamReader _reader;

        public event Action<NetworkMessage> MessageReceived;
        public event Action Disconnected;

        public async Task Connect(string serverIp, int port)
        {
            _client = new TcpClient();
            await _client.ConnectAsync(serverIp, port);

            var stream = _client.GetStream();
            _writer = new StreamWriter(stream) { AutoFlush = true };
            _reader = new StreamReader(stream);

            _ = Task.Run(Listen);
        }

        public async Task SendMessage(NetworkMessage message)
        {
            string json = JsonSerializer.Serialize(message);
            await _writer.WriteLineAsync(json);
        }

        public async Task Listen()
        {
            try
            {
                string line;
                while ((line = await _reader.ReadLineAsync()) != null)
                {
                    var msg = JsonSerializer.Deserialize<NetworkMessage>(line);
                    if (msg != null)
                        MessageReceived?.Invoke(msg);
                }
            }
            catch
            {
                Disconnected?.Invoke();
            }
        }

        public void Disconnect()
        {
            _client?.Close();
        }
    }
}
