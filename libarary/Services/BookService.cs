/**using libarary.Data;
using libarary.Models;
using Microsoft.EntityFrameworkCore;

namespace libarary.Services
{
    public class BookService
    {
        private readonly AppDbContext _context;

        public BookService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddBookWithCopies(Book book)
        {
            // Save the original book
            book.CopyCode = "Original";
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            int originalId = book.BookId;

            // Create 9 copies
            for (int i = 1; i <= 9; i++)
            {
                var copy = new Book
                {
                    Title = book.Title,
                    Author = book.Author,
                    CopyCode = $"O{i}"
                };
                _context.Books.Add(copy);
            }

            await _context.SaveChangesAsync();
        }
    }
}
**/