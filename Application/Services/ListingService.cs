using RealEstateApi.Application.Interfaces;
using RealEstateApi.Application.DTOs;
using RealEstateApi.Domain.Models;
using RealEstateApi.Mappings;

namespace RealEstateApi.Application.Services;

public class ListingService : IListingService
{
    private readonly IListingRepository _listingRepo;
    private readonly IListingAddressRepository _addressRepo;
    private readonly IListingBuildingInfoRepository _buildingInfoRepo;
    private readonly IListingValuationRepository _valuationRepo;
    private readonly IPropertyRunningCostsRepository _runningCostsRepo;
    private readonly IListingRoomRepository _roomRepo;
    private readonly IListingParkingRepository _parkingRepo;
    private readonly IContactRepository _contactRepo;

    public ListingService(
        IListingRepository listingRepo,
        IListingAddressRepository addressRepo,
        IListingBuildingInfoRepository buildingInfoRepo,
        IListingValuationRepository valuationRepo,
        IPropertyRunningCostsRepository runningCostsRepo,
        IListingRoomRepository roomRepo,
        IListingParkingRepository parkingRepo,
        IContactRepository contactRepo)
    {
        _listingRepo = listingRepo;
        _addressRepo = addressRepo;
        _buildingInfoRepo = buildingInfoRepo;
        _valuationRepo = valuationRepo;
        _runningCostsRepo = runningCostsRepo;
        _roomRepo = roomRepo;
        _parkingRepo = parkingRepo;
        _contactRepo = contactRepo;
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
        return listings.Select(l => l.ToSummaryDto());
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
        return address?.ToDto();
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
        return result.ToDto();
    }

    public async Task<BuildingInfoDto?> GetBuildingInfoAsync(int listingId)
    {
        var info = await _buildingInfoRepo.GetByListingIdAsync(listingId);
        return info?.ToDto();
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
        return result.ToDto();
    }

    public async Task<ValuationDto?> GetValuationAsync(int listingId)
    {
        var valuation = await _valuationRepo.GetByListingIdAsync(listingId);
        return valuation?.ToDto();
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
        return result.ToDto();
    }

    public async Task<RunningCostsDto?> GetRunningCostsAsync(int listingId)
    {
        var costs = await _runningCostsRepo.GetByListingIdAsync(listingId);
        return costs?.ToDto();
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
        return result.ToDto();
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

        await Task.WhenAll(addressTask, buildingInfoTask, valuationTask, runningCostsTask, roomsTask, parkingTask, contactsTask);

        return listing.ToFullDto(
            addressTask.Result?.ToDto(),
            buildingInfoTask.Result?.ToDto(),
            valuationTask.Result?.ToDto(),
            runningCostsTask.Result?.ToDto(),
            roomsTask.Result,
            parkingTask.Result.Select(p => p.ToParkingDto(p.ParkingTypeDescription ?? "")).ToList(),
            contactsTask.Result.Select(c => c.ToDto()).ToList()
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

            roomDtos.Add(room.ToDto(
                conditionTask.Result?.ToDto(),
                featuresTask.Result.Select(f => f.ToDto()).ToList(),
                customFeaturesTask.Result.Select(cf => cf.ToDto()).ToList()
            ));
        }

        return roomDtos;
    }
}
