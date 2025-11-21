namespace ChatApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;

        public string? Password { get; set; }

        public ICollection<ChatRoomUser> Rooms { get; set; } = new List<ChatRoomUser>();
        public ICollection<Message> MessagesSent { get; set; } = new List<Message>();
    }
}
