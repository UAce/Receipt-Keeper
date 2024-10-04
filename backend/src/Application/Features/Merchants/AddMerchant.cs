using Application.Endpoints;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace Application.Features.Merchants;

public static class AddMerchant
{
    public record AddMerchantRequest(string Name) { };

    public class AddMerchantValidator : AbstractValidator<AddMerchantRequest>
    {
        public AddMerchantValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull()
                .MaximumLength(50)
                .WithMessage("Merchant Name cannot exceed 50 characters.");
        }
    }

    public record AddMerchantResponse(Guid Id, decimal Total, string CurrencyCode);

    public class Endpoint : MerchantsEndpoint, IEndpoint
    {
        public void MapRoute(IEndpointRouteBuilder app)
        {
            app.MapPost("/", Handler).WithName("AddMerchant").Produces<AddMerchantResponse>(201);
        }
    }

    [Authorize]
    public static async Task<IResult> Handler(
        AddMerchantRequest request,
        IMerchantRepository merchantRepository,
        IValidator<AddMerchantRequest> validator,
        IHttpContextService httpContextService
    )
    {
        Console.WriteLine("Add Merchant");

        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(
                validationResult
                    .Errors.GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
            );
        }

        Merchant merchant = await merchantRepository.AddMerchantAsync(
            new MerchantEntity { Name = request.Name, UserId = httpContextService.CurrentUser.Id }
        );

        return Results.Ok(new { merchant.Id, merchant.Name });
    }
}
