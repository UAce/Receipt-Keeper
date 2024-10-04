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
                SELECT
                    *
                FROM
                    ""Receipt"" r 
                INNER JOIN
                    ""Merchant"" m ON r.""MerchantId"" = m.""Id"" 
                WHERE
                    r.""Id"" = @Id
            ";

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
                    MerchantId = merchant.Id,
                    Merchant = new Merchant { Id = merchant.Id, Name = merchant.Name },
                };
            },
            new { Id = receiptId },
            splitOn: "MerchantId"
        );

        return result.FirstOrDefault();
    }

    public async Task<List<ReceiptListItem>> ListReceiptsAsync(ListReceiptsFilter filter)
    {
        const string query =
            @"
                SELECT
                    *
                FROM
                    ""Receipt"" r
                INNER JOIN
                    ""Merchant"" m ON r.""MerchantId"" = m.""Id""
                WHERE
                    r.""UserId"" = @UserId AND
                    (@FromDate IS NULL OR r.""PrintedAt"" >= @FromDate) AND
                    (@ToDate IS NULL OR r.""PrintedAt"" < @ToDate) -- Exclusive To
                ORDER BY 
                    r.""PrintedAt""
                OFFSET @Offset ROWS
                FETCH NEXT @PageSize ROWS ONLY;
            ";

        var result = await _dbConnection.QueryAsync<ReceiptEntity, MerchantEntity, ReceiptListItem>(
            query,
            (receipt, merchant) =>
            {
                return new ReceiptListItem
                {
                    Id = receipt.Id,
                    Total = receipt.Total,
                    PrintedAt = receipt.PrintedAt,
                    UserId = receipt.UserId,
                    CurrencyCode = receipt.CurrencyCode,
                    Merchant = new Merchant { Id = merchant.Id, Name = merchant.Name },
                };
            },
            new
            {
                filter.UserId,
                FromDate = filter.DateTimeRange?.From,
                ToDate = filter.DateTimeRange?.To,
                Offset = (filter.Pagination.PageNumber - 1) * filter.Pagination.PageSize,
                filter.Pagination.PageSize,
            },
            splitOn: "MerchantId"
        );

        return result.ToList();
    }

    public async Task<Receipt> StoreReceiptAsync(ReceiptEntity receipt)
    {
        const string query =
            @"
            INSERT INTO ""Receipt"" (
               ""Id"", ""Total"", ""Note"", ""PrintedAt"", ""CurrencyCode"", ""UserId"", ""MerchantId""
            ) 
            VALUES 
                (
                    gen_random_uuid(), 
                    @Total, 
                    @Note, 
                    @PrintedAt,
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
                receipt.PrintedAt,
                receipt.CurrencyCode,
                receipt.UserId,
                receipt.MerchantId,
            }
        );
    }

    public async Task<Receipt> EditReceiptAsync(EditReceiptRequest receipt)
    {
        const string query =
            @"
            UPDATE 
                ""Receipt"" 
            SET 
                ""Total"" = @Total,
                ""Note"" = @Note,
                ""CurrencyCode"" = @CurrencyCode,
                ""PrintedAt"" = @PrintedAt,
                ""MerchantId"" = @MerchantId
            WHERE
                ""Id"" = @Id
            RETURNING *;
        ";

        return await _dbConnection.QueryFirstAsync<Receipt>(
            query,
            new
            {
                receipt.Id,
                receipt.Total,
                receipt.Note,
                receipt.PrintedAt,
                receipt.CurrencyCode,
                receipt.MerchantId,
            }
        );
    }
}
