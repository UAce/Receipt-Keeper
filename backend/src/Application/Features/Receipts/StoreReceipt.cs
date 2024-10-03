using System.Security.Claims;
using Application.Endpoints;
using Application.Features.Receipts;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace Application.Features.Users;

public static class StoreReceipt
{
    public record StoreReceiptRequest(string FirstName, string LastName, string Email);

    public record StoreReceiptResponse(Guid Id, string FirstName, string LastName, string Email);

    public class Endpoint : ReceiptsEndpoint, IEndpoint
    {
        public void MapRoute(IEndpointRouteBuilder app)
        {
            app.MapPost("/", Handler).WithName("StoreReceipt").Produces<StoreReceiptResponse>(201);
        }
    }

    [Authorize]
    public static async Task<IResult> Handler(HttpContext context, StoreReceiptRequest request, IUserRepository userRepository)
    {
        Console.WriteLine("Storing Receipts");

        // Obtain the IdentityId from the authenticated user
        var identities = context.User.Identities;
        var identityId = identities.FirstOrDefault()?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (identityId == null)
        {
            Console.WriteLine("Unauthorized: User is not authenticated");
            return Results.Unauthorized();
        }

        User registeredUser = await userRepository.RegisterAsync(
            new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                IdentityId = identityId,
            }
        );

        return Results.Ok(
            new StoreReceiptResponse(registeredUser.Id, registeredUser.FirstName, registeredUser.LastName, registeredUser.Email)
        );
    }
}
