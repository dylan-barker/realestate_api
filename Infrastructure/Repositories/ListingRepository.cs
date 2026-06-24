using Dapper;
using RealEstateApi.Application.Interfaces;
using RealEstateApi.Domain.Models;
using RealEstateApi.Infrastructure.Data;

namespace RealEstateApi.Infrastructure.Repositories;

public class ListingRepository : IListingRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public ListingRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Listing?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Listing>(
            "sp_Listings_GetById",
            new { Id = id },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Listing>> GetAllAsync(string? status, DateTime? dateFrom, DateTime? dateTo)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Listing>(
            "sp_Listings_GetAll",
            new { Status = status, DateFrom = dateFrom, DateTo = dateTo },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<Listing> CreateAsync(int propertyTypeId, string? p24Ref)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Listing>(
            "sp_Listings_Create",
            new { PropertyTypeId = propertyTypeId, P24Ref = p24Ref },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<Listing?> UpdateAsync(int id, string? status, string? p24Ref, int? propertyTypeId)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Listing>(
            "sp_Listings_Update",
            new { Id = id, Status = status, P24Ref = p24Ref, PropertyTypeId = propertyTypeId },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(
            "sp_Listings_Delete",
            new { Id = id },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<Listing?> SubmitAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Listing>(
            "sp_Listings_Submit",
            new { Id = id },
            commandType: System.Data.CommandType.StoredProcedure);
    }
}
