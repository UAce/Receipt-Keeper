namespace Core.Models.User;

public class UserRegistrationModel
{
    public UserRegistrationModel(string firstName, string lastName, string email, string externalId)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        ExternalId = externalId;
    }

    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Email { get; set; }
    
    public string ExternalId { get; set; }
}