using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using libarary.Data;
using libarary.Models;

namespace libarary.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class IQueryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public IQueryController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/IQuery
        [HttpPost]
        public async Task<IActionResult> PostIQuery([FromBody] IQuery iQuery)
        {
            if (iQuery == null)
            {
                return BadRequest("Invalid input data.");
            }

            // Assign DateTime values for Time and Date
            iQuery.Time = DateTime.Now.ToString("HH:mm:ss");
            iQuery.Date = DateTime.Now.ToString("yyyy-MM-dd");

            // Add to the database
            _context.IQuery.Add(iQuery); // Changed from _context.IQueries.Add(iQuery)
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIQuery", new { id = iQuery.QueryId }, iQuery);
        }

        // GET: api/IQuery/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IQuery>> GetIQuery(int id)
        {
            var iQuery = await _context.IQuery.FindAsync(id); // Changed from _context.IQueries

            if (iQuery == null)
            {
                return NotFound();
            }

            return iQuery;
        }

        // GET: api/IQuery/user/1
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IQueryable<IQuery>>> GetIQueriesByUserId(int userId)
        {
            var userQueries = _context.IQuery.Where(q => q.UserId == userId.ToString()); // Changed from _context.IQueries

            if (!userQueries.Any())
            {
                return NotFound($"No queries found for user with ID {userId}.");
            }

            return Ok(userQueries);
        }

        // PUT: api/IQuery/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIQuery(int id, [FromBody] IQuery iQuery)
        {
            if (id != iQuery.QueryId)
            {
                return BadRequest();
            }

            _context.Entry(iQuery).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IQueryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/IQuery/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIQuery(int id)
        {
            var iQuery = await _context.IQuery.FindAsync(id); // Changed from _context.IQueries
            if (iQuery == null)
            {
                return NotFound();
            }

            _context.IQuery.Remove(iQuery); // Changed from _context.IQueries
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IQueryExists(int id)
        {
            return _context.IQuery.Any(e => e.QueryId == id); // Changed from _context.IQueries
        }

        // GET: api/IQuery
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IQuery>>> GetAllIQueries()
        {
            return await _context.IQuery.ToListAsync(); // Changed from _context.IQueries
        }
    }

}
