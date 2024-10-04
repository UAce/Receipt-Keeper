namespace Domain.Models;

public class ReceiptListItem
{
    public required Guid Id { get; set; }

    public required decimal Total { get; set; }

    public required DateTimeOffset PrintedAt { get; set; }

    // Relationship

    public required Guid UserId { get; set; }

    public required string CurrencyCode { get; set; }

    public required Merchant Merchant { get; set; }
}
