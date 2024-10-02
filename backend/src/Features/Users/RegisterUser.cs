using Features.Endpoints;
using Domain.Models;
using Domain.Interfaces;

namespace Features.Users;

public static class RegisterUser {

  public record RegisterUserRequest(string FirstName, string LastName, string Email, string ExternalId);
  public record RegisterUserResponse(Guid Id, string FirstName, string LastName, string Email, string ExternalId);

  public class Endpoint : IEndpoint
  {
      public void Map(IEndpointRouteBuilder app)
      {
        app.MapPost("/user/register", Handler).WithTags("Users").WithName("RegisterUser").WithOpenApi();
      }
  }

  public static async Task<IResult> Handler(RegisterUserRequest request, IUserRepository userRepository)
  {
    Console.WriteLine("Registering user...");

    User user = new()
    {
      FirstName = request.FirstName,
      LastName = request.LastName,
      Email = request.Email,
      ExternalId = request.ExternalId
    };

    User registeredUser = await userRepository.RegisterAsync(user);
    
    return Results.Ok(new RegisterUserResponse(registeredUser.Id, registeredUser.FirstName, registeredUser.LastName, registeredUser.Email, registeredUser.ExternalId));
  }

}