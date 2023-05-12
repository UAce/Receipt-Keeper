using Core.Entities;
using Core.Entities.User;
using Core.Interfaces;
using Core.Models.User;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IConfiguration _config;

    public UserRepository(IConfiguration config)
    {
        _config = config;
    }

    public async Task<UserModel> RegisterAsync(UserRegistrationModel userRegistrationModel)
    {
        await using var connection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));
        var user = await connection.QueryFirstAsync<UserEntity>(
            "INSERT INTO management.\"User\" (\"FirstName\", \"LastName\", \"Email\", \"ExternalId\") VALUES (@FirstName, @LastName, @Email, @ExternalId) RETURNING *",
            new {FirstName = userRegistrationModel.FirstName, LastName = userRegistrationModel.LastName, Email = userRegistrationModel.Email, ExternalId = userRegistrationModel.ExternalId});

        return new UserModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }

    public async Task<UserModel?> GetUserAsync(string externalId)
    {
        try
        {
            await using var connection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));
            var user = await connection.QuerySingleOrDefaultAsync<UserEntity>("SELECT * FROM management.\"User\" where \"ExternalId\" = @ExternalId", new {ExternalId = externalId});
            return user != null ? new UserModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            } : null;
        }
        catch (Exception e)
        {
            Console.WriteLine($"UserRepository: Failed to get user with externalId: '{externalId}'. Error: $e");
            throw;
        }
    }
}