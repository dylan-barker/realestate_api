using RealEstateApi.Application.Interfaces;
using RealEstateApi.Application.DTOs;
using RealEstateApi.Mappings;

namespace RealEstateApi.Application.Services;

public class LookupService : ILookupService
{
    private readonly ILookupRepository _lookupRepository;

    public LookupService(ILookupRepository lookupRepository)
    {
        _lookupRepository = lookupRepository;
    }

    public async Task<IEnumerable<PropertyTypeDto>> GetPropertyTypesAsync()
    {
        var types = await _lookupRepository.GetPropertyTypesAsync();
        return types.Select(t => t.ToDto());
    }

    public async Task<IEnumerable<RoomTypeDto>> GetRoomTypesAsync()
    {
        var types = await _lookupRepository.GetRoomTypesAsync();
        return types.Select(t => t.ToDto());
    }

    public async Task<IEnumerable<FeatureDto>> GetFeaturesAsync()
    {
        var features = await _lookupRepository.GetFeaturesAsync();
        return features.Select(f => f.ToDto());
    }

    public async Task<IEnumerable<ConditionCategoryDto>> GetConditionCategoriesAsync()
    {
        var categories = await _lookupRepository.GetConditionCategoriesAsync();
        return categories.Select(c => c.ToDto());
    }

    public async Task<IEnumerable<ParkingTypeDto>> GetParkingTypesAsync()
    {
        var types = await _lookupRepository.GetParkingTypesAsync();
        return types.Select(t => t.ToDto());
    }

    public async Task<IEnumerable<FacingDto>> GetFacingAsync()
    {
        var facing = await _lookupRepository.GetFacingAsync();
        return facing.Select(f => f.ToDto());
    }

    public async Task<IEnumerable<ZoningDto>> GetZoningAsync()
    {
        var zoning = await _lookupRepository.GetZoningAsync();
        return zoning.Select(z => z.ToDto());
    }
}
