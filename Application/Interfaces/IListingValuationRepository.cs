using RealEstateApi.Domain.Models;

namespace RealEstateApi.Application.Interfaces;

public interface IListingValuationRepository
{
    Task<ListingValuation?> GetByListingIdAsync(int listingId);
    Task<ListingValuation> UpsertAsync(int listingId, ListingValuation valuation);
}
