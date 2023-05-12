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
    
    public async Task RegisterAsync(UserRegistrationModel userRegistrationModel)
    {
        await using var connection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));
        // TODO: Create User
        var users = await connection.QueryAsync<UserEntity>("SELECT * FROM management.\"User\"");
    }
}