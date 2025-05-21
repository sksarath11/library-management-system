using libarary.Models;
using libarary.Services;
using Microsoft.AspNetCore.Mvc;

namespace libarary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly MemberService _memberService;

        public MemberController(MemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpPost("add")]
        public IActionResult AddMember([FromBody] Member member)
        {
            var createdMember = _memberService.AddMember(member);
            return Ok(createdMember);
        }

        [HttpGet("all")]
        public IActionResult GetAllMembers()
        {
            var members = _memberService.GetAllMembers();
            return Ok(members);
        }
    }
}
