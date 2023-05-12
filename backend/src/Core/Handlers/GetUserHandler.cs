using Core.Interfaces;
using Core.Models.User;
using Core.Queries.User;
using MediatR;

namespace Core.Handlers;

public class GetUserHandler : IRequestHandler<GetUserQuery, UserModel?>
{
    private readonly IUserRepository _userRepository;

    public GetUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public Task<UserModel?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Handling [GetUserQuery] request for externalId: '{request.ExternalId}'");
        return _userRepository.GetUserAsync(request.ExternalId);
    }
}