using System.Data;
using Dapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;

namespace Infrastructure.Repositories.Dapper;

public class MerchantRepository(IDbConnection dbConnection)
    : BaseRepository(dbConnection),
        IMerchantRepository
{
    public async Task<Merchant> AddMerchantAsync(MerchantEntity merchant)
    {
        const string query =
            @"
            INSERT INTO ""Merchant"" (
               ""Id"", ""Name"", ""UserId""
            ) 
            VALUES 
                (
                    gen_random_uuid(), 
                    @Name, 
                    @UserId
                ) RETURNING *
        ";

        return await _dbConnection.QueryFirstAsync<Merchant>(
            query,
            new { merchant.Name, merchant.UserId }
        );
    }

    public async Task<List<Merchant>> ListMerchantsAsync(ListMerchantsFilter filter)
    {
        const string query =
            @"
                SELECT
                    *
                FROM
                    ""Merchant"" m
                WHERE
                    m.""UserId"" = @UserId AND (@Search IS NULL OR m.""Name"" ILIKE @Search )
                ;
            ";

        var results = await _dbConnection.QueryAsync<Merchant>(
            query,
            new { filter.UserId, Search = $"{filter.Search}%" }
        );

        return results.ToList();
    }
}
