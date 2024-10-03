namespace Application.Endpoints
{
    public interface IEndpoint
    {
        public IEndpointRouteBuilder MapApiVersion(IEndpointRouteBuilder app);
        public IEndpointRouteBuilder MapResource(IEndpointRouteBuilder app);
        public void MapRoute(IEndpointRouteBuilder app);
    }
}
