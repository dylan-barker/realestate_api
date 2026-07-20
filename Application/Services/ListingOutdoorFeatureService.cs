using AutoMapper;
using RealEstateApi.Application.Interfaces;
using RealEstateApi.Application.DTOs;
using RealEstateApi.Domain.Models;

namespace RealEstateApi.Application.Services;

public class ListingOutdoorFeatureService : IListingOutdoorFeatureService
{
    private readonly IListingRepository _listingRepo;
    private readonly IListingOutdoorFeatureRepository _outdoorFeatureRepo;
    private readonly IMapper _mapper;

    public ListingOutdoorFeatureService(
        IListingRepository listingRepo,
        IListingOutdoorFeatureRepository outdoorFeatureRepo,
        IMapper mapper)
    {
        _listingRepo = listingRepo;
        _outdoorFeatureRepo = outdoorFeatureRepo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OutdoorFeatureDto>> GetByListingIdAsync(int listingId)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        var features = await _outdoorFeatureRepo.GetByListingIdAsync(listingId);
        return _mapper.Map<IEnumerable<OutdoorFeatureDto>>(features);
    }

    public async Task<OutdoorFeatureDto> AddAsync(int listingId, AddOutdoorFeatureRequest request)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        var feature = _mapper.Map<ListingOutdoorFeature>(request);
        feature.ListingId = listingId;

        var result = await _outdoorFeatureRepo.AddAsync(feature);
        return _mapper.Map<OutdoorFeatureDto>(result);
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
        return _mapper.Map<IEnumerable<OutdoorFeatureDto>>(result);
    }
}
