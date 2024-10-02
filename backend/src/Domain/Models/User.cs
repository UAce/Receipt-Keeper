namespace Domain.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }

    public required string Email { get; set; }
    
    public string FullName => $"{FirstName} {LastName}";
}