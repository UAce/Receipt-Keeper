using Domain.Entities;
using Domain.Models;

namespace Domain.Interfaces;

public interface IMerchantRepository
{
    public Task<Merchant> AddMerchantAsync(MerchantEntity merchant);
}
