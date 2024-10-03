using System.Security.Claims;
using Application.Endpoints;
using Application.Services;
using Domain.Interfaces;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace Application.Features.Users;

public static class RegisterUser
{
    public record RegisterUserRequest(string FirstName, string LastName, string Email);

    public record RegisterUserResponse(Guid Id, string FirstName, string LastName, string Email);

    public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required.");
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");
        }
    }

    public class Endpoint : UsersEndpoint, IEndpoint
    {
        public void MapRoute(IEndpointRouteBuilder app)
        {
            app.MapPost("/register", Handler)
                .WithName("RegisterUser")
                .Produces<RegisterUserResponse>(201)
                .ProducesValidationProblem();
            ;
        }
    }

    [Authorize]
    public static async Task<IResult> Handler(
        RegisterUserRequest request,
        IUserRepository userRepository,
        IValidator<RegisterUserRequest> validator,
        IHttpContextService httpContextService
    )
    {
        Console.WriteLine("Registering user");

        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(
                validationResult
                    .Errors.GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
            );
        }

        User registeredUser = await userRepository.RegisterAsync(
            new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                IdentityId = httpContextService.IdentityId,
            }
        );

        return Results.Ok(
            new RegisterUserResponse(
                registeredUser.Id,
                registeredUser.FirstName,
                registeredUser.LastName,
                registeredUser.Email
            )
        );
    }
}
