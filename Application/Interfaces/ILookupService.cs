using RealEstateApi.Application.DTOs;

namespace RealEstateApi.Application.Interfaces;

public interface ILookupService
{
    Task<IEnumerable<PropertyTypeDto>> GetPropertyTypesAsync();
    Task<IEnumerable<RoomTypeDto>> GetRoomTypesAsync();
    Task<IEnumerable<FeatureDto>> GetFeaturesAsync();
    Task<IEnumerable<ConditionCategoryDto>> GetConditionCategoriesAsync();
    Task<IEnumerable<ParkingTypeDto>> GetParkingTypesAsync();
    Task<IEnumerable<FacingDto>> GetFacingAsync();
    Task<IEnumerable<ZoningDto>> GetZoningAsync();
}
