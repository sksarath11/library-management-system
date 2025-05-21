namespace libarary.Models;

public class User
{
    public int UserId { get; set; }  // User Number - Primary Key
    public string Name { get; set; } = string.Empty;
    public string Sex { get; set; } = string.Empty;
    public string NationalIdNumber { get; set; } = string.Empty;  // National Identity Card Number
    public string Address { get; set; } = string.Empty;
    
}  
