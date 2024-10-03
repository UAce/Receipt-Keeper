namespace Application.Endpoints;

public abstract class ApiV1Endpoint
{
    public IEndpointRouteBuilder MapApiVersion(IEndpointRouteBuilder app)
    {
        return app.MapGroup(EndpointConstants.ApiV1);
    }
}
