namespace Domain.Models;

public class Receipt : Base
{
    // This maps to Numeric in PostgreSQL
    public required decimal Total { get; set; }

    public string Note { get; set; } = string.Empty;

    // Relationship

    public required string CurrencyCode { get; set; }

    public required Guid UserId { get; set; }

    public required Guid MerchantId { get; set; }
}