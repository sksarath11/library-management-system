
namespace libarary.Models{
    public class Book
    {
        public int BookId { get; set; }  // Primary Key
        public string BookNumber { get; set; } = string.Empty;
        public string Classification { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;  
        public string Publisher { get; set; } = string.Empty;
        public bool IsReference { get; set; }
        public string CopyCode { get; set; } = "Original";
        public bool IsAvailable { get; set; } = true;  
        public DateTime PurchaseDate { get; set; }

        

        
    }
}

