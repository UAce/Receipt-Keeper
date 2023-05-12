using Core.Entities.User;
using Core.Models.User;

namespace Core.Interfaces;

public interface IUserRepository
{
    public Task<UserModel> RegisterAsync(UserRegistrationModel userRegistrationModel);
    public Task<UserModel?> GetUserAsync(string externalId);
}