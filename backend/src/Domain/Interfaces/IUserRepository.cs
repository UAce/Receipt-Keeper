using Domain.Entities;
using Domain.Models;

namespace Domain.Interfaces;

public interface IUserRepository
{
    public Task<CurrentUser> RegisterUserAsync(UserEntity user);
    public Task<CurrentUser?> GetUserAsync(string identityId);
}
