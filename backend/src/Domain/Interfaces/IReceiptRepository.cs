using Domain.Models;

namespace Domain.Interfaces;

public interface IReceiptRepository
{
    public Task<Receipt> StoreReceiptAsync(Receipt receipt);

    public Task<Receipt?> GetReceiptAsync(Guid receiptId);

    public Task<List<Receipt>> ListReceiptsAsync(ListReceiptsRequest request);
}
