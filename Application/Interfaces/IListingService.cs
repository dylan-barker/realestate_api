using RealEstateApi.Application.DTOs;

namespace RealEstateApi.Application.Interfaces;

public interface IListingService
{
    Task<ListingResponse> CreateAsync(CreateListingRequest request);
    Task<ListingResponse?> GetByIdAsync(int id);
    Task<IEnumerable<ListingSummaryDto>> GetAllAsync(string? status, DateTime? dateFrom, DateTime? dateTo);
    Task<ListingResponse?> UpdateAsync(int id, UpdateListingRequest request);
    Task DeleteAsync(int id);
    Task<ListingResponse?> SubmitAsync(int id);
    Task<ListingAddressDto?> GetAddressAsync(int listingId);
    Task<ListingAddressDto> UpsertAddressAsync(int listingId, UpsertAddressRequest request);
    Task<BuildingInfoDto?> GetBuildingInfoAsync(int listingId);
    Task<BuildingInfoDto> UpsertBuildingInfoAsync(int listingId, UpsertBuildingInfoRequest request);
    Task<ValuationDto?> GetValuationAsync(int listingId);
    Task<ValuationDto> UpsertValuationAsync(int listingId, UpsertValuationRequest request);
    Task<RunningCostsDto?> GetRunningCostsAsync(int listingId);
    Task<RunningCostsDto> UpsertRunningCostsAsync(int listingId, UpsertRunningCostsRequest request);
}
