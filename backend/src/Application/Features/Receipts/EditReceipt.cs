using Application.Endpoints;
using Application.Services;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.Receipts;

public static class EditReceipt
{
    public record EditReceiptApiRequest(
        decimal? Total,
        string? Note,
        DateTimeOffset? PrintedAt,
        string? CurrencyCode,
        Guid? MerchantId
    ) { };

    public class EditReceiptValidator : AbstractValidator<EditReceiptApiRequest>
    {
        public EditReceiptValidator()
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
        }
    }

    public record EditReceiptResponse(
        Guid Id,
        decimal Total,
        string Note,
        DateTimeOffset PrintedAt,
        string CurrencyCode,
        Guid MerchantId
    );

    public class Endpoint : ReceiptsEndpoint, IEndpoint
    {
        public void MapRoute(IEndpointRouteBuilder app)
        {
            app.MapPut("/{id}", Handler).WithName("EditReceipt").Produces<EditReceiptResponse>(201);
        }
    }

    [Authorize]
    public static async Task<IResult> Handler(
        [FromRoute] Guid id,
        EditReceiptApiRequest request,
        IReceiptRepository receiptRepository,
        IValidator<EditReceiptApiRequest> validator,
        IHttpContextService httpContextService
    )
    {
        Console.WriteLine("Editing Receipts");

        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(
                validationResult
                    .Errors.GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
            );
        }

        Receipt? receipt = await receiptRepository.GetReceiptAsync(id);

        if (receipt == null)
        {
            return Results.NotFound();
        }

        if (receipt.UserId != httpContextService.CurrentUser.Id)
        {
            return Results.Forbid();
        }

        if (
            request.Total == null
            && request.Note == null
            && request.PrintedAt == null
            && request.CurrencyCode == null
            && request.MerchantId == null
        )
        {
            return Results.Ok(
                new
                {
                    receipt.Id,
                    receipt.Total,
                    receipt.CurrencyCode,
                }
            );
        }

        Receipt editedReceipt = await receiptRepository.EditReceiptAsync(
            new EditReceiptRequest(
                receipt.Id,
                request.Total,
                request.Note,
                request.PrintedAt,
                request.CurrencyCode,
                request.MerchantId
            )
        );

        return Results.Ok(
            new
            {
                editedReceipt.Id,
                editedReceipt.Total,
                editedReceipt.Note,
                editedReceipt.PrintedAt,
                editedReceipt.CurrencyCode,
                editedReceipt.MerchantId,
            }
        );
    }
}
