
-- Get a refresh token by its hash
CREATE   PROCEDURE sp_RefreshTokens_GetByTokenHash
    @TokenHash NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, UserId, TokenHash, ExpiresAt, CreatedAt, IsRevoked
    FROM RefreshTokens
    WHERE TokenHash = @TokenHash;
END

GO
