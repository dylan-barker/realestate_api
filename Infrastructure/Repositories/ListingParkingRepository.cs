using Dapper;
using RealEstateApi.Application.Interfaces;
using RealEstateApi.Domain.Models;
using RealEstateApi.Infrastructure.Data;

namespace RealEstateApi.Infrastructure.Repositories;

public class ListingParkingRepository : IListingParkingRepository
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
            "sp_ListingParking_GetByListingId",
            new { ListingId = listingId },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<ListingParking> CreateAsync(ListingParking parking)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<ListingParking>(
            "sp_ListingParking_Create",
            new
            {
                ListingId = parking.ListingId,
                ParkingTypeId = parking.ParkingTypeId,
                Quantity = parking.Quantity
            },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<ListingParking?> UpdateAsync(int id, int quantity)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<ListingParking>(
            "sp_ListingParking_Update",
            new { Id = id, Quantity = quantity },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(
            "sp_ListingParking_Delete",
            new { Id = id },
            commandType: System.Data.CommandType.StoredProcedure);
    }
}
