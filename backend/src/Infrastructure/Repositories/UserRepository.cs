using System.Data;
using Dapper;
using Domain.Interfaces;
using Domain.Models;

namespace Infrastructure.Repositories;

public class UserRepository(IDbConnection dbConnection) : BaseRepository(dbConnection), IUserRepository
{
    public async Task<User?> GetUserAsync(string identityId)
    {
        const string query = @"
            SELECT * FROM ""User"" where ""IdentityId"" = @IdentityId
        ";

        return await _dbConnection.QuerySingleOrDefaultAsync<User>(query, new {IdentityId = identityId});
    }

    public async Task<User> RegisterAsync(User user)
    {
        const string query = @"
            INSERT INTO ""User"" (
               ""Id"", ""FirstName"", ""LastName"", ""Email"", ""IdentityId""
            ) 
            VALUES 
                (
                    gen_random_uuid(), 
                    @FirstName, 
                    @LastName, 
                    @Email, 
                    @IdentityId
                ) RETURNING *
        ";
        
        // No need to use using statement. Dapper will automatically
        // open, close and dispose the connection for you.
        return await _dbConnection.QueryFirstAsync<User>(
            query,
            new { user.FirstName, user.LastName, user.Email, user.IdentityId });

    }
}