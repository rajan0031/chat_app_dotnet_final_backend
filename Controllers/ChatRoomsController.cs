using ChatApp.Data;
using ChatApp.DTOs;
using ChatApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Controllers
{
    [ApiController]
    [Route("api/rooms")]
    public class ChatRoomsController : ControllerBase
    {
        private readonly ChatDbContext _db;
        public ChatRoomsController(ChatDbContext db) { _db = db; }

       
        private static string MakeRoomKey(int a, int b)
        {
            var min = Math.Min(a, b);
            var max = Math.Max(a, b);
            return $"{min}:{max}";
        }

        [HttpPost("create-or-get")]
        public IActionResult CreateOrGetRoom(CreateRoomDto dto)
        {
            if (dto.UserAId == dto.UserBId) return BadRequest("Users must be different");
            var key = MakeRoomKey(dto.UserAId, dto.UserBId);

            var room = _db.ChatRooms
                .Include(r => r.Participants)
                .FirstOrDefault(r => r.RoomKey == key);

            if (room == null)
            {
                room = new ChatRoom { RoomKey = key };
                _db.ChatRooms.Add(room);
                _db.SaveChanges();

                _db.ChatRoomUsers.AddRange(
                    new ChatRoomUser { ChatRoomId = room.Id, UserId = dto.UserAId },
                    new ChatRoomUser { ChatRoomId = room.Id, UserId = dto.UserBId }
                );
                _db.SaveChanges();
            }

            return Ok(new { roomId = room.Id, roomKey = room.RoomKey });
        }

        [HttpGet("{roomId}/participants")]
        public IActionResult GetParticipants(int roomId)
        {
            var users = _db.ChatRoomUsers
                .Where(x => x.ChatRoomId == roomId)
                .Select(x => x.User)
                .ToList();

            return Ok(users);
        }
    }
}
