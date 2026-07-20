using Dapper;
using RealEstateApi.Application.Interfaces;
using RealEstateApi.Domain.Models;
using RealEstateApi.Infrastructure.Data;
using System.Data;
using System.Text.Json;

namespace RealEstateApi.Infrastructure.Repositories;

public class ListingOutdoorFeatureRepository : IListingOutdoorFeatureRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public ListingOutdoorFeatureRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<ListingOutdoorFeature>> GetByListingIdAsync(int listingId)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<ListingOutdoorFeature>(
            "sp_ListingOutdoorFeature_GetByListingId",
            new { ListingId = listingId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<ListingOutdoorFeature> AddAsync(ListingOutdoorFeature feature)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<ListingOutdoorFeature>(
            "sp_ListingOutdoorFeature_Add",
            new
            {
                ListingId = feature.ListingId,
                Description = feature.Description
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(
            "sp_ListingOutdoorFeature_Delete",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<ListingOutdoorFeature>> ReplaceAllAsync(int listingId, IEnumerable<string> descriptions)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<ListingOutdoorFeature>(
            "sp_ListingOutdoorFeature_ReplaceAll",
            new
            {
                ListingId = listingId,
                Descriptions = JsonSerializer.Serialize(descriptions)
            },
            commandType: CommandType.StoredProcedure);
    }
}
