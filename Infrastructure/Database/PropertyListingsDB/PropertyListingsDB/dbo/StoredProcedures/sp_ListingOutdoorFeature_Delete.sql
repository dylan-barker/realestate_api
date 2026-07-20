CREATE PROCEDURE sp_ListingOutdoorFeature_Delete
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM ListingOutdoorFeature WHERE Id = @Id;
END
