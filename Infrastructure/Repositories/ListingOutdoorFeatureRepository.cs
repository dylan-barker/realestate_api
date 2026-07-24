using Dapper;
using RealEstateApi.Domain.Models;
using RealEstateApi.Infrastructure.Data;

namespace RealEstateApi.Infrastructure.Repositories;

public class ListingOutdoorFeatureRepository
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
            "SELECT Id, ListingId, Description FROM ListingOutdoorFeature WHERE ListingId = @ListingId ORDER BY Id",
            new { ListingId = listingId });
    }

    public async Task<ListingOutdoorFeature> AddAsync(ListingOutdoorFeature feature)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<ListingOutdoorFeature>(
            "INSERT INTO ListingOutdoorFeature (ListingId, Description) " +
            "OUTPUT INSERTED.Id, INSERTED.ListingId, INSERTED.Description " +
            "VALUES (@ListingId, @Description)",
            new { feature.ListingId, feature.Description });
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(
            "DELETE FROM ListingOutdoorFeature WHERE Id = @Id", new { Id = id });
    }

    public async Task<IEnumerable<ListingOutdoorFeature>> ReplaceAllAsync(int listingId, IEnumerable<string> descriptions)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();
        using var tx = connection.BeginTransaction();
        await connection.ExecuteAsync(
            "DELETE FROM ListingOutdoorFeature WHERE ListingId = @ListingId",
            new { ListingId = listingId }, transaction: tx);
        foreach (var desc in descriptions)
        {
            await connection.ExecuteAsync(
                "INSERT INTO ListingOutdoorFeature (ListingId, Description) VALUES (@ListingId, @Description)",
                new { ListingId = listingId, Description = desc }, transaction: tx);
        }
        tx.Commit();
        return await GetByListingIdAsync(listingId);
    }
}
