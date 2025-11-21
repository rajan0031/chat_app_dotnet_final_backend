namespace ChatApp.Models
{
    public class ChatRoom
    {
        public int Id { get; set; }
        public string RoomKey { get; set; } = string.Empty; // deterministic two-user key
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ChatRoomUser> Participants { get; set; } = new List<ChatRoomUser>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
