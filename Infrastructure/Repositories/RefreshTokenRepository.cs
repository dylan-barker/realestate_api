using Dapper;
using RealEstateApi.Application.Interfaces;
using RealEstateApi.Domain.Models;
using RealEstateApi.Infrastructure.Data;
using System.Data;

namespace RealEstateApi.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public RefreshTokenRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash)
    {
        using var conn = _connectionFactory.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<RefreshToken>(
            "sp_RefreshTokens_GetByTokenHash",
            new { TokenHash = tokenHash },
            commandType: CommandType.StoredProcedure);
    }

    public async Task RevokeUserTokensAsync(int userId)
    {
        using var conn = _connectionFactory.CreateConnection();
        await conn.ExecuteAsync(
            "sp_RefreshTokens_RevokeUserTokens",
            new { UserId = userId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task CreateAsync(RefreshToken refreshToken)
    {
        using var conn = _connectionFactory.CreateConnection();
        await conn.ExecuteAsync(
            "sp_RefreshTokens_Create",
            new { refreshToken.UserId, refreshToken.TokenHash, refreshToken.ExpiresAt, refreshToken.CreatedAt, refreshToken.IsRevoked },
            commandType: CommandType.StoredProcedure);
    }

    public async Task RevokeAsync(int id)
    {
        using var conn = _connectionFactory.CreateConnection();
        await conn.ExecuteAsync(
            "sp_RefreshTokens_Revoke",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }
}
