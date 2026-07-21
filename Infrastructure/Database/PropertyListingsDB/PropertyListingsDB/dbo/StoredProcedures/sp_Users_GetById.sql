
-- Get user by ID
CREATE   PROCEDURE sp_Users_GetById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Username, PasswordHash, DisplayName, Role, IsActive, CreatedAt
    FROM Users
    WHERE Id = @Id;
END

GO
