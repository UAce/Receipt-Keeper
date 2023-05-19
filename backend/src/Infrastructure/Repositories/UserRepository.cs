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
    private readonly string? _connectionString;

    public UserRepository(IConfiguration config)
    {
        _config = config;
        _connectionString = _config.GetConnectionString("DefaultConnection");
        Console.WriteLine(_connectionString);
        if (_connectionString == null){
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
    }

    public async Task<UserModel> RegisterAsync(UserRegistrationModel userRegistrationModel)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        var user = await connection.QueryFirstAsync<UserEntity>(
            "INSERT INTO management.\"User\" (\"Id\", \"FirstName\", \"LastName\", \"Email\", \"ExternalId\") VALUES (gen_random_uuid(), @FirstName, @LastName, @Email, @ExternalId) RETURNING *",
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
            await using var connection = new NpgsqlConnection(_connectionString);
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