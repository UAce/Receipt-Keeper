using System.Data;
using Dapper;
using Domain.Interfaces;
using Domain.Models;

namespace Infrastructure.Repositories;

public class UserRepository(IDbConnection dbConnection) : BaseRepository(dbConnection), IUserRepository
{
    public async Task<User?> GetUserAsync(string externalId)
    {
        const string query = @"
            SELECT * FROM ""User"" where ""ExternalId"" = @ExternalId
        ";

        return await _dbConnection.QuerySingleOrDefaultAsync<User>(query, new {ExternalId = externalId});
    }

    public async Task<User> RegisterAsync(User user)
    {
        const string query = @"
            INSERT INTO ""User"" (
               ""Id"", ""FirstName"", ""LastName"", ""Email"", ""ExternalId""
            ) 
            VALUES 
                (
                    gen_random_uuid(), 
                    @FirstName, 
                    @LastName, 
                    @Email, 
                    @ExternalId
                ) RETURNING *
        ";
        
        // No need to use using statement. Dapper will automatically
        // open, close and dispose the connection for you.
        return await _dbConnection.QueryFirstAsync<User>(
            query,
            new { user.FirstName, user.LastName, user.Email, user.ExternalId });

    }
}