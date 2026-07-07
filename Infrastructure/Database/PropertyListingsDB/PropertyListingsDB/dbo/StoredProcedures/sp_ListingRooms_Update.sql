
CREATE   PROCEDURE sp_ListingRooms_Update
    @Id INT,
    @Name NVARCHAR(200) = NULL,
    @RoomTypeId INT = NULL,
    @RoomTypeOther NVARCHAR(200) = NULL,
    @PhotoUrl NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ListingRoom
    SET Name = COALESCE(@Name, Name),
        RoomTypeId = COALESCE(@RoomTypeId, RoomTypeId),
        RoomTypeOther = COALESCE(@RoomTypeOther, RoomTypeOther),
        PhotoUrl = COALESCE(@PhotoUrl, PhotoUrl),
        UpdatedAt = GETUTCDATE()
    WHERE Id = @Id;

    SELECT Id, ListingId, Name, RoomTypeId, RoomTypeOther, PhotoUrl, CreatedAt, UpdatedAt
    FROM ListingRoom
    WHERE Id = @Id;
END

GO

