using libarary.Data;
using libarary.Models;

namespace libarary.Services
{
    public class MemberService
    {
        private readonly AppDbContext _context;

        public MemberService(AppDbContext context)
        {
            _context = context;
        }

        public Member AddMember(Member member)
        {
            _context.Members.Add(member);
            _context.SaveChanges();
            return member;
        }

        public List<Member> GetAllMembers()
        {
            return _context.Members.ToList();
        }
    }
}
