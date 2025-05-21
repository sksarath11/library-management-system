
using libarary.Data;
using libarary.Models;
using libarary.Services;
using Microsoft.EntityFrameworkCore;
namespace libarary.Services
{
    public class DbTaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public DbTaskService(AppDbContext context)
        {
            _context = context;
        }

        // Add a new Todo (Book)
        public Todo AddTodo(Todo task)
        {
            string prefix = new string(task.BookNumber.TakeWhile(c => !char.IsDigit(c)).ToArray());
            string numberPart = new string(task.BookNumber.SkipWhile(c => !char.IsDigit(c)).ToArray());

            int startNumber = int.Parse(numberPart);
            for (int i = 0; i < 10; i++)
            {
                var book = new Book
                {
                    // BookId is omitted, DB will auto-assign
                    BookNumber = $"{prefix}{(startNumber + i).ToString("D3")}",
                    Classification = task.Classification,
                    Author = task.Author,
                    Publisher = task.Publisher,
                    Title = task.Title,
                    IsAvailable = task.IsAvailable,
                    IsReference = task.IsReference
                };

                Console.WriteLine("Adding Book Details:");
                Console.WriteLine($"Book Number: {book.BookNumber}");
                Console.WriteLine($"Classification: {book.Classification}");
                Console.WriteLine($"Title: {book.Title}");
                Console.WriteLine($"Author: {book.Author}");
                Console.WriteLine($"Publisher: {book.Publisher}");
                Console.WriteLine($"Is Reference: {book.IsReference}");
                Console.WriteLine($"Is Available: {book.IsAvailable}");

                _context.Books.Add(book);
            }

            _context.SaveChanges();
            return task;
        }

        // Delete a Todo (Book) by ID
        public void DeleteById(int BookId)
        {
            var book = _context.Books.SingleOrDefault(b => b.BookId == BookId);
            if (book == null)
            {
                throw new InvalidOperationException($"No book found with BookId {BookId}.");
            }

            _context.Books.Remove(book);
            _context.SaveChanges();
        }

        // Get a specific Todo (Book) by ID
        public Todo? GetTodoById(int BookId)
        {
            var book = _context.Books.SingleOrDefault(b => b.BookId == BookId);
            return book == null ? null : new Todo(book.BookId, book.BookNumber, book.Classification, book.Title, book.Author, book.Publisher, book.IsReference, book.IsAvailable);
        }

        // Get all Todos (Books)
        public List<Todo> GetTodos()
        {
            return _context.Books
                .Select(b => new Todo(b.BookId, b.BookNumber, b.Classification, b.Title, b.Author, b.Publisher, b.IsReference, b.IsAvailable))
                .ToList();
        }

        // Update a Todo (Book) by ID
        public Todo? UpdateById(int BookId, Todo updatedTodo)
        {
            var existingBook = _context.Books.SingleOrDefault(b => b.BookId == BookId);
            if (existingBook == null)
            {
                return null; // Return null if no matching book is found
            }

            // Update properties
            existingBook.Author = updatedTodo.Author;
            existingBook.Title = updatedTodo.Title;

            

            _context.SaveChanges();
            

            return new Todo(existingBook.BookId, existingBook.BookNumber, existingBook.Classification, existingBook.Title, existingBook.Author, existingBook.Publisher, existingBook.IsReference, existingBook.IsAvailable);
        }

        public async Task AddBookWithCopies(Book input)
        {
            // Get last used book number in this classification
            var lastBook = await _context.Books
                .Where(b => b.Classification == input.Classification)
                .OrderByDescending(b => b.BookNumber)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastBook != null)
            {
                nextNumber = int.Parse(lastBook.BookNumber.Substring(1)) + 1;
            }

            string bookNumber = input.Classification + nextNumber.ToString("D4");

            // Save original book
            var originalBook = new Book
            {
                BookNumber = bookNumber,
                Classification = input.Classification,
                Title = input.Title,
                Publisher = input.Publisher,
                IsReference = input.IsReference,
                CopyCode = "copy",
                IsAvailable = input.IsAvailable // Ensure this is passed
            };

            Console.WriteLine("Adding Book Details:");
            Console.WriteLine($"Book Number: {bookNumber}");
            Console.WriteLine($"Classification: {input.Classification}");
            Console.WriteLine($"Title: {input.Title}");
            Console.WriteLine($"Publisher: {input.Publisher}");
            Console.WriteLine($"Is Reference: {input.IsReference}");
            Console.WriteLine($"Is Available: {input.IsAvailable}");
            _context.Books.Add(originalBook);

            

            // Save all changes in a single operation
            await _context.SaveChangesAsync();
        }

        public void BuyBook(int bookId)
        {
            var book = _context.Books.SingleOrDefault(b => b.BookId == bookId);
            if (book == null)
            {
                throw new InvalidOperationException($"No book found with BookId {bookId}.");
            }

            if (!book.IsAvailable)
            {
                throw new InvalidOperationException("Book is not available for purchase.");
            }

            // Mark the book as unavailable (purchased)
            book.IsAvailable = false;
            _context.SaveChanges();
        }

        // Implement ReserveBook method
        public void ReserveBook(int bookId)
        {
            var book = _context.Books.SingleOrDefault(b => b.BookId == bookId);
            if (book == null)
            {
                throw new InvalidOperationException($"No book found with BookId {bookId}.");
            }

            if (!book.IsAvailable)
            {
                throw new InvalidOperationException("Book is already reserved or sold.");
            }

            // Mark the book as reserved (not available for others)
            book.IsAvailable = false;
            _context.SaveChanges();
        }

        // Implement CancelReservation method
        public void CancelReservation(int bookId)
        {
            var book = _context.Books.SingleOrDefault(b => b.BookId == bookId);
            if (book == null)
            {
                throw new InvalidOperationException($"No book found with BookId {bookId}.");
            }

            if (book.IsAvailable)
            {
                throw new InvalidOperationException("Book is not reserved.");
            }

            // Mark the book as available again
            book.IsAvailable = true;
            _context.SaveChanges();
        }

    }
}
