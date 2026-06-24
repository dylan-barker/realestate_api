using RealEstateApi.Application.Interfaces;
using RealEstateApi.Application.DTOs;
using RealEstateApi.Domain.Models;
using RealEstateApi.Mappings;

namespace RealEstateApi.Application.Services;

public class ListingParkingService : IListingParkingService
{
    private readonly IListingRepository _listingRepo;
    private readonly IListingParkingRepository _parkingRepo;

    public ListingParkingService(IListingRepository listingRepo, IListingParkingRepository parkingRepo)
    {
        _listingRepo = listingRepo;
        _parkingRepo = parkingRepo;
    }

    public async Task<IEnumerable<ParkingDto>> GetParkingAsync(int listingId)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        var parking = await _parkingRepo.GetByListingIdAsync(listingId);
        return parking.Select(p => p.ToParkingDto(p.ParkingTypeDescription ?? ""));
    }

    public async Task<ParkingDto> AddParkingAsync(int listingId, AddParkingRequest request)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        var parking = new ListingParking
        {
            ListingId = listingId,
            ParkingTypeId = request.ParkingTypeId,
            Quantity = request.Quantity
        };

        var result = await _parkingRepo.CreateAsync(parking);
        return result.ToParkingDto(result.ParkingTypeDescription ?? "");
    }

    public async Task<ParkingDto?> UpdateParkingAsync(int listingId, int parkingId, UpdateParkingRequest request)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        var result = await _parkingRepo.UpdateAsync(parkingId, request.Quantity);
        if (result == null) return null;

        return result.ToParkingDto(result.ParkingTypeDescription ?? "");
    }

    public async Task DeleteParkingAsync(int listingId, int parkingId)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        await _parkingRepo.DeleteAsync(parkingId);
    }
}
