namespace Application.Endpoints;

public interface IEndpoint
{
    void Map(IEndpointRouteBuilder app);
}