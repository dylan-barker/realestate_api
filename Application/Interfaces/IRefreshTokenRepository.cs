using RealEstateApi.Domain.Models;

namespace RealEstateApi.Application.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenHashAsync(string tokenHash);
    Task RevokeUserTokensAsync(int userId);
    Task CreateAsync(RefreshToken refreshToken);
    Task RevokeAsync(int id);
}
