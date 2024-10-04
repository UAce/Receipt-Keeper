using System.Data;
using Dapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;

namespace Infrastructure.Repositories.Dapper;

public class ReceiptRepository(IDbConnection dbConnection)
    : BaseRepository(dbConnection),
        IReceiptRepository
{
    public async Task<Receipt?> GetReceiptAsync(Guid receiptId)
    {
        const string query =
            @"
                SELECT * FROM ""Receipt"" r 
                INNER JOIN ""Merchant"" m ON r.""MerchantId"" = m.""MerchantId"" 
                WHERE ""Id"" = @Id
            ";

        var receiptDictionary = new Dictionary<Guid, Receipt>();

        var result = await _dbConnection.QueryAsync<ReceiptEntity, MerchantEntity, Receipt>(
            query,
            (receipt, merchant) =>
            {
                return new Receipt
                {
                    Id = receipt.Id,
                    Total = receipt.Total,
                    Note = receipt.Note,
                    PrintedAt = receipt.PrintedAt,
                    UserId = receipt.UserId,
                    CurrencyCode = receipt.CurrencyCode,
                    Merchant = new Merchant { Id = merchant.Id, Name = merchant.Name },
                };
            },
            new { Id = receiptId },
            splitOn: "MerchantId"
        );

        return result.FirstOrDefault();
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
