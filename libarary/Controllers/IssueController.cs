using libarary.Services;
using Microsoft.AspNetCore.Mvc;

namespace libarary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        private readonly IssueService _issueService;

        public IssueController(IssueService issueService)
        {
            _issueService = issueService;
        }

        [HttpPost("issue")]
        public IActionResult IssueBook([FromQuery] int bookId, [FromQuery] int memberId)
        {
            try
            {
                _issueService.IssueBook(bookId, memberId);
                return Ok("Book issued successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("return")]
        public IActionResult ReturnBook([FromQuery] int bookId)
        {
            try
            {
                _issueService.ReturnBook(bookId);
                return Ok("Book returned successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
