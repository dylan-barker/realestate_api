using Dapper;
using RealEstateApi.Domain.Models;
using RealEstateApi.Infrastructure.Data;

namespace RealEstateApi.Infrastructure.Repositories;

public class PropertyRunningCostsRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public PropertyRunningCostsRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<PropertyRunningCosts?> GetByListingIdAsync(int listingId)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<PropertyRunningCosts>(
            "SELECT * FROM PropertyRunningCosts WHERE ListingId = @ListingId", new { ListingId = listingId });
    }

    public async Task<PropertyRunningCosts> UpsertAsync(PropertyRunningCosts costs)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<PropertyRunningCosts>(
            "MERGE PropertyRunningCosts AS t " +
            "USING (SELECT @ListingId AS ListingId) AS s " +
            "ON t.ListingId = s.ListingId " +
            "WHEN MATCHED THEN UPDATE SET MonthlyLevy = @MonthlyLevy, MonthlyRates = @MonthlyRates, Electricity = @Electricity, Water = @Water " +
            "WHEN NOT MATCHED THEN INSERT (ListingId, MonthlyLevy, MonthlyRates, Electricity, Water) " +
            "VALUES (@ListingId, @MonthlyLevy, @MonthlyRates, @Electricity, @Water) " +
            "OUTPUT INSERTED.*;",
            costs);
    }
}
