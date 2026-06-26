using RealEstateApi.Application.DTOs;

namespace RealEstateApi.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}
