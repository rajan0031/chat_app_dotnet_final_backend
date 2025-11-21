using ChatApp.Data;
using ChatApp.DTOs;
using ChatApp.Models;
using Microsoft.AspNetCore.Mvc;


namespace ChatApp.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly ChatDbContext _db;
        public UsersController(ChatDbContext db) { _db = db; }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserName)) return BadRequest("UserName required");
            var exists = _db.Users.Any(u => u.UserName == dto.UserName);
            if (exists) return Conflict("UserName already taken");

            var user = new User { UserName = dto.UserName, Password = dto.Password };
            _db.Users.Add(user);
            _db.SaveChanges();
            return Ok(user);
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_db.Users.OrderBy(u => u.UserName).ToList());


        // login user controller 

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserName) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("UserName and Password required");

            var result = _db.Users.FirstOrDefault(u => u.UserName == dto.UserName && u.Password == dto.Password);
            if (result == null)
                return Unauthorized(new { message = "Invalid credentials" });

            return Ok(result);
        }

    }
}
