using Dapper;
using RealEstateApi.Domain.Models;
using RealEstateApi.Infrastructure.Data;

namespace RealEstateApi.Infrastructure.Repositories;

public class ContactRepository
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
            "SELECT Id, FullName, IdNumber, CompanyName, CompanyRegistrationNumber, MobilePhone, EmailAddress, Role, ListingId " +
            "FROM Contact WHERE ListingId = @ListingId ORDER BY FullName",
            new { ListingId = listingId });
    }

    public async Task<Contact> CreateAsync(Contact contact)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Contact>(
            "INSERT INTO Contact (ListingId, FullName, IdNumber, CompanyName, CompanyRegistrationNumber, MobilePhone, EmailAddress, Role) " +
            "OUTPUT INSERTED.Id, INSERTED.FullName, INSERTED.IdNumber, INSERTED.CompanyName, INSERTED.CompanyRegistrationNumber, INSERTED.MobilePhone, INSERTED.EmailAddress, INSERTED.Role, INSERTED.ListingId " +
            "VALUES (@ListingId, @FullName, @IdNumber, @CompanyName, @CompanyRegistrationNumber, @MobilePhone, @EmailAddress, @Role)",
            contact);
    }

    public async Task<Contact?> UpdateAsync(Contact contact)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Contact>(
            "UPDATE Contact SET FullName = COALESCE(@FullName, FullName), IdNumber = COALESCE(@IdNumber, IdNumber), " +
            "CompanyName = COALESCE(@CompanyName, CompanyName), CompanyRegistrationNumber = COALESCE(@CompanyRegistrationNumber, CompanyRegistrationNumber), " +
            "MobilePhone = COALESCE(@MobilePhone, MobilePhone), EmailAddress = COALESCE(@EmailAddress, EmailAddress), Role = COALESCE(@Role, Role) " +
            "OUTPUT INSERTED.Id, INSERTED.FullName, INSERTED.IdNumber, INSERTED.CompanyName, INSERTED.CompanyRegistrationNumber, INSERTED.MobilePhone, INSERTED.EmailAddress, INSERTED.Role, INSERTED.ListingId " +
            "WHERE Id = @Id",
            contact);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(
            "DELETE FROM Contact WHERE Id = @Id", new { Id = id });
    }
}
