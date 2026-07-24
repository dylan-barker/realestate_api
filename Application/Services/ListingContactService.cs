using RealEstateApi.Application.DTOs;
using RealEstateApi.Domain.Models;
using RealEstateApi.Infrastructure.Repositories;

namespace RealEstateApi.Application.Services;

public class ListingContactService
{
    private readonly ListingRepository _listingRepo;
    private readonly ContactRepository _contactRepo;

    public ListingContactService(ListingRepository listingRepo, ContactRepository contactRepo)
    {
        _listingRepo = listingRepo;
        _contactRepo = contactRepo;
    }

    public async Task<IEnumerable<ContactDto>> GetContactsAsync(int listingId)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        var contacts = await _contactRepo.GetByListingIdAsync(listingId);
        return contacts.Select(c => new ContactDto(c.Id, c.FullName, c.IdNumber, c.CompanyName, c.CompanyRegistrationNumber, c.MobilePhone, c.EmailAddress, c.Role, c.ListingId));
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
        return new ContactDto(result.Id, result.FullName, result.IdNumber, result.CompanyName, result.CompanyRegistrationNumber, result.MobilePhone, result.EmailAddress, result.Role, result.ListingId);
    }

    public async Task<ContactDto?> UpdateContactAsync(int listingId, int contactId, UpdateContactRequest request)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        var contact = new Contact
        {
            Id = contactId,
            ListingId = listingId,
            FullName = request.FullName,
            IdNumber = request.IdNumber,
            CompanyName = request.CompanyName,
            CompanyRegistrationNumber = request.CompanyRegistrationNumber,
            MobilePhone = request.MobilePhone,
            EmailAddress = request.EmailAddress,
            Role = request.Role
        };

        var result = await _contactRepo.UpdateAsync(contact);
        return result is null ? null : new ContactDto(result.Id, result.FullName, result.IdNumber, result.CompanyName, result.CompanyRegistrationNumber, result.MobilePhone, result.EmailAddress, result.Role, result.ListingId);
    }

    public async Task DeleteContactAsync(int listingId, int contactId)
    {
        var listing = await _listingRepo.GetByIdAsync(listingId);
        if (listing == null) throw new KeyNotFoundException($"Listing {listingId} not found");

        await _contactRepo.DeleteAsync(contactId);
    }
}
