using RealEstateApi.Domain.Models;

namespace RealEstateApi.Application.Interfaces;

public interface IListingRoomRepository
{
    Task<IEnumerable<ListingRoom>> GetByListingIdAsync(int listingId);
    Task<ListingRoom> CreateAsync(ListingRoom room);
    Task<ListingRoom?> UpdateAsync(ListingRoom room);
    Task DeleteAsync(int id);
    Task<Condition?> GetConditionByRoomIdAsync(int listingRoomId);
    Task<Condition> UpsertConditionAsync(Condition condition);
    Task<IEnumerable<Feature>> GetLinkedFeaturesAsync(int listingRoomId);
    Task<IEnumerable<Feature>> LinkFeatureAsync(int listingRoomId, int featureId);
    Task<IEnumerable<Feature>> UnlinkFeatureAsync(int listingRoomId, int featureId);
    Task<IEnumerable<ListingRoomCustomFeature>> GetCustomFeaturesAsync(int listingRoomId);
    Task<ListingRoomCustomFeature> AddCustomFeatureAsync(ListingRoomCustomFeature feature);
    Task DeleteCustomFeatureAsync(int id);
}
