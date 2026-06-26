using Dapper;
using RealEstateApi.Application.Interfaces;
using RealEstateApi.Domain.Models;
using RealEstateApi.Infrastructure.Data;
using System.Data;

namespace RealEstateApi.Infrastructure.Repositories;

public class ListingAddressRepository : IListingAddressRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public ListingAddressRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<ListingAddress?> GetByListingIdAsync(int listingId)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<ListingAddress>(
            "sp_ListingAddress_GetByListingId",
            new { ListingId = listingId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<ListingAddress> UpsertAsync(ListingAddress address)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<ListingAddress>(
            "sp_ListingAddress_Upsert",
            new
            {
                ListingId = address.ListingId,
                ErfNumber = address.ErfNumber,
                EstateName = address.EstateName,
                StreetNumber = address.StreetNumber,
                UnitNumber = address.UnitNumber,
                Street = address.Street,
                Suburb = address.Suburb,
                City = address.City,
                Province = address.Province,
                Country = address.Country,
                PostalCode = address.PostalCode,
                Latitude = address.Latitude,
                Longitude = address.Longitude
            },
            commandType: CommandType.StoredProcedure);
    }
}
