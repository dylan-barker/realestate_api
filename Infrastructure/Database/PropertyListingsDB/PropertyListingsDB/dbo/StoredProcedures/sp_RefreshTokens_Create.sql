
-- Create a new refresh token
CREATE   PROCEDURE sp_RefreshTokens_Create
    @UserId INT,
    @TokenHash NVARCHAR(500),
    @ExpiresAt DATETIME2,
    @CreatedAt DATETIME2,
    @IsRevoked BIT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO RefreshTokens (UserId, TokenHash, ExpiresAt, CreatedAt, IsRevoked)
    VALUES (@UserId, @TokenHash, @ExpiresAt, @CreatedAt, @IsRevoked);
END

GO
