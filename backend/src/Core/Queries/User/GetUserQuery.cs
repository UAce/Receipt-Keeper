using Core.Models.User;
using MediatR;

namespace Core.Queries.User;

public record GetUserQuery(string ExternalId): IRequest<UserModel?>;