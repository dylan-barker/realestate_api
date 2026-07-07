
CREATE   PROCEDURE sp_ListingRoomCustomFeatures_Remove
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM ListingRoomCustomFeature WHERE Id = @Id;
END

GO

