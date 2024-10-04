using System.Data;
using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;

namespace Infrastructure.Repositories;

public class UserRepository(IDbConnection dbConnection)
    : BaseRepository(dbConnection),
        IUserRepository
{
    public async Task<CurrentUser?> GetUserAsync(string identityId)
    {
        const string query =
            @"
            SELECT * FROM ""User"" where ""IdentityId"" = @IdentityId
        ";

        return await _dbConnection.QuerySingleOrDefaultAsync<CurrentUser>(
            query,
            new { IdentityId = identityId }
        );
    }

    public async Task<CurrentUser> RegisterUserAsync(UserEntity user)
    {
        const string query =
            @"
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
        return await _dbConnection.QueryFirstAsync<CurrentUser>(
            query,
            new
            {
                user.FirstName,
                user.LastName,
                user.Email,
                user.IdentityId,
            }
        );
    }
}
