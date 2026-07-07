-- ============================================
-- ListingRoom, Condition, Features Stored Procedures
-- ============================================

-- Rooms CRUD
CREATE   PROCEDURE sp_ListingRooms_GetByListingId
    @ListingId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, ListingId, Name, RoomTypeId, RoomTypeOther, PhotoUrl, CreatedAt, UpdatedAt
    FROM ListingRoom
    WHERE ListingId = @ListingId
    ORDER BY CreatedAt;
END

GO

