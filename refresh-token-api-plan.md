# Refresh Token Implementation — API & Database

## Overview

The Flutter client (already updated) now expects these changes on the API side. Implementation steps are ordered and grouped by layer.

---

## 1. Database — New `RefreshTokens` Table

```sql
CREATE TABLE RefreshTokens (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    TokenHash NVARCHAR(500) NOT NULL,
    ExpiresAt DATETIME2 NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    IsRevoked BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_RefreshTokens_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE INDEX IX_RefreshTokens_TokenHash ON RefreshTokens(TokenHash);
CREATE INDEX IX_RefreshTokens_UserId ON RefreshTokens(UserId);
```

Only **one valid refresh token per user** at a time. When a new one is issued (login or refresh), revoke the old one.

---

## 2. AppSettings — New Config

**`appsettings.json`** — add under the `Jwt` section:

```json
"Jwt": {
    "Secret": "CHANGE-ME-to-a-secret-key-at-least-32-characters-long",
    "Issuer": "RealEstateApi",
    "Audience": "RealEstateApi",
    "ExpiryHours": 1,
    "RefreshTokenExpiryDays": 60
}
```

---

## 3. JwtOptions — New Property

**File:** `Application/Services/JwtOptions.cs`

```csharp
    public int RefreshTokenExpiryDays { get; set; } = 60;
```

---

## 4. Domain Model — RefreshToken Entity

**New file:** `Domain/Models/RefreshToken.cs`

```csharp
namespace RealEstateApi.Domain.Models;

public class RefreshToken
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string TokenHash { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRevoked { get; set; }
}
```

---

## 5. DTOs — New Request/Response

**File:** `Application/DTOs/AuthDtos.cs`

Add these records:

```csharp
public record RefreshTokenRequest(string RefreshToken);

public record RefreshTokenResponse(string Token, DateTime ExpiresAt, string RefreshToken);
```

Update `LoginResponse` to include the refresh token:

```csharp
public record LoginResponse(string Token, DateTime ExpiresAt, string DisplayName, string Role, string RefreshToken);
```

---

## 6. Repository — IRefreshTokenRepository

**New file:** `Application/Interfaces/IRefreshTokenRepository.cs`

```csharp
using RealEstateApi.Domain.Models;

namespace RealEstateApi.Application.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenHashAsync(string tokenHash);
    Task RevokeUserTokensAsync(int userId);
    Task CreateAsync(RefreshToken refreshToken);
    Task RevokeAsync(int id);
}
```

**New file:** `Infrastructure/Repositories/RefreshTokenRepository.cs`

```csharp
using Dapper;
using RealEstateApi.Application.Interfaces;
using RealEstateApi.Domain.Models;
using RealEstateApi.Infrastructure.Data;

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
            @"INSERT INTO RefreshTokens (UserId, TokenHash, ExpiresAt, CreatedAt, IsRevoked)
              VALUES (@UserId, @TokenHash, @ExpiresAt, @CreatedAt, @IsRevoked)",
            refreshToken);
    }

    public async Task RevokeAsync(int id)
    {
        using var conn = _connectionFactory.CreateConnection();
        await conn.ExecuteAsync(
            "UPDATE RefreshTokens SET IsRevoked = 1 WHERE Id = @Id",
            new { Id = id });
    }
}
```

---

## 7. AuthService — Updated Logic

**File:** `Application/Services/AuthService.cs`

### Changes to `LoginAsync`

After creating the access token, also:

1. Generate a cryptographically random refresh token
2. Hash it (SHA256) and store in DB
3. Return the **raw** refresh token in the response (only the hash is stored)

```csharp
using System.Security.Cryptography;

// In LoginAsync, after building the access token:
var refreshTokenRaw = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
var refreshTokenHash = SHA256.HashData(Encoding.UTF8.GetBytes(refreshTokenRaw));

await _refreshTokenRepository.RevokeUserTokensAsync(user.Id);

var refreshTokenEntity = new RefreshToken
{
    UserId = user.Id,
    TokenHash = Convert.ToBase64String(refreshTokenHash),
    ExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpiryDays),
    CreatedAt = DateTime.UtcNow,
    IsRevoked = false
};

await _refreshTokenRepository.CreateAsync(refreshTokenEntity);

return new LoginResponse(tokenString, expires, user.DisplayName, user.Role, refreshTokenRaw);
```

### New Method — `RefreshTokenAsync`

```csharp
public async Task<RefreshTokenResponse?> RefreshTokenAsync(RefreshTokenRequest request)
{
    var rawHash = SHA256.HashData(Encoding.UTF8.GetBytes(request.RefreshToken));
    var tokenHash = Convert.ToBase64String(rawHash);

    var storedToken = await _refreshTokenRepository.GetByTokenHashAsync(tokenHash);
    if (storedToken is null || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
        return null;

    var user = await _userRepository.GetByIdAsync(storedToken.UserId);
    if (user is null || !user.IsActive)
        return null;

    // Revoke current refresh token (rotation)
    await _refreshTokenRepository.RevokeAsync(storedToken.Id);

    // Issue new access token
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.UTF8.GetBytes(_jwtOptions.Secret);
    var expires = DateTime.UtcNow.AddHours(_jwtOptions.ExpiryHours);
    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role),
        new Claim("displayName", user.DisplayName)
    };

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(claims),
        Expires = expires,
        Issuer = _jwtOptions.Issuer,
        Audience = _jwtOptions.Audience,
        SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
    };

    var newAccessToken = tokenHandler.CreateToken(tokenDescriptor);
    var newAccessTokenString = tokenHandler.WriteToken(newAccessToken);

    // Issue new refresh token
    var newRefreshRaw = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    var newRefreshHash = SHA256.HashData(Encoding.UTF8.GetBytes(newRefreshRaw));

    var newRefreshEntity = new RefreshToken
    {
        UserId = user.Id,
        TokenHash = Convert.ToBase64String(newRefreshHash),
        ExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpiryDays),
        CreatedAt = DateTime.UtcNow,
        IsRevoked = false
    };

    await _refreshTokenRepository.CreateAsync(newRefreshEntity);

    return new RefreshTokenResponse(newAccessTokenString, expires, newRefreshRaw);
}
```

### IAuthService — Add Method

```csharp
public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
    Task<RefreshTokenResponse?> RefreshTokenAsync(RefreshTokenRequest request);
}
```

### AuthService Constructor — Inject Repository

```csharp
private readonly IRefreshTokenRepository _refreshTokenRepository;

public AuthService(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IOptions<JwtOptions> jwtOptions)
{
    _userRepository = userRepository;
    _refreshTokenRepository = refreshTokenRepository;
    _jwtOptions = jwtOptions.Value;
}
```

---

## 8. AuthController — New Endpoint

**File:** `Controllers/AuthController.cs`

```csharp
[HttpPost("refresh")]
public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
{
    var result = await _authService.RefreshTokenAsync(request);
    if (result is null)
        return Unauthorized(new { message = "Invalid or expired refresh token" });

    return Ok(result);
}
```

Note: **Do not** add `[Authorize]` — this endpoint must be public (it's called when the access token has expired).

---

## 9. DI Registration in Program.cs

**File:** `Program.cs`

```csharp
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
```

Place alongside the existing `IUserRepository` registration (line ~49).

---

## 10. IUserRepository — Add GetByIdAsync

**File:** `Application/Interfaces/IUserRepository.cs`

```csharp
Task<User?> GetByIdAsync(int id);
```

**File:** `Infrastructure/Repositories/UserRepository.cs`

```csharp
public async Task<User?> GetByIdAsync(int id)
{
    using var conn = _connectionFactory.CreateConnection();
    return await conn.QueryFirstOrDefaultAsync<User>(
        "SELECT Id, Username, PasswordHash, DisplayName, Role, IsActive, CreatedAt FROM Users WHERE Id = @Id",
        new { Id = id });
}
```

---

## Summary of New Files

| File | Purpose |
|------|---------|
| `Domain/Models/RefreshToken.cs` | Entity model |
| `Application/Interfaces/IRefreshTokenRepository.cs` | Repository interface |
| `Infrastructure/Repositories/RefreshTokenRepository.cs` | Dapper implementation |

## Summary of Modified Files

| File | Change |
|------|--------|
| `appsettings.json` | Add `RefreshTokenExpiryDays` |
| `Application/Services/JwtOptions.cs` | Add `RefreshTokenExpiryDays` property |
| `Application/DTOs/AuthDtos.cs` | Add `RefreshTokenRequest`, `RefreshTokenResponse`; update `LoginResponse` |
| `Application/Services/AuthService.cs` | Inject `IRefreshTokenRepository`; generate & rotate refresh tokens; add `RefreshTokenAsync` |
| `Application/Interfaces/IAuthService.cs` | Add `RefreshTokenAsync` to interface |
| `Controllers/AuthController.cs` | Add `POST /api/auth/refresh` |
| `Application/Interfaces/IUserRepository.cs` | Add `GetByIdAsync` |
| `Infrastructure/Repositories/UserRepository.cs` | Implement `GetByIdAsync` |
| `Program.cs` | Register `IRefreshTokenRepository` |

## Security Notes

- **Never store raw refresh tokens** in the DB — always store SHA256 hashes
- **Rotate on every use** — each time a refresh token is consumed, revoke it and issue a new one. This limits the damage of a leaked token.
- **Refresh tokens have their own expiry** (60 days by default), independent of the access token
- **Server-side revocation** — set `IsRevoked = 1` to invalidate tokens. Useful for "log out everywhere" features.
- The `RefreshTokens` table grows over time; consider a background cleanup job to purge expired + revoked rows older than N days.
