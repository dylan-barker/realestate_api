
-- Delete listing (cascade handled via FK or application)
CREATE   PROCEDURE sp_Listings_Delete
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Listings WHERE Id = @Id;
END

GO

