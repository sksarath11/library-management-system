namespace libarary.Models
{
    public class Loan
    {
        public int LoanID { get; set; }           // Unique identifier for each loan
        public int? BookId { get; set; }    // Foreign key referencing the Books table
        public int UserId { get; set; }           // Foreign key referencing the Users table
        public DateTime LoanDate { get; set; }    // Date when the book copy was borrowed
        public DateTime DueDate { get; set; }     // Date when the book copy is due for return
        public DateTime? ReturnDate { get; set; } // Date when the book copy was returned (nullable)
        public string? Status { get; set; }        // Status of the loan (e.g., 'Active', 'Returned', 'Overdue')
    }
}
