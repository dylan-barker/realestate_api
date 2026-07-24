using RealEstateApi.Application.DTOs;
using RealEstateApi.Domain.Models;
using RealEstateApi.Infrastructure.Repositories;

namespace RealEstateApi.Application.Services;

public class ListingService
{
    private readonly ListingRepository _listingRepo;
    private readonly ListingAddressRepository _addressRepo;
    private readonly ListingBuildingInfoRepository _buildingInfoRepo;
    private readonly ListingValuationRepository _valuationRepo;
    private readonly PropertyRunningCostsRepository _runningCostsRepo;
    private readonly ListingRoomRepository _roomRepo;
    private readonly ListingParkingRepository _parkingRepo;
    private readonly ContactRepository _contactRepo;
    private readonly ListingOutdoorFeatureRepository _outdoorFeatureRepo;

    public ListingService(
        ListingRepository listingRepo,
        ListingAddressRepository addressRepo,
        ListingBuildingInfoRepository buildingInfoRepo,
        ListingValuationRepository valuationRepo,
        PropertyRunningCostsRepository runningCostsRepo,
        ListingRoomRepository roomRepo,
        ListingParkingRepository parkingRepo,
        ContactRepository contactRepo,
        ListingOutdoorFeatureRepository outdoorFeatureRepo)
    {
        _listingRepo = listingRepo;
        _addressRepo = addressRepo;
        _buildingInfoRepo = buildingInfoRepo;
        _valuationRepo = valuationRepo;
        _runningCostsRepo = runningCostsRepo;
        _roomRepo = roomRepo;
        _parkingRepo = parkingRepo;
        _contactRepo = contactRepo;
        _outdoorFeatureRepo = outdoorFeatureRepo;
    }

    public async Task<ListingResponse> CreateAsync(CreateListingRequest request)
    {
        var listing = await _listingRepo.CreateAsync(request.PropertyTypeId, request.P24Ref);
        return await BuildFullResponseAsync(listing);
    }

    public async Task<ListingResponse?> GetByIdAsync(int id)
    {
        var listing = await _listingRepo.GetByIdAsync(id);
        if (listing == null) return null;
        return await BuildFullResponseAsync(listing);
    }

    public async Task<IEnumerable<ListingSummaryDto>> GetAllAsync(string? status, DateTime? dateFrom, DateTime? dateTo)
    {
        var listings = await _listingRepo.GetAllAsync(status, dateFrom, dateTo);
        return listings.Select(l => new ListingSummaryDto(
            l.Id, l.ReferenceNumber, l.P24Ref, l.PropertyTypeId,
            l.ListingValuationId, l.ListDate, l.Status, l.CreatedAt, l.UpdatedAt));
    }

    public async Task<ListingResponse?> UpdateAsync(int id, UpdateListingRequest request)
    {
        var listing = await _listingRepo.UpdateAsync(id, request.Status, request.P24Ref, request.PropertyTypeId);
        if (listing == null) return null;
        return await BuildFullResponseAsync(listing);
    }

    public async Task DeleteAsync(int id)
    {
        await _listingRepo.DeleteAsync(id);
    }

    public async Task<ListingResponse?> SubmitAsync(int id)
    {
        var listing = await _listingRepo.SubmitAsync(id);
        if (listing == null) return null;
        return await BuildFullResponseAsync(listing);
    }

    public async Task<ListingAddressDto?> GetAddressAsync(int listingId)
    {
        var address = await _addressRepo.GetByListingIdAsync(listingId);
        return address is null ? null : MapAddress(address);
    }

    public async Task<ListingAddressDto> UpsertAddressAsync(int listingId, UpsertAddressRequest request)
    {
        var address = new ListingAddress
        {
            ListingId = listingId,
            ErfNumber = request.ErfNumber,
            EstateName = request.EstateName,
            StreetNumber = request.StreetNumber,
            UnitNumber = request.UnitNumber,
            Street = request.Street,
            Suburb = request.Suburb,
            City = request.City,
            Province = request.Province,
            Country = request.Country,
            PostalCode = request.PostalCode,
            Latitude = request.Latitude,
            Longitude = request.Longitude
        };
        var result = await _addressRepo.UpsertAsync(address);
        return MapAddress(result);
    }

    public async Task<BuildingInfoDto?> GetBuildingInfoAsync(int listingId)
    {
        var info = await _buildingInfoRepo.GetByListingIdAsync(listingId);
        return info is null ? null : MapBuildingInfo(info);
    }

    public async Task<BuildingInfoDto> UpsertBuildingInfoAsync(int listingId, UpsertBuildingInfoRequest request)
    {
        var info = new ListingBuildingInfo
        {
            ListingId = listingId,
            ErfSize = request.ErfSize,
            FloorArea = request.FloorArea,
            ConstructionYear = request.ConstructionYear,
            FacingId = request.FacingId,
            ZoningId = request.ZoningId
        };
        var result = await _buildingInfoRepo.UpsertAsync(info);
        return MapBuildingInfo(result);
    }

    public async Task<ValuationDto?> GetValuationAsync(int listingId)
    {
        var valuation = await _valuationRepo.GetByListingIdAsync(listingId);
        return valuation is null ? null : MapValuation(valuation);
    }

    public async Task<ValuationDto> UpsertValuationAsync(int listingId, UpsertValuationRequest request)
    {
        var valuation = new ListingValuation
        {
            OwnersNetPrice = request.OwnersNetPrice,
            AgentValuation = request.AgentValuation,
            CommissionPercent = request.CommissionPercent
        };
        var result = await _valuationRepo.UpsertAsync(listingId, valuation);
        return MapValuation(result);
    }

