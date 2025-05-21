using libarary.Data;
using libarary.Models;

namespace libarary.Services
{
    public class IssueService
    {
        private readonly AppDbContext _context;

        public IssueService(AppDbContext context)
        {
            _context = context;
        }

        public void IssueBook(int bookId, int memberId)
        {
            var book = _context.Books.Find(bookId);
            if (book == null || !book.IsAvailable) throw new InvalidOperationException("Book not available.");

            var issue = new IssueRecord
            {
                BookId = bookId,
                MemberId = memberId,
                IssueDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(14)  // 2-week loan
            };

            book.IsAvailable = false;
            _context.IssueRecords.Add(issue);
            _context.SaveChanges();
        }

        public void ReturnBook(int bookId)
        {
            var record = _context.IssueRecords.FirstOrDefault(r => r.BookId == bookId && r.ReturnDate == null);
            if (record == null) throw new InvalidOperationException("This book was not issued.");

            record.ReturnDate = DateTime.Now;

            if (record.ReturnDate > record.DueDate)
            {
                record.FineAmount = (decimal)(record.ReturnDate.Value - record.DueDate).TotalDays * 10;
            }

            var book = _context.Books.Find(bookId);
            if (book != null) book.IsAvailable = true;

            _context.SaveChanges();
        }
    }
}
