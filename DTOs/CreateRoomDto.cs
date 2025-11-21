namespace ChatApp.DTOs
{
    public class RegisterUserDto
    {
        public string UserName { get; set; } = string.Empty;
        public string? Password { get; set; }
    }

    public class CreateRoomDto
    {
        public int UserAId { get; set; }
        public int UserBId { get; set; }
    }

    public class SendMessageDto
    {
        public int ChatRoomId { get; set; }
        public int SenderId { get; set; }
        public string Text { get; set; } = string.Empty;
    }

    public class MessageDto
    {
        public int Id { get; set; }
        public int ChatRoomId { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
    }
}
