using Core.Entities.User;
using Core.Models.User;

namespace Core.Interfaces;

public interface IUserRepository
{
    public Task RegisterAsync(UserRegistrationModel userRegistrationModel);

}