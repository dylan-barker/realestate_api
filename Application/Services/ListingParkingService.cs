using AutoMapper;
using RealEstateApi.Application.Interfaces;
using RealEstateApi.Application.DTOs;
using RealEstateApi.Domain.Models;

namespace RealEstateApi.Application.Services;

public class ListingParkingService : IListingParkingService
{
    private readonly IListingRepository _listingRepo;
    private readonly IListingParkingRepository _parkingRepo;
    private readonly IMapper _mapper;

    public ListingParkingService(IListingRepository listingRepo, IListingParkingRepository parkingRepo, IMapper mapper)
    {
        _listingRepo = listingRepo;
        _parkingRepo = parkingRepo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ParkingDto>> GetParkingAsync(int listingId)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        var parking = await _parkingRepo.GetByListingIdAsync(listingId);
        return _mapper.Map<IEnumerable<ParkingDto>>(parking);
    }

    public async Task<ParkingDto> AddParkingAsync(int listingId, AddParkingRequest request)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        var parking = _mapper.Map<ListingParking>(request);
        parking.ListingId = listingId;

        var result = await _parkingRepo.CreateAsync(parking);
        return _mapper.Map<ParkingDto>(result);
    }

    public async Task<ParkingDto?> UpdateParkingAsync(int listingId, int parkingId, UpdateParkingRequest request)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        var result = await _parkingRepo.UpdateAsync(parkingId, request.Quantity);
        if (result == null) return null;

        return _mapper.Map<ParkingDto>(result);
    }

    public async Task DeleteParkingAsync(int listingId, int parkingId)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        await _parkingRepo.DeleteAsync(parkingId);
    }
}
