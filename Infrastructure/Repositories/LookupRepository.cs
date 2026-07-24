using Dapper;
using RealEstateApi.Domain.Models;
using RealEstateApi.Infrastructure.Data;

namespace RealEstateApi.Infrastructure.Repositories;

public class LookupRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public LookupRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<PropertyType>> GetPropertyTypesAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<PropertyType>(
            "SELECT Id, Name, SortOrder, IsActive FROM PropertyType WHERE IsActive = 1 ORDER BY SortOrder ASC");
    }

    public async Task<IEnumerable<RoomType>> GetRoomTypesAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<RoomType>(
            "SELECT Id, Description FROM RoomTypes ORDER BY Description");
    }

    public async Task<IEnumerable<Feature>> GetFeaturesAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Feature>(
            "SELECT Id, Category, Description FROM Feature ORDER BY Category, Description");
    }

    public async Task<IEnumerable<ConditionCategory>> GetConditionCategoriesAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<ConditionCategory>(
            "SELECT Id, Description FROM ConditionCategory ORDER BY Description");
    }

    public async Task<IEnumerable<ParkingType>> GetParkingTypesAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<ParkingType>(
            "SELECT Id, Description FROM ParkingType ORDER BY Description");
    }

    public async Task<IEnumerable<Facing>> GetFacingAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Facing>(
            "SELECT Id, Description FROM Facing ORDER BY Description");
    }

    public async Task<IEnumerable<Zoning>> GetZoningAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Zoning>(
            "SELECT Id, Description FROM Zoning ORDER BY Description");
    }
}
