using RealEstateApi.Domain.Models;

namespace RealEstateApi.Application.Interfaces;

public interface IListingBuildingInfoRepository
{
    Task<ListingBuildingInfo?> GetByListingIdAsync(int listingId);
    Task<ListingBuildingInfo> UpsertAsync(ListingBuildingInfo info);
}
