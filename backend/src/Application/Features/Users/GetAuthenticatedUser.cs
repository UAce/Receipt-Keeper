using System.Security.Claims;
using Application.Endpoints;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace Application.Features.Users;

public static class GetAuthenticatedUser
{
    public record GetAuthenticatedUserResponse(
        Guid Id,
        string FirstName,
        string LastName,
        string Email
    );

    public class Endpoint : UsersEndpoint, IEndpoint
    {
        public void MapRoute(IEndpointRouteBuilder app)
        {
            app.MapGet("/me", Handler)
                .WithName("GetAuthenticatedUser")
                .Produces<GetAuthenticatedUserResponse>(200);
        }
    }

    [Authorize]
    public static async Task<IResult> Handler(HttpContext context, IUserRepository userRepository)
    {
        Console.WriteLine("Get authenticated user");

        // Obtain the IdentityId from the authenticated user
        var identities = context.User.Identities;
        var identityId = identities
            .FirstOrDefault()
            ?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
            ?.Value;
        User? authenticatedUser =
            identityId == null ? null : await userRepository.GetUserAsync(identityId);

        if (authenticatedUser == null)
        {
            Console.WriteLine("NotFound: User has not been registered");
            return Results.NotFound();
        }

        return Results.Ok(
            new GetAuthenticatedUserResponse(
                authenticatedUser.Id,
                authenticatedUser.FirstName,
                authenticatedUser.LastName,
                authenticatedUser.Email
            )
        );
    }
}
