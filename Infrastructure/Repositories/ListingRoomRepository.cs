using Dapper;
using RealEstateApi.Application.Interfaces;
using RealEstateApi.Domain.Models;
using RealEstateApi.Infrastructure.Data;

namespace RealEstateApi.Infrastructure.Repositories;

public class ListingRoomRepository : IListingRoomRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public ListingRoomRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<ListingRoom>> GetByListingIdAsync(int listingId)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<ListingRoom>(
            "sp_ListingRooms_GetByListingId",
            new { ListingId = listingId },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<ListingRoom?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<ListingRoom>(
            "SELECT Id, ListingId, Name, RoomTypeId, RoomTypeOther, PhotoUrl, CreatedAt, UpdatedAt FROM ListingRoom WHERE Id = @Id",
            new { Id = id });
    }

    public async Task UpdatePhotoUrlAsync(int roomId, string? photoUrl)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(
            "UPDATE ListingRoom SET PhotoUrl = @PhotoUrl, UpdatedAt = GETUTCDATE() WHERE Id = @Id",
            new { Id = roomId, PhotoUrl = photoUrl });
    }

    public async Task<ListingRoom> CreateAsync(ListingRoom room)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<ListingRoom>(
            "sp_ListingRooms_Create",
            new
            {
                ListingId = room.ListingId,
                Name = room.Name,
                RoomTypeId = room.RoomTypeId,
                RoomTypeOther = room.RoomTypeOther,
                PhotoUrl = room.PhotoUrl
            },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<ListingRoom?> UpdateAsync(ListingRoom room)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<ListingRoom>(
            "sp_ListingRooms_Update",
            new
            {
                Id = room.Id,
                Name = room.Name,
                RoomTypeId = room.RoomTypeId,
                RoomTypeOther = room.RoomTypeOther,
                PhotoUrl = room.PhotoUrl
            },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(
            "sp_ListingRooms_Delete",
            new { Id = id },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<Condition?> GetConditionByRoomIdAsync(int listingRoomId)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Condition>(
            "sp_Condition_GetByListingRoomId",
            new { ListingRoomId = listingRoomId },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<Condition> UpsertConditionAsync(Condition condition)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Condition>(
            "sp_Condition_Upsert",
            new
            {
                ListingRoomId = condition.ListingRoomId,
                ConditionRating = condition.ConditionRating,
                Notes = condition.Notes,
                ConditionCategoryId = condition.ConditionCategoryId
            },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Feature>> GetLinkedFeaturesAsync(int listingRoomId)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Feature>(
            "sp_ListingRoomFeatures_GetByListingRoomId",
            new { ListingRoomId = listingRoomId },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Feature>> LinkFeatureAsync(int listingRoomId, int featureId)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Feature>(
            "sp_ListingRoomFeatures_Link",
            new { ListingRoomId = listingRoomId, FeatureId = featureId },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Feature>> UnlinkFeatureAsync(int listingRoomId, int featureId)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Feature>(
            "sp_ListingRoomFeatures_Unlink",
            new { ListingRoomId = listingRoomId, FeatureId = featureId },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<ListingRoomCustomFeature>> GetCustomFeaturesAsync(int listingRoomId)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<ListingRoomCustomFeature>(
            "sp_ListingRoomCustomFeatures_GetByListingRoomId",
            new { ListingRoomId = listingRoomId },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<ListingRoomCustomFeature> AddCustomFeatureAsync(ListingRoomCustomFeature feature)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<ListingRoomCustomFeature>(
            "sp_ListingRoomCustomFeatures_Add",
            new
            {
                ListingRoomId = feature.ListingRoomId,
                Description = feature.Description
            },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task DeleteCustomFeatureAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(
            "sp_ListingRoomCustomFeatures_Remove",
            new { Id = id },
            commandType: System.Data.CommandType.StoredProcedure);
    }
}
