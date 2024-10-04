using Application.Endpoints;
using Application.Services;
using Domain.DTOs;
using Domain.Interfaces;
using Domain.Models;
using Domain.Types;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.Receipts;

public static class ListReceipts
{
    public record ListReceiptsQueryParams(Pagination Pagination, DateTimeRange? DateTimeRange) { };

    public class ListReceiptsValidator : AbstractValidator<ListReceiptsQueryParams>
    {
        public ListReceiptsValidator()
        {
            // TODO
        }
    }

    public class Endpoint : ReceiptsEndpoint, IEndpoint
    {
        public void MapRoute(IEndpointRouteBuilder app)
        {
            app.MapGet("/", Handler).WithName("ListReceipts").Produces<List<ReceiptListItem>>(201);
            ;
        }
    }

    [Authorize]
    public static async Task<IResult> Handler(
        [FromQuery] int PageNumber,
        [FromQuery] int PageSize,
        [FromQuery] DateTimeOffset? From,
        [FromQuery] DateTimeOffset? To,
        IReceiptRepository receiptRepository,
        IValidator<ListReceiptsQueryParams> validator,
        IHttpContextService httpContextService
    )
    {
        Console.WriteLine("Listing Receipts");

        ListReceiptsQueryParams queryParams =
            new(
                new Pagination(PageNumber, PageSize),
                From != null && To != null
                    ? new DateTimeRange((DateTimeOffset)From, (DateTimeOffset)To)
                    : null
            );

        var validationResult = await validator.ValidateAsync(queryParams);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(
                validationResult
                    .Errors.GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
            );
        }

        List<ReceiptListItem> receipts = await receiptRepository.ListReceiptsAsync(
            new ListReceiptsFilter(
                httpContextService.CurrentUser.Id,
                queryParams.Pagination,
                queryParams.DateTimeRange
            )
        );

        return Results.Ok(receipts);
    }
}
