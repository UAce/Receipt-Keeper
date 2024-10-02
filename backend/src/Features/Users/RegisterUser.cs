using Features.Endpoints;
using Domain.Models;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;

namespace Features.Users;

public static class RegisterUser {

  public record RegisterUserRequest(string FirstName, string LastName, string Email, string ExternalId);
  public record RegisterUserResponse(Guid Id, string FirstName, string LastName, string Email, string ExternalId);

  public class Endpoint : IEndpoint
  {
      public void Map(IEndpointRouteBuilder app)
      {
        app.MapPost("/user/register", Handler).WithTags("Users").WithName("RegisterUser").Produces<RegisterUserResponse>(201)
        .AddEndpointFilter(async (context, next) => {
          var userRequest = context.GetArgument<RegisterUserRequest>(0);
          var externalId = userRequest.ExternalId;

          // Validate that the externalId we are trying to register matches the claims
          var identities = context.HttpContext.User.Identities;
          var authenticatedIdentity = identities.FirstOrDefault((identity)=> identity.HasClaim((claim) => claim.Type == ClaimTypes.NameIdentifier && claim.Value == externalId));

          if (authenticatedIdentity == null)
          {
            Console.WriteLine("Unauthorized: User cannot register another user");
            return Results.Unauthorized();
          }

          return await next(context);
        });
      }
  }

  [Authorize]
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