using ChatApp.Data;
using ChatApp.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessagesController : ControllerBase
    {
        private readonly ChatDbContext _db;
        public MessagesController(ChatDbContext db) { _db = db; }

        [HttpGet("{roomId}")]
        public IActionResult GetByRoom(int roomId)
        {
            var messages = _db.Messages
                .Include(m => m.Sender)
                .Where(m => m.ChatRoomId == roomId)
                .OrderBy(m => m.SentAt)
                .Select(m => new DTOs.MessageDto
                {
                    Id = m.Id,
                    ChatRoomId = m.ChatRoomId,
                    SenderId = m.SenderId,
                    SenderName = m.Sender.UserName,
                    Text = m.Text,
                    SentAt = m.SentAt
                })
                .ToList();

            return Ok(messages);
        }

        [HttpPost]
        public IActionResult Send(SendMessageDto dto)
        {
            var inRoom = _db.ChatRoomUsers.Any(x => x.ChatRoomId == dto.ChatRoomId && x.UserId == dto.SenderId);
            if (!inRoom) return Forbid("Sender is not a participant of this room");

            var msg = new ChatApp.Models.Message
            {
                ChatRoomId = dto.ChatRoomId,
                SenderId = dto.SenderId,
                Text = dto.Text,
                SentAt = DateTime.UtcNow
            };
            _db.Messages.Add(msg);
            _db.SaveChanges();

            return Ok(msg);
        }
    }
}
