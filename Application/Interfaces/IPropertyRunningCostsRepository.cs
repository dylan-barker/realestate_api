using RealEstateApi.Domain.Models;

namespace RealEstateApi.Application.Interfaces;

public interface IPropertyRunningCostsRepository
{
    Task<PropertyRunningCosts?> GetByListingIdAsync(int listingId);
    Task<PropertyRunningCosts> UpsertAsync(PropertyRunningCosts costs);
}
