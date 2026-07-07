
CREATE   PROCEDURE sp_ListingBuildingInfo_GetByListingId
    @ListingId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, ListingId, ErfSize, FloorArea, ConstructionYear, FacingId, ZoningId
    FROM ListingBuildingInfo
    WHERE ListingId = @ListingId;
END

GO

