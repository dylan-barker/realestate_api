
-- Revoke all non-revoked refresh tokens for a user
CREATE   PROCEDURE sp_RefreshTokens_RevokeUserTokens
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE RefreshTokens
    SET IsRevoked = 1
    WHERE UserId = @UserId AND IsRevoked = 0;
END

GO
