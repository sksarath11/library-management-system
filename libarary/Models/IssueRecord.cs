
namespace libarary.Models;
public class IssueRecord
    {
        public int IssueRecordId { get; set; }  // Primary Key
        public int BookId { get; set; }         // Foreign Key
        public int MemberId { get; set; }       // Foreign Key
        public DateTime IssueDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal FineAmount { get; set; } = 0;
    }