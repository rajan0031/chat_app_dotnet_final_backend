using ChatApp.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatDbContext _db;
        public ChatHub(ChatDbContext db) { _db = db; }

        // Client calls this after determining roomId (via REST create-or-get)
        public async Task JoinRoom(int roomId, int userId)
        {
            var isParticipant = _db.ChatRoomUsers.Any(x => x.ChatRoomId == roomId && x.UserId == userId);
            if (!isParticipant) throw new HubException("Not a participant of this room");

            await Groups.AddToGroupAsync(Context.ConnectionId, RoomGroup(roomId));

            // send previous history only to caller
            var history = await _db.Messages
                .Include(m => m.Sender)
                .Where(m => m.ChatRoomId == roomId)
                .OrderBy(m => m.SentAt)
                .Select(m => new { m.Id, m.ChatRoomId, m.SenderId, SenderName = m.Sender.UserName, m.Text, m.SentAt })
                .ToListAsync();

            await Clients.Caller.SendAsync("LoadChatHistory", history);
        }

        public async Task SendMessageToRoom(int roomId, int senderId, string text)
        {
            var isParticipant = _db.ChatRoomUsers.Any(x => x.ChatRoomId == roomId && x.UserId == senderId);
            if (!isParticipant) throw new HubException("Not a participant of this room");

            var msg = new ChatApp.Models.Message
            {
                ChatRoomId = roomId,
                SenderId = senderId,
                Text = text,
                SentAt = DateTime.UtcNow
            };
            _db.Messages.Add(msg);
            await _db.SaveChangesAsync();

            await Clients.Group(RoomGroup(roomId))
                .SendAsync("ReceiveMessage", new { roomId, senderId, text, sentAt = msg.SentAt });
        }

        private static string RoomGroup(int roomId) => $"room:{roomId}";
    }
}
