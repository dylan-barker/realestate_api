using RealEstateApi.Domain.Models;

namespace RealEstateApi.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
}
