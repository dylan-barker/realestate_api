using RealEstateApi.Domain.Models;

namespace RealEstateApi.Application.Interfaces;

public interface IListingRepository
{
    Task<Listing?> GetByIdAsync(int id);
    Task<IEnumerable<Listing>> GetAllAsync(string? status, DateTime? dateFrom, DateTime? dateTo);
    Task<Listing> CreateAsync(int propertyTypeId, string? p24Ref);
    Task<Listing?> UpdateAsync(int id, string? status, string? p24Ref, int? propertyTypeId);
    Task DeleteAsync(int id);
    Task<Listing?> SubmitAsync(int id);
}
