using RealEstateApi.Domain.Models;

namespace RealEstateApi.Application.Interfaces;

public interface IListingAddressRepository
{
    Task<ListingAddress?> GetByListingIdAsync(int listingId);
    Task<ListingAddress> UpsertAsync(ListingAddress address);
}
