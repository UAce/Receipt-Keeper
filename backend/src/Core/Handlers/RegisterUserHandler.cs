using Core.Commands.User;
using Core.Interfaces;
using Core.Models.User;
using MediatR;

namespace Core.Handlers;

public class RegisterUserHandler: IRequestHandler<RegisterUserCommand, UserModel>
{
    private readonly IUserRepository _userRepository;

    public RegisterUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    public Task<UserModel> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Handling [RegisterUserCommand] request for externalId: '{request.ExternalId}'");
        UserRegistrationModel userRegistrationModel = new(request.FirstName, request.LastName, request.Email, request.ExternalId);
        return _userRepository.RegisterAsync(userRegistrationModel);
    }
}