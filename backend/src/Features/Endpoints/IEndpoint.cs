namespace Features.Endpoints;

public interface IEndpoint
{
    void Map(IEndpointRouteBuilder app);
}