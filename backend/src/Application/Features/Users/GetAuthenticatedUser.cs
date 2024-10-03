using System.Security.Claims;
using Application.Endpoints;
using Application.Services;
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
    public static IResult Handler(IHttpContextService userContextService)
    {
        Console.WriteLine("Get authenticated user");

        User? currentUser = userContextService.CurrentUser;
        if (currentUser == null)
        {
            Console.WriteLine("Unauthorized: User is not authenticated");
            return Results.Unauthorized();
        }

        return Results.Ok(
            new GetAuthenticatedUserResponse(
                currentUser.Id,
                currentUser.FirstName,
                currentUser.LastName,
                currentUser.Email
            )
        );
    }
}
