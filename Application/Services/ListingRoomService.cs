using RealEstateApi.Application.Interfaces;
using RealEstateApi.Application.DTOs;
using RealEstateApi.Domain.Models;
using RealEstateApi.Mappings;

namespace RealEstateApi.Application.Services;

public class ListingRoomService : IListingRoomService
{
    private readonly IListingRepository _listingRepo;
    private readonly IListingRoomRepository _roomRepo;

    public ListingRoomService(IListingRepository listingRepo, IListingRoomRepository roomRepo)
    {
        _listingRepo = listingRepo;
        _roomRepo = roomRepo;
    }

    public async Task<IEnumerable<RoomDto>> GetRoomsAsync(int listingId)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        var rooms = await _roomRepo.GetByListingIdAsync(listingId);
        var roomDtos = new List<RoomDto>();

        foreach (var room in rooms)
        {
            var conditionTask = _roomRepo.GetConditionByRoomIdAsync(room.Id);
            var featuresTask = _roomRepo.GetLinkedFeaturesAsync(room.Id);
            var customFeaturesTask = _roomRepo.GetCustomFeaturesAsync(room.Id);

            await Task.WhenAll(conditionTask, featuresTask, customFeaturesTask);

            roomDtos.Add(room.ToDto(
                conditionTask.Result?.ToDto(),
                featuresTask.Result.Select(f => f.ToDto()).ToList(),
                customFeaturesTask.Result.Select(cf => cf.ToDto()).ToList()
            ));
        }

        return roomDtos;
    }

    public async Task<RoomDto> CreateRoomAsync(int listingId, CreateRoomRequest request)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        var room = new ListingRoom
        {
            ListingId = listingId,
            Name = request.Name,
            RoomTypeId = request.RoomTypeId,
            RoomTypeOther = request.RoomTypeOther,
            PhotoUrl = request.PhotoUrl
        };

        var created = await _roomRepo.CreateAsync(room);
        return created.ToDto(null, new List<FeatureDto>(), new List<CustomFeatureDto>());
    }

    public async Task<RoomDto?> UpdateRoomAsync(int listingId, int roomId, UpdateRoomRequest request)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        var room = new ListingRoom
        {
            Id = roomId,
            Name = request.Name ?? string.Empty,
            RoomTypeId = request.RoomTypeId ?? 0,
            RoomTypeOther = request.RoomTypeOther,
            PhotoUrl = request.PhotoUrl
        };

        var updated = await _roomRepo.UpdateAsync(room);
        if (updated == null) return null;

        var condition = await _roomRepo.GetConditionByRoomIdAsync(updated.Id);
        var features = await _roomRepo.GetLinkedFeaturesAsync(updated.Id);
        var customFeatures = await _roomRepo.GetCustomFeaturesAsync(updated.Id);

        return updated.ToDto(
            condition?.ToDto(),
            features.Select(f => f.ToDto()).ToList(),
            customFeatures.Select(cf => cf.ToDto()).ToList()
        );
    }

    public async Task DeleteRoomAsync(int listingId, int roomId)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        await _roomRepo.DeleteAsync(roomId);
    }

    public async Task<RoomConditionDto> UpsertConditionAsync(int listingId, int roomId, UpsertRoomConditionRequest request)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        var condition = new Condition
        {
            ListingRoomId = roomId,
            ConditionRating = request.ConditionRating,
            Notes = request.Notes,
            ConditionCategoryId = request.ConditionCategoryId
        };

        var result = await _roomRepo.UpsertConditionAsync(condition);
        return result.ToDto();
    }

    public async Task<IEnumerable<FeatureDto>> LinkFeatureAsync(int listingId, int roomId, int featureId)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        var features = await _roomRepo.LinkFeatureAsync(roomId, featureId);
        return features.Select(f => f.ToDto());
    }

    public async Task<IEnumerable<FeatureDto>> UnlinkFeatureAsync(int listingId, int roomId, int featureId)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        var features = await _roomRepo.UnlinkFeatureAsync(roomId, featureId);
        return features.Select(f => f.ToDto());
    }

    public async Task<CustomFeatureDto> AddCustomFeatureAsync(int listingId, int roomId, AddCustomFeatureRequest request)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        var feature = new ListingRoomCustomFeature
        {
            ListingRoomId = roomId,
            Description = request.Description
        };

        var result = await _roomRepo.AddCustomFeatureAsync(feature);
        return result.ToDto();
    }

    public async Task DeleteCustomFeatureAsync(int listingId, int roomId, int customFeatureId)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        await _roomRepo.DeleteCustomFeatureAsync(customFeatureId);
    }
}
