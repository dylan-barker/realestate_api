using Dapper;
using RealEstateApi.Domain.Models;
using RealEstateApi.Infrastructure.Data;

namespace RealEstateApi.Infrastructure.Repositories;

public class ListingValuationRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public ListingValuationRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<ListingValuation?> GetByListingIdAsync(int listingId)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<ListingValuation>(
            "SELECT lv.Id, lv.OwnersNetPrice, lv.AgentValuation, lv.CommissionPercent " +
            "FROM ListingValuation lv INNER JOIN Listings l ON l.ListingValuationId = lv.Id WHERE l.Id = @ListingId",
            new { ListingId = listingId });
    }

    public async Task<ListingValuation> UpsertAsync(int listingId, ListingValuation valuation)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<ListingValuation>(
            "DECLARE @ValuationId INT; " +
            "SELECT @ValuationId = ListingValuationId FROM Listings WHERE Id = @ListingId; " +
            "IF @ValuationId IS NOT NULL " +
            "UPDATE ListingValuation SET OwnersNetPrice = @OwnersNetPrice, AgentValuation = @AgentValuation, CommissionPercent = @CommissionPercent WHERE Id = @ValuationId; " +
            "ELSE BEGIN " +
            "INSERT INTO ListingValuation (OwnersNetPrice, AgentValuation, CommissionPercent) VALUES (@OwnersNetPrice, @AgentValuation, @CommissionPercent); " +
            "SET @ValuationId = SCOPE_IDENTITY(); " +
            "UPDATE Listings SET ListingValuationId = @ValuationId, UpdatedAt = GETUTCDATE() WHERE Id = @ListingId; " +
            "END " +
            "SELECT Id, OwnersNetPrice, AgentValuation, CommissionPercent FROM ListingValuation WHERE Id = @ValuationId;",
            new { ListingId = listingId, valuation.OwnersNetPrice, valuation.AgentValuation, valuation.CommissionPercent });
    }
}
