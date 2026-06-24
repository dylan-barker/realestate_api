using Dapper;
using RealEstateApi.Application.Interfaces;
using RealEstateApi.Domain.Models;
using RealEstateApi.Infrastructure.Data;

namespace RealEstateApi.Infrastructure.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public ContactRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Contact>> GetByListingIdAsync(int listingId)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Contact>(
            "sp_Contacts_GetByListingId",
            new { ListingId = listingId },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<Contact> CreateAsync(Contact contact)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Contact>(
            "sp_Contacts_Create",
            new
            {
                ListingId = contact.ListingId,
                FullName = contact.FullName,
                IdNumber = contact.IdNumber,
                CompanyName = contact.CompanyName,
                CompanyRegistrationNumber = contact.CompanyRegistrationNumber,
                MobilePhone = contact.MobilePhone,
                EmailAddress = contact.EmailAddress,
                Role = contact.Role
            },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<Contact?> UpdateAsync(Contact contact)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Contact>(
            "sp_Contacts_Update",
            new
            {
                Id = contact.Id,
                FullName = contact.FullName,
                IdNumber = contact.IdNumber,
                CompanyName = contact.CompanyName,
                CompanyRegistrationNumber = contact.CompanyRegistrationNumber,
                MobilePhone = contact.MobilePhone,
                EmailAddress = contact.EmailAddress,
                Role = contact.Role
            },
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(
            "sp_Contacts_Delete",
            new { Id = id },
            commandType: System.Data.CommandType.StoredProcedure);
    }
}
