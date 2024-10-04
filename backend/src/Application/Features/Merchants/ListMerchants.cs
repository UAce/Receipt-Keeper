using Application.Endpoints;
using Application.Services;
using Domain.DTOs;
using Domain.Interfaces;
using Domain.Models;
using Domain.Types;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.Merchants;

public static class ListMerchants
{
    public record ListMerchantsQueryParams(string Search) { };

    public class ListMerchantsValidator : AbstractValidator<ListMerchantsQueryParams>
    {
        public ListMerchantsValidator()
        {
            // TODO
        }
    }

    public class Endpoint : MerchantsEndpoint, IEndpoint
    {
        public void MapRoute(IEndpointRouteBuilder app)
        {
            app.MapGet("/", Handler).WithName("ListMerchants").Produces<List<Merchant>>(200);
            ;
        }
    }

    [Authorize]
    public static async Task<IResult> Handler(
        [FromQuery] string? Search,
        IMerchantRepository merchantRepository,
        // IValidator<ListMerchantsQueryParams> validator,
        IHttpContextService httpContextService
    )
    {
        Console.WriteLine("Listing Merchants");

        List<Merchant> merchants = await merchantRepository.ListMerchantsAsync(
            new ListMerchantsFilter(httpContextService.CurrentUser.Id, Search)
        );

        return Results.Ok(merchants);
    }
}
