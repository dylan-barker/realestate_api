using RealEstateApi.Application.DTOs;

namespace RealEstateApi.Application.Interfaces;

public interface IListingOutdoorFeatureService
{
    Task<IEnumerable<OutdoorFeatureDto>> GetByListingIdAsync(int listingId);
    Task<OutdoorFeatureDto> AddAsync(int listingId, AddOutdoorFeatureRequest request);
    Task DeleteAsync(int listingId, int id);
    Task<IEnumerable<OutdoorFeatureDto>> ReplaceAllAsync(int listingId, ReplaceOutdoorFeaturesRequest request);
}
