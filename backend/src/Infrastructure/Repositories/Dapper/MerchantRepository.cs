using System.Data;
using Dapper;
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
                    @UserId,
                ) RETURNING *
        ";

        return await _dbConnection.QueryFirstAsync<Merchant>(
            query,
            new { merchant.Name, merchant.UserId }
        );
    }
}
