using Dapper;
using RealEstateApi.Domain.Models;
using RealEstateApi.Infrastructure.Data;

namespace RealEstateApi.Infrastructure.Repositories;

public class RefreshTokenRepository
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
            "SELECT Id, UserId, TokenHash, ExpiresAt, CreatedAt, IsRevoked FROM RefreshTokens WHERE TokenHash = @TokenHash",
            new { TokenHash = tokenHash });
    }

    public async Task RevokeUserTokensAsync(int userId)
    {
        using var conn = _connectionFactory.CreateConnection();
        await conn.ExecuteAsync(
            "UPDATE RefreshTokens SET IsRevoked = 1 WHERE UserId = @UserId AND IsRevoked = 0",
            new { UserId = userId });
    }

    public async Task CreateAsync(RefreshToken refreshToken)
    {
        using var conn = _connectionFactory.CreateConnection();
        await conn.ExecuteAsync(
            "INSERT INTO RefreshTokens (UserId, TokenHash, ExpiresAt, CreatedAt, IsRevoked) VALUES (@UserId, @TokenHash, @ExpiresAt, @CreatedAt, @IsRevoked)",
            new { refreshToken.UserId, refreshToken.TokenHash, refreshToken.ExpiresAt, refreshToken.CreatedAt, refreshToken.IsRevoked });
    }

    public async Task RevokeAsync(int id)
    {
        using var conn = _connectionFactory.CreateConnection();
        await conn.ExecuteAsync(
            "UPDATE RefreshTokens SET IsRevoked = 1 WHERE Id = @Id", new { Id = id });
    }
}
