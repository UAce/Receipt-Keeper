using Application.Endpoints;

namespace Application.Features.Users;

public class UsersEndpoint : ApiV1Endpoint
{
    public IEndpointRouteBuilder MapResource(IEndpointRouteBuilder app)
    {
        return app.MapGroup(EndpointConstants.Users).WithTags("Users");
    }
}
