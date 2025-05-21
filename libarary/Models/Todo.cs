namespace libarary.Models
{
    public class Todo
    {
        public int BookId { get; set; }
        public string BookNumber { get; set; }
        public string Classification { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public bool IsReference { get; set; }
        public bool IsAvailable { get; set; }

        // Constructor to map Book to Todo
        public Todo(int bookId, string bookNumber, string classification, string title, string author, string publisher, bool isReference, bool isAvailable)
        {
            BookId = bookId;
            BookNumber = bookNumber;
            Classification = classification;
            Title = title;
            Author = author;
            Publisher = publisher;
            IsReference = isReference;
            IsAvailable = isAvailable;
        }
    }
}
