
CREATE   PROCEDURE sp_ListingParking_Delete
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM ListingParking WHERE Id = @Id;
END

GO

