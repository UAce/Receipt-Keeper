using Application.Endpoints;

namespace Application.Features.Currencies;

public abstract class CurrenciesEndpoint : ApiV1Endpoint
{
    public IEndpointRouteBuilder MapResource(IEndpointRouteBuilder app)
    {
        return app.MapGroup(EndpointConstants.Currencies).WithTags("Currencies");
    }
}
