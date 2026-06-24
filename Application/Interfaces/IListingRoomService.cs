using RealEstateApi.Application.DTOs;

namespace RealEstateApi.Application.Interfaces;

public interface IListingRoomService
{
    Task<IEnumerable<RoomDto>> GetRoomsAsync(int listingId);
    Task<RoomDto> CreateRoomAsync(int listingId, CreateRoomRequest request);
    Task<RoomDto?> UpdateRoomAsync(int listingId, int roomId, UpdateRoomRequest request);
    Task DeleteRoomAsync(int listingId, int roomId);
    Task<RoomConditionDto> UpsertConditionAsync(int listingId, int roomId, UpsertRoomConditionRequest request);
    Task<IEnumerable<FeatureDto>> LinkFeatureAsync(int listingId, int roomId, int featureId);
    Task<IEnumerable<FeatureDto>> UnlinkFeatureAsync(int listingId, int roomId, int featureId);
    Task<CustomFeatureDto> AddCustomFeatureAsync(int listingId, int roomId, AddCustomFeatureRequest request);
    Task DeleteCustomFeatureAsync(int listingId, int roomId, int customFeatureId);
}
