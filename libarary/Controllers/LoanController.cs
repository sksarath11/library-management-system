using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using libarary.Data;
using libarary.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace libarary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoanController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LoanController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Loan
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Loan>>> GetLoans()
        {
            return await _context.Loans.ToListAsync();
        }

        // GET: api/Loan/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Loan>> GetLoan(int id)
        {
            var loan = await _context.Loans.FindAsync(id);

            if (loan == null)
            {
                return NotFound();
            }

            return loan;
        }

        // POST: api/Loan
        [HttpPost]
        public async Task<ActionResult<Loan>> PostLoan([FromBody] Loan loan)
        {
            if (loan == null)
            {
                return BadRequest("Invalid input data.");
            }

            // Set the loan status as 'Active' when it is created
            loan.Status = "Active";

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLoan", new { id = loan.LoanID }, loan);
        }

        // PUT: api/Loan/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoan(int id, [FromBody] Loan loan)
        {
            if (id != loan.LoanID)
            {
                return BadRequest();
            }

            _context.Entry(loan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanExists(id))
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

        // DELETE: api/Loan/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound();
            }

            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoanExists(int id)
        {
            return _context.Loans.Any(e => e.LoanID == id);
        }
    }
}
