using Application.Endpoints;

namespace Application.Features.Receipts;

public abstract class ReceiptsEndpoint : ApiV1Endpoint
{
    public IEndpointRouteBuilder MapResource(IEndpointRouteBuilder app)
    {
        return app.MapGroup(EndpointConstants.Receipts).WithTags("Receipts");
    }
}
