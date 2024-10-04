using Application.Endpoints;
using Application.Services;
using Domain.Interfaces;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace Application.Features.Receipts;

public static class StoreReceipt
{
    public record StoreReceiptRequest(
        decimal Total,
        string CurrencyCode,
        string Note,
        Guid MerchantId
    ) { };

    public class StoreReceiptValidator : AbstractValidator<StoreReceiptRequest>
    {
        public StoreReceiptValidator()
        {
            RuleFor(x => x.Total).GreaterThan(0).WithMessage("Total must be greater than 0.");
            RuleFor(x => x.CurrencyCode)
                .NotEmpty()
                .NotNull()
                .WithMessage("Currency code is required.")
                .Length(3)
                .WithMessage("Currency code must be 3 characters long.");
            RuleFor(x => x.Note)
                .MaximumLength(255)
                .WithMessage("Note cannot exceed 255 characters.");
            RuleFor(x => x.MerchantId).NotEmpty().NotNull().WithMessage("Merchant ID is required.");
        }
    }

    public record StoreReceiptResponse(Guid Id, decimal Total, string CurrencyCode);

    public class Endpoint : ReceiptsEndpoint, IEndpoint
    {
        public void MapRoute(IEndpointRouteBuilder app)
        {
            app.MapPost("/", Handler).WithName("StoreReceipt").Produces<StoreReceiptResponse>(201);
        }
    }

    [Authorize]
    public static async Task<IResult> Handler(
        StoreReceiptRequest request,
        IReceiptRepository receiptRepository,
        IValidator<StoreReceiptRequest> validator,
        IHttpContextService httpContextService
    )
    {
        Console.WriteLine("Storing Receipts");

        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(
                validationResult
                    .Errors.GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
            );
        }

        Receipt newReceipt = await receiptRepository.StoreReceiptAsync(
            new Receipt
            {
                Total = request.Total,
                Note = request.Note,
                CurrencyCode = request.CurrencyCode,
                MerchantId = request.MerchantId,
                UserId = httpContextService.CurrentUser.Id,
            }
        );

        return Results.Ok(
            new
            {
                newReceipt.Id,
                newReceipt.Total,
                newReceipt.CurrencyCode,
            }
        );
    }
}
