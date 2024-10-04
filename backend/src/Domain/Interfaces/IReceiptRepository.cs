using Domain.DTOs;
using Domain.Entities;
using Domain.Models;

namespace Domain.Interfaces;

public interface IReceiptRepository
{
    public Task<Receipt> StoreReceiptAsync(ReceiptEntity receipt);

    public Task<Receipt> EditReceiptAsync(EditReceiptRequest receipt);

    public Task<Receipt?> GetReceiptAsync(Guid receiptId);

    public Task<List<ReceiptListItem>> ListReceiptsAsync(ListReceiptsFilter filter);
}
