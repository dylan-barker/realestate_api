
CREATE   PROCEDURE sp_ListingRoomCustomFeatures_GetByListingRoomId
    @ListingRoomId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, ListingRoomId, Description
    FROM ListingRoomCustomFeature
    WHERE ListingRoomId = @ListingRoomId;
END

GO

