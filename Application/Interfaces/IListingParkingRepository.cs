using RealEstateApi.Domain.Models;

namespace RealEstateApi.Application.Interfaces;

public interface IListingParkingRepository
{
    Task<IEnumerable<ListingParking>> GetByListingIdAsync(int listingId);
    Task<ListingParking> CreateAsync(ListingParking parking);
    Task<ListingParking?> UpdateAsync(int id, int quantity);
    Task DeleteAsync(int id);
}
