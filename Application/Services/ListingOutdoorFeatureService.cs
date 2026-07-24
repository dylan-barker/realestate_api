using RealEstateApi.Application.DTOs;
using RealEstateApi.Domain.Models;
using RealEstateApi.Infrastructure.Repositories;

namespace RealEstateApi.Application.Services;

public class ListingOutdoorFeatureService
{
    private readonly ListingRepository _listingRepo;
    private readonly ListingOutdoorFeatureRepository _outdoorFeatureRepo;

    public ListingOutdoorFeatureService(ListingRepository listingRepo, ListingOutdoorFeatureRepository outdoorFeatureRepo)
    {
        _listingRepo = listingRepo;
        _outdoorFeatureRepo = outdoorFeatureRepo;
    }

    public async Task<IEnumerable<OutdoorFeatureDto>> GetByListingIdAsync(int listingId)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        var features = await _outdoorFeatureRepo.GetByListingIdAsync(listingId);
        return features.Select(f => new OutdoorFeatureDto(f.Id, f.ListingId, f.Description));
    }

    public async Task<OutdoorFeatureDto> AddAsync(int listingId, AddOutdoorFeatureRequest request)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        var feature = new ListingOutdoorFeature
        {
            ListingId = listingId,
            Description = request.Description
        };

        var result = await _outdoorFeatureRepo.AddAsync(feature);
        return new OutdoorFeatureDto(result.Id, result.ListingId, result.Description);
    }

    public async Task DeleteAsync(int listingId, int id)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        await _outdoorFeatureRepo.DeleteAsync(id);
    }

    public async Task<IEnumerable<OutdoorFeatureDto>> ReplaceAllAsync(int listingId, ReplaceOutdoorFeaturesRequest request)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        var result = await _outdoorFeatureRepo.ReplaceAllAsync(listingId, request.Descriptions);
        return result.Select(f => new OutdoorFeatureDto(f.Id, f.ListingId, f.Description));
    }
}
