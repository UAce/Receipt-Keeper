using Features.Endpoints;
using Domain.Models;

namespace Features.Users;

public static class RegisterUser {

  public record RegisterUserRequest(string FirstName, string LastName, string Email, string ExternalId);
  public record RegisterUserResponse(Guid Id, string FirstName, string LastName, string Email, string ExternalId);

  public sealed class Endpoint : IEndpoint
  {
      public void Map(IEndpointRouteBuilder app)
      {
        app.MapPost("/user/register", Handler).WithTags("Users").WithName("RegisterUser").WithOpenApi();
      }
  }

  public static IResult Handler(RegisterUserRequest request)
  {
    User user = new()
    {
      FirstName = request.FirstName,
      LastName = request.LastName,
      Email = request.Email
    };

    // TODO: Save user to database
    
    return Results.Ok(new RegisterUserResponse(user.Id, user.FirstName, user.LastName, user.Email, request.ExternalId));
  }

}