using Core.Entities.User;

namespace Core.Models.User;

public class UserModel
{
    public Guid Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }

    public string Email { get; set; }
    
    public string FullName => $"{FirstName} {LastName}";
}