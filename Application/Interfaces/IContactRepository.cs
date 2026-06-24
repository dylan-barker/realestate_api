using RealEstateApi.Domain.Models;

namespace RealEstateApi.Application.Interfaces;

public interface IContactRepository
{
    Task<IEnumerable<Contact>> GetByListingIdAsync(int listingId);
    Task<Contact> CreateAsync(Contact contact);
    Task<Contact?> UpdateAsync(Contact contact);
    Task DeleteAsync(int id);
}
