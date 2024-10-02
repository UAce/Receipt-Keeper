namespace Domain.Models;

public class User
{
    public Guid Id { get; set; }
    
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }

    public required string Email { get; set; }

    public required string IdentityId { get; set; }
    
    public string FullName => $"{FirstName} {LastName}";
}