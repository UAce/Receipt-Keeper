using Domain.Types;

namespace Domain.DTOs
{
    public record ListReceiptsFilter(
        Guid UserId,
        Pagination Pagination,
        DateTimeRange DateTimeRange
    ) { };
}
