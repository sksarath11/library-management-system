using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite; // Use SqliteParameter for SQLite
using Microsoft.EntityFrameworkCore;
using libarary.Data;

namespace libarary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserBooksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserBooksController(AppDbContext context)
        {
            _context = context;
        }

        public class UserBookRequest
        {
            public int UserId { get; set; }
            public int BookId { get; set; }
            public DateTime PurchaseDate { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserBook([FromBody] UserBookRequest request)
        {
            if (request == null)
                return BadRequest();

            var sql = "INSERT INTO UserBooks (UserId, BookId, PurchaseDate) VALUES (@UserId, @BookId, @PurchaseDate)";

            var parameters = new[]
            {
                new SqliteParameter("@UserId", request.UserId),
                new SqliteParameter("@BookId", request.BookId),
                new SqliteParameter("@PurchaseDate", request.PurchaseDate)
            };

            // Using ExecuteSqlRawAsync for async execution with Sqlite
            await _context.Database.ExecuteSqlRawAsync(sql, parameters);

            return Ok(new { message = "UserBook entry created successfully." });
        }
        [HttpGet("all")]
        public IActionResult GetAllUserBooks()
        {
            var userBooks = _context.UserBooks
                .Select(ub => new
                {
                    ub.UserId,
                    ub.BookId,
                    ub.PurchaseDate
                })
                .ToList();

            return Ok(userBooks);
        }
        
        [HttpGet("user/{userId}")]
        public IActionResult GetUserBooksByUserId(int userId)
        {
            var userBooks = _context.UserBooks
                .Where(ub => ub.UserId == userId)
                .Select(ub => new
                {
                    ub.UserId,
                    ub.BookId,
                    ub.PurchaseDate
                })
                .ToList();

            if (userBooks.Count == 0)
            {
                return NotFound(new { message = $"No books found for User ID {userId}." });
            }

            return Ok(userBooks);
        }

        [HttpDelete("book/{bookId}")]
        public async Task<IActionResult> DeleteUserBookByBookId(int bookId)
        {
            var sql = "DELETE FROM UserBooks WHERE BookId = @BookId";

            var parameters = new[]
            {
                new SqliteParameter("@BookId", bookId)
            };

            var result = await _context.Database.ExecuteSqlRawAsync(sql, parameters);

            if (result == 0)
            {
                return NotFound(new { message = $"No entry found for Book ID {bookId}." });
            }

            return Ok(new { message = "UserBook entry deleted successfully by Book ID." });
        }


    }
}
