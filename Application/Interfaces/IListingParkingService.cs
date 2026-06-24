using RealEstateApi.Application.DTOs;

namespace RealEstateApi.Application.Interfaces;

public interface IListingParkingService
{
    Task<IEnumerable<ParkingDto>> GetParkingAsync(int listingId);
    Task<ParkingDto> AddParkingAsync(int listingId, AddParkingRequest request);
    Task<ParkingDto?> UpdateParkingAsync(int listingId, int parkingId, UpdateParkingRequest request);
    Task DeleteParkingAsync(int listingId, int parkingId);
}
