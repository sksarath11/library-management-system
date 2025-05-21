using libarary.Data;
using libarary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace libarary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperUserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SuperUserController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/superuser/checkUser
        [HttpPost("checkUser")]
        public IActionResult CheckUser([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Username and Password are required.");
            }

            var librarian = _context.Librarians
                                    .FirstOrDefault(l => l.Username == request.Username && l.Password == request.Password);

            if (librarian == null)
            {
                return NotFound("User not found or invalid credentials.");
            }

            return Ok(new
            {
                Message = "User found!",
                Username = librarian.Username,
                Password = librarian.Password
            });
        }
    }

    public class LoginRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
