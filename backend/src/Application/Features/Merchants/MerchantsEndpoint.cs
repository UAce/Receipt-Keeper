using Application.Endpoints;

namespace Application.Features.Merchants;

public abstract class MerchantsEndpoint : ApiV1Endpoint
{
    public IEndpointRouteBuilder MapResource(IEndpointRouteBuilder app)
    {
        return app.MapGroup(EndpointConstants.Merchants).WithTags("Merchants");
    }
}