    public async Task<RunningCostsDto?> GetRunningCostsAsync(int listingId)
    {
        var costs = await _runningCostsRepo.GetByListingIdAsync(listingId);
        return costs is null ? null : MapRunningCosts(costs);
    }

    public async Task<RunningCostsDto> UpsertRunningCostsAsync(int listingId, UpsertRunningCostsRequest request)
    {
        var costs = new PropertyRunningCosts
        {
            ListingId = listingId,
            MonthlyLevy = request.MonthlyLevy,
            MonthlyRates = request.MonthlyRates,
            Electricity = request.Electricity,
            Water = request.Water
        };
        var result = await _runningCostsRepo.UpsertAsync(costs);
        return MapRunningCosts(result);
    }

    private async Task<ListingResponse> BuildFullResponseAsync(Listing listing)
    {
        var id = listing.Id;

        var addressTask = _addressRepo.GetByListingIdAsync(id);
        var buildingInfoTask = _buildingInfoRepo.GetByListingIdAsync(id);
        var valuationTask = _valuationRepo.GetByListingIdAsync(id);
        var runningCostsTask = _runningCostsRepo.GetByListingIdAsync(id);
        var roomsTask = BuildRoomDtosAsync(id);
        var parkingTask = _parkingRepo.GetByListingIdAsync(id);
        var contactsTask = _contactRepo.GetByListingIdAsync(id);
        var outdoorFeaturesTask = _outdoorFeatureRepo.GetByListingIdAsync(id);

        await Task.WhenAll(addressTask, buildingInfoTask, valuationTask, runningCostsTask, roomsTask, parkingTask, contactsTask, outdoorFeaturesTask);

        return new ListingResponse(
            listing.Id, listing.ReferenceNumber, listing.P24Ref, listing.PropertyTypeId,
            listing.ListingValuationId, listing.ListDate, listing.Status,
            listing.CreatedAt, listing.UpdatedAt,
            addressTask.Result is null ? null : MapAddress(addressTask.Result),
            buildingInfoTask.Result is null ? null : MapBuildingInfo(buildingInfoTask.Result),
            valuationTask.Result is null ? null : MapValuation(valuationTask.Result),
            runningCostsTask.Result is null ? null : MapRunningCosts(runningCostsTask.Result),
            roomsTask.Result,
            parkingTask.Result.Select(p => new ParkingDto(p.Id, p.ListingId, p.ParkingTypeId, p.Quantity, p.ParkingTypeDescription ?? "")).ToList(),
            contactsTask.Result.Select(c => new ContactDto(c.Id, c.FullName, c.IdNumber, c.CompanyName, c.CompanyRegistrationNumber, c.MobilePhone, c.EmailAddress, c.Role, c.ListingId)).ToList(),
            outdoorFeaturesTask.Result.Select(f => new OutdoorFeatureDto(f.Id, f.ListingId, f.Description)).ToList()
        );
    }

    private async Task<List<RoomDto>> BuildRoomDtosAsync(int listingId)
    {
        var rooms = await _roomRepo.GetByListingIdAsync(listingId);
        var roomDtos = new List<RoomDto>();

        foreach (var room in rooms)
        {
            var conditionTask = _roomRepo.GetConditionByRoomIdAsync(room.Id);
            var featuresTask = _roomRepo.GetLinkedFeaturesAsync(room.Id);
            var customFeaturesTask = _roomRepo.GetCustomFeaturesAsync(room.Id);

            await Task.WhenAll(conditionTask, featuresTask, customFeaturesTask);

            roomDtos.Add(new RoomDto(
                room.Id, room.ListingId, room.Name, room.RoomTypeId,
                room.RoomTypeOther, room.PhotoUrl, room.CreatedAt, room.UpdatedAt,
                conditionTask.Result is null ? null : new RoomConditionDto(
                    conditionTask.Result.Id, conditionTask.Result.ListingRoomId,
                    conditionTask.Result.ConditionRating, conditionTask.Result.Notes,
                    conditionTask.Result.ConditionCategoryId),
                featuresTask.Result.Select(f => new FeatureDto(f.Id, f.Category, f.Description)).ToList(),
                customFeaturesTask.Result.Select(cf => new CustomFeatureDto(cf.Id, cf.ListingRoomId, cf.Description)).ToList()
            ));
        }

        return roomDtos;
    }

    private static ListingAddressDto MapAddress(ListingAddress a) => new(
        a.ListingAddressId, a.ListingId, a.ErfNumber, a.EstateName,
        a.StreetNumber, a.UnitNumber, a.Street, a.Suburb, a.City,
        a.Province, a.Country, a.PostalCode, a.Latitude, a.Longitude);

    private static BuildingInfoDto MapBuildingInfo(ListingBuildingInfo i) => new(
        i.Id, i.ListingId, i.ErfSize, i.FloorArea, i.ConstructionYear, i.FacingId, i.ZoningId);

    private static ValuationDto MapValuation(ListingValuation v) => new(
        v.Id, v.OwnersNetPrice, v.AgentValuation, v.CommissionPercent);

    private static RunningCostsDto MapRunningCosts(PropertyRunningCosts c) => new(
        c.Id, c.ListingId, c.MonthlyLevy, c.MonthlyRates, c.Electricity, c.Water);
}
