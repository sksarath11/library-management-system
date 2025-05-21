namespace libarary.Models
{
    public class Librarian
    {
        public int Id { get; set; }            // Primary Key
        public string? Username { get; set; }  // Username
        public string? Password { get; set; }  // Password (later you should hash this for security!)
        
    }
}
