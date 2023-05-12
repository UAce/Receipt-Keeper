using Core.Models.User;
using MediatR;

namespace Core.Commands.User;

public record RegisterUserCommand(string FirstName, string LastName, string Email, string ExternalId): IRequest<UserModel>;