using Dapper;
using RealEstateApi.Application.Interfaces;
using RealEstateApi.Domain.Models;
using RealEstateApi.Infrastructure.Data;
using System.Data;

namespace RealEstateApi.Infrastructure.Repositories;

public class PropertyRunningCostsRepository : IPropertyRunningCostsRepository
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
            "sp_PropertyRunningCosts_GetByListingId",
            new { ListingId = listingId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<PropertyRunningCosts> UpsertAsync(PropertyRunningCosts costs)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<PropertyRunningCosts>(
            "sp_PropertyRunningCosts_Upsert",
            new
            {
                ListingId = costs.ListingId,
                MonthlyLevy = costs.MonthlyLevy,
                MonthlyRates = costs.MonthlyRates,
                Electricity = costs.Electricity,
                Water = costs.Water
            },
            commandType: CommandType.StoredProcedure);
    }
}
