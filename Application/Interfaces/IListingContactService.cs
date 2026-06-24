using RealEstateApi.Application.DTOs;

namespace RealEstateApi.Application.Interfaces;

public interface IListingContactService
{
    Task<IEnumerable<ContactDto>> GetContactsAsync(int listingId);
    Task<ContactDto> AddContactAsync(int listingId, AddContactRequest request);
    Task<ContactDto?> UpdateContactAsync(int listingId, int contactId, UpdateContactRequest request);
    Task DeleteContactAsync(int listingId, int contactId);
}
