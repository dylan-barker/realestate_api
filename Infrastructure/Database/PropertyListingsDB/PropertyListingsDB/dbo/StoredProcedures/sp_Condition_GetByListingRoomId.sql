
CREATE   PROCEDURE sp_Condition_GetByListingRoomId
    @ListingRoomId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, ListingRoomId, ConditionRating, Notes, ConditionCategoryId
    FROM Condition
    WHERE ListingRoomId = @ListingRoomId;
END

GO

