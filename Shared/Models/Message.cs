namespace Shared.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public int SenderId { get; set; }
        public string Timestamp { get; set; }
        public string Content { get; set; }
    }
}
