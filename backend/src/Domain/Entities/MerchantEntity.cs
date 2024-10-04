namespace Domain.Entities;

public class MerchantEntity : BaseEntity
{
    public required string Name { get; set; }

    public required Guid UserId { get; set; }
}
