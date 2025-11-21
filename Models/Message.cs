namespace ChatApp.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; } = null!;

        public int SenderId { get; set; }
        public User Sender { get; set; } = null!;

        public string Text { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
