using Dapper;
using RealEstateApi.Domain.Models;
using RealEstateApi.Infrastructure.Data;

namespace RealEstateApi.Infrastructure.Repositories;

public class ListingParkingRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public ListingParkingRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<ListingParking>> GetByListingIdAsync(int listingId)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<ListingParking>(
            "SELECT lp.Id, lp.ListingId, lp.ParkingTypeId, lp.Quantity, pt.Description AS ParkingTypeDescription " +
            "FROM ListingParking lp INNER JOIN ParkingType pt ON pt.Id = lp.ParkingTypeId " +
            "WHERE lp.ListingId = @ListingId ORDER BY pt.Description",
            new { ListingId = listingId });
    }

    public async Task<ListingParking> CreateAsync(ListingParking parking)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<ListingParking>(
            "DECLARE @Id INT; " +
            "INSERT INTO ListingParking (ListingId, ParkingTypeId, Quantity) VALUES (@ListingId, @ParkingTypeId, @Quantity); " +
            "SET @Id = SCOPE_IDENTITY(); " +
            "SELECT lp.Id, lp.ListingId, lp.ParkingTypeId, lp.Quantity, pt.Description AS ParkingTypeDescription " +
            "FROM ListingParking lp INNER JOIN ParkingType pt ON pt.Id = lp.ParkingTypeId WHERE lp.Id = @Id;",
            new { parking.ListingId, parking.ParkingTypeId, parking.Quantity });
    }

    public async Task<ListingParking?> UpdateAsync(int id, int quantity)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<ListingParking>(
            "UPDATE ListingParking SET Quantity = @Quantity WHERE Id = @Id; " +
            "SELECT lp.Id, lp.ListingId, lp.ParkingTypeId, lp.Quantity, pt.Description AS ParkingTypeDescription " +
            "FROM ListingParking lp INNER JOIN ParkingType pt ON pt.Id = lp.ParkingTypeId WHERE lp.Id = @Id;",
            new { Id = id, Quantity = quantity });
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(
            "DELETE FROM ListingParking WHERE Id = @Id", new { Id = id });
    }
}
