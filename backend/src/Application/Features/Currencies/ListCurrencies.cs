using System.Data;
using Application.Endpoints;
using Application.Services;
using Dapper;
using Domain.DTOs;
using Domain.Interfaces;
using Domain.Models;
using Domain.Types;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.Currencies;

public static class ListCurrencies
{
    public record ListCurrenciesQueryParams(string Search) { };

    public class ListCurrenciesValidator : AbstractValidator<ListCurrenciesQueryParams>
    {
        public ListCurrenciesValidator()
        {
            // TODO
        }
    }

    public class Endpoint : CurrenciesEndpoint, IEndpoint
    {
        public void MapRoute(IEndpointRouteBuilder app)
        {
            app.MapGet("/", Handler).WithName("ListCurrencies").Produces<List<Currency>>(200);
            ;
        }
    }

    [Authorize]
    public static async Task<IResult> Handler(IDbConnection _dbConnection)
    {
        Console.WriteLine("Listing Currencies");

        const string query = @"SELECT * FROM ""Currency""";
        var results = await _dbConnection.QueryAsync<Currency>(query);

        return Results.Ok(results.ToList());
    }
}
