using RealEstateApi.Application.DTOs;
using RealEstateApi.Infrastructure.Repositories;

namespace RealEstateApi.Application.Services;

public class LookupService
{
    private readonly LookupRepository _lookupRepository;

    public LookupService(LookupRepository lookupRepository)
    {
        _lookupRepository = lookupRepository;
    }

    public async Task<IEnumerable<PropertyTypeDto>> GetPropertyTypesAsync()
    {
        var types = await _lookupRepository.GetPropertyTypesAsync();
        return types.Select(t => new PropertyTypeDto(t.Id, t.Name, t.SortOrder, t.IsActive));
    }

    public async Task<IEnumerable<RoomTypeDto>> GetRoomTypesAsync()
    {
        var types = await _lookupRepository.GetRoomTypesAsync();
        return types.Select(t => new RoomTypeDto(t.Id, t.Description));
    }

    public async Task<IEnumerable<FeatureDto>> GetFeaturesAsync()
    {
        var features = await _lookupRepository.GetFeaturesAsync();
        return features.Select(f => new FeatureDto(f.Id, f.Category, f.Description));
    }

    public async Task<IEnumerable<ConditionCategoryDto>> GetConditionCategoriesAsync()
    {
        var categories = await _lookupRepository.GetConditionCategoriesAsync();
        return categories.Select(c => new ConditionCategoryDto(c.Id, c.Description));
    }

    public async Task<IEnumerable<ParkingTypeDto>> GetParkingTypesAsync()
    {
        var types = await _lookupRepository.GetParkingTypesAsync();
        return types.Select(t => new ParkingTypeDto(t.Id, t.Description));
    }

    public async Task<IEnumerable<FacingDto>> GetFacingAsync()
    {
        var facing = await _lookupRepository.GetFacingAsync();
        return facing.Select(f => new FacingDto(f.Id, f.Description));
    }

    public async Task<IEnumerable<ZoningDto>> GetZoningAsync()
    {
        var zoning = await _lookupRepository.GetZoningAsync();
        return zoning.Select(z => new ZoningDto(z.Id, z.Description));
    }
}
