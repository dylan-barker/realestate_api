
-- Revoke a specific refresh token by ID
CREATE   PROCEDURE sp_RefreshTokens_Revoke
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE RefreshTokens
    SET IsRevoked = 1
    WHERE Id = @Id;
END

GO
