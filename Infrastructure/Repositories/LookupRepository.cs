using Dapper;
using RealEstateApi.Application.Interfaces;
using RealEstateApi.Domain.Models;
using RealEstateApi.Infrastructure.Data;
using System.Data;

namespace RealEstateApi.Infrastructure.Repositories;

public class LookupRepository : ILookupRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public LookupRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<PropertyType>> GetPropertyTypesAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<PropertyType>("sp_PropertyTypes_GetAll",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<RoomType>> GetRoomTypesAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<RoomType>("sp_RoomTypes_GetAll",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Feature>> GetFeaturesAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Feature>("sp_Features_GetAll",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<ConditionCategory>> GetConditionCategoriesAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<ConditionCategory>("sp_ConditionCategories_GetAll",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<ParkingType>> GetParkingTypesAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<ParkingType>("sp_ParkingTypes_GetAll",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Facing>> GetFacingAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Facing>("sp_Facing_GetAll",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Zoning>> GetZoningAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Zoning>("sp_Zoning_GetAll",
            commandType: CommandType.StoredProcedure);
    }
}
