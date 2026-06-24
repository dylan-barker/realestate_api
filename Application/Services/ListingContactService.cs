using RealEstateApi.Application.Interfaces;
using RealEstateApi.Application.DTOs;
using RealEstateApi.Domain.Models;
using RealEstateApi.Mappings;

namespace RealEstateApi.Application.Services;

public class ListingContactService : IListingContactService
{
    private readonly IListingRepository _listingRepo;
    private readonly IContactRepository _contactRepo;

    public ListingContactService(IListingRepository listingRepo, IContactRepository contactRepo)
    {
        _listingRepo = listingRepo;
        _contactRepo = contactRepo;
    }

    public async Task<IEnumerable<ContactDto>> GetContactsAsync(int listingId)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        var contacts = await _contactRepo.GetByListingIdAsync(listingId);
        return contacts.Select(c => c.ToDto());
    }

    public async Task<ContactDto> AddContactAsync(int listingId, AddContactRequest request)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        var contact = new Contact
        {
            ListingId = listingId,
            FullName = request.FullName,
            IdNumber = request.IdNumber,
            CompanyName = request.CompanyName,
            CompanyRegistrationNumber = request.CompanyRegistrationNumber,
            MobilePhone = request.MobilePhone,
            EmailAddress = request.EmailAddress,
            Role = request.Role
        };

        var result = await _contactRepo.CreateAsync(contact);
        return result.ToDto();
    }

    public async Task<ContactDto?> UpdateContactAsync(int listingId, int contactId, UpdateContactRequest request)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        var contact = new Contact
        {
            Id = contactId,
            FullName = request.FullName,
            IdNumber = request.IdNumber,
            CompanyName = request.CompanyName,
            CompanyRegistrationNumber = request.CompanyRegistrationNumber,
            MobilePhone = request.MobilePhone,
            EmailAddress = request.EmailAddress,
            Role = request.Role
        };

        var result = await _contactRepo.UpdateAsync(contact);
        return result?.ToDto();
    }

    public async Task DeleteContactAsync(int listingId, int contactId)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        await _contactRepo.DeleteAsync(contactId);
    }
}
