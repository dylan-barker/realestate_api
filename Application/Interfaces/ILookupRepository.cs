using RealEstateApi.Domain.Models;

namespace RealEstateApi.Application.Interfaces;

public interface ILookupRepository
{
    Task<IEnumerable<PropertyType>> GetPropertyTypesAsync();
    Task<IEnumerable<RoomType>> GetRoomTypesAsync();
    Task<IEnumerable<Feature>> GetFeaturesAsync();
    Task<IEnumerable<ConditionCategory>> GetConditionCategoriesAsync();
    Task<IEnumerable<ParkingType>> GetParkingTypesAsync();
    Task<IEnumerable<Facing>> GetFacingAsync();
    Task<IEnumerable<Zoning>> GetZoningAsync();
}
