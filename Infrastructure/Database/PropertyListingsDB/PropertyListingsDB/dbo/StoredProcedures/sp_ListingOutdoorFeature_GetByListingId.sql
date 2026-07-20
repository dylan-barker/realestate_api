CREATE PROCEDURE sp_ListingOutdoorFeature_GetByListingId
    @ListingId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, ListingId, Description
    FROM ListingOutdoorFeature
    WHERE ListingId = @ListingId
    ORDER BY Id;
END
