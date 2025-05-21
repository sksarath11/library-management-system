namespace libarary.Models 
{
    public class IQuery
    {
        public int QueryId { get; set; }
        public string? UserId { get; set; }
        public int? BookId { get; set; }    
        public string? Message { get; set; } 
        public string? Date { get; set; }    
        public string? Time { get; set; }  
          
    }
}
