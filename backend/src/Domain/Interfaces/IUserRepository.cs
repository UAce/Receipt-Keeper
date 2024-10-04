using Domain.Models;

namespace Domain.Interfaces;

public interface IUserRepository
{
    public Task<User> RegisterUserAsync(User user);
    public Task<User?> GetUserAsync(string identityId);
}
