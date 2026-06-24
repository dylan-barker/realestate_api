using Dapper;
using RealEstateApi.Application.Interfaces;
using RealEstateApi.Domain.Models;
using RealEstateApi.Infrastructure.Data;

namespace RealEstateApi.Infrastructure.Repositories;

public class ListingValuationRepository : IListingValuationRepository
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
            "sp_ListingValuation_GetByListingId",
            new { ListingId = listingId },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<ListingValuation> UpsertAsync(int listingId, ListingValuation valuation)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<ListingValuation>(
            "sp_ListingValuation_Upsert",
            new
            {
                ListingId = listingId,
                OwnersNetPrice = valuation.OwnersNetPrice,
                AgentValuation = valuation.AgentValuation,
                CommissionPercent = valuation.CommissionPercent
            },
            commandType: System.Data.CommandType.StoredProcedure);
    }
}
