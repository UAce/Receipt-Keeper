namespace Domain.Models;

public class CurrentUser
{
    public required Guid Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string Email { get; set; }

    public required string IdentityId { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}
