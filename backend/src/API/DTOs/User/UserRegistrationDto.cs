using System.ComponentModel.DataAnnotations;

namespace API.DTOs.User;

public class UserRegistrationDto
{
    public UserRegistrationDto(string firstName, string lastName, string email, string externalId)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        ExternalId = externalId;
    }

    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string ExternalId { get; set; }
}