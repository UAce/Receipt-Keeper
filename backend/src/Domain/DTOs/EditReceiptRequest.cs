namespace Domain.DTOs
{
    public record EditReceiptRequest(
        Guid Id,
        decimal? Total,
        string? Note,
        DateTimeOffset? PrintedAt,
        string? CurrencyCode,
        Guid? MerchantId
    ) { };
}
