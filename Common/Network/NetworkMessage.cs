using System.Text.Json;

namespace Common.Network
{
    public class NetworkMessage
    {
        public NetworkMessageType MessageType { get; set; }
        public string Payload { get; set; }

        public NetworkMessage() { }
        public NetworkMessage(NetworkMessageType messageType, object payload = null)
        {
            MessageType = messageType;
            if (payload != null)
                Payload = JsonSerializer.Serialize(payload);
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public T GetPayload<T>()
        {
            if (string.IsNullOrEmpty(Payload)) return default(T);
            return JsonSerializer.Deserialize<T>(Payload);
        }
    }
}
