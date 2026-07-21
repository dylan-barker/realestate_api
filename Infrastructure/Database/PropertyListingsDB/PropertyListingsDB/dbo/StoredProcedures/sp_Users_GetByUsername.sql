
-- Get user by username (active only)
CREATE   PROCEDURE sp_Users_GetByUsername
    @Username NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Username, PasswordHash, DisplayName, Role, IsActive, CreatedAt
    FROM Users
    WHERE Username = @Username AND IsActive = 1;
END

GO
