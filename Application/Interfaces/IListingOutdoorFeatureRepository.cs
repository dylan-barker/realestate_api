using RealEstateApi.Domain.Models;

namespace RealEstateApi.Application.Interfaces;

public interface IListingOutdoorFeatureRepository
{
    Task<IEnumerable<ListingOutdoorFeature>> GetByListingIdAsync(int listingId);
    Task<ListingOutdoorFeature> AddAsync(ListingOutdoorFeature feature);
    Task DeleteAsync(int id);
    Task<IEnumerable<ListingOutdoorFeature>> ReplaceAllAsync(int listingId, IEnumerable<string> descriptions);
}
