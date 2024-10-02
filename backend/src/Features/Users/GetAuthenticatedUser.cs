using Features.Endpoints;
using Domain.Models;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Features.Users;

public static class GetAuthenticatedUser {

  public record GetAuthenticatedUserResponse(Guid Id, string FirstName, string LastName, string Email);

  public class Endpoint : IEndpoint
  {
      public void Map(IEndpointRouteBuilder app)
      {
        app.MapGet("/user", Handler).WithTags("Users").WithName("GetAuthenticatedUser").Produces<GetAuthenticatedUserResponse>(200);
      }
  }

  [Authorize]
  public static async Task<IResult> Handler(HttpContext context, IUserRepository userRepository)
  {
    Console.WriteLine("Get authenticated user");

    // Obtain the IdentityId from the authenticated user
    var identities = context.User.Identities;
    var identityId = identities.FirstOrDefault()?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    User? authenticatedUser = identityId == null ? null : await userRepository.GetUserAsync(identityId);

    if (authenticatedUser == null)
    {
      Console.WriteLine("NotFound: User has not been registered");
      return Results.NotFound();
    }

    return Results.Ok(new GetAuthenticatedUserResponse(authenticatedUser.Id, authenticatedUser.FirstName, authenticatedUser.LastName, authenticatedUser.Email));
  }

}