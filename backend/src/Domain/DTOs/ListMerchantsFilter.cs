namespace Domain.DTOs
{
    public record ListMerchantsFilter(Guid UserId, string? Search) { };
}
