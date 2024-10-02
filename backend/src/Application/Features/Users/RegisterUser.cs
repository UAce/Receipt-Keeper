using Application.Endpoints;
using Domain.Models;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Application.Features.Users;

public static class RegisterUser {

  public record RegisterUserRequest(string FirstName, string LastName, string Email);
  public record RegisterUserResponse(Guid Id, string FirstName, string LastName, string Email);

  public class Endpoint : IEndpoint
  {
      public void Map(IEndpointRouteBuilder app)
      {
        app.MapPost("/user/register", Handler).WithTags("Users").WithName("RegisterUser").Produces<RegisterUserResponse>(201);
      }
  }

  [Authorize]
  public static async Task<IResult> Handler(HttpContext context, RegisterUserRequest request, IUserRepository userRepository)
  {
    Console.WriteLine("Registering user...");

    // Obtain the IdentityId from the authenticated user
    var identities = context.User.Identities;
    var identityId = identities.FirstOrDefault()?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

    if (identityId == null)
    {
      Console.WriteLine("Unauthorized: User is not authenticated");
      return Results.Unauthorized();
    }

    User registeredUser = await userRepository.RegisterAsync(new User {
      FirstName = request.FirstName,
      LastName = request.LastName,
      Email = request.Email,
      IdentityId = identityId
    });
    
    return Results.Ok(new RegisterUserResponse(registeredUser.Id, registeredUser.FirstName, registeredUser.LastName, registeredUser.Email));
  }

}