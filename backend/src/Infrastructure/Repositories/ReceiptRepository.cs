using System.Data;
using Dapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;

namespace Infrastructure.Repositories;

public class ReceiptRepository(IDbConnection dbConnection)
    : BaseRepository(dbConnection),
        IReceiptRepository
{
    public async Task<Receipt?> GetReceiptAsync(Guid receiptId)
    {
        const string query =
            @"
            SELECT * FROM ""Receipt"" where ""Id"" = @Id
        ";

        return await _dbConnection.QuerySingleOrDefaultAsync<Receipt>(
            query,
            new { Id = receiptId }
        );
    }

    public Task<List<ReceiptListItem>> ListReceiptsAsync(ListReceiptsFilter filter)
    {
        throw new NotImplementedException();
    }

    public async Task<Receipt> StoreReceiptAsync(ReceiptEntity receipt)
    {
        const string query =
            @"
            INSERT INTO ""Receipt"" (
               ""Id"", ""Total"", ""Note"", ""CurrencyCode"", ""UserId"", ""MerchantId""
            ) 
            VALUES 
                (
                    gen_random_uuid(), 
                    @Total, 
                    @Note, 
                    @CurrencyCode, 
                    @UserId,
                    @MerchantId
                ) RETURNING *
        ";

        return await _dbConnection.QueryFirstAsync<Receipt>(
            query,
            new
            {
                receipt.Total,
                receipt.Note,
                receipt.CurrencyCode,
                receipt.UserId,
                receipt.MerchantId,
            }
        );
    }
}
