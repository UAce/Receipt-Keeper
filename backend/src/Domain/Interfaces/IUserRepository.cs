using Domain.Models;

namespace Domain.Interfaces;

public interface IUserRepository
{
    public Task<User> RegisterAsync(User user);
    public Task<User?> GetUserAsync(string externalId);
}