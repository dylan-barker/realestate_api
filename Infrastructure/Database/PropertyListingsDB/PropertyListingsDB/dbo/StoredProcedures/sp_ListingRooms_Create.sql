
CREATE   PROCEDURE sp_ListingRooms_Create
    @ListingId INT,
    @Name NVARCHAR(200),
    @RoomTypeId INT,
    @RoomTypeOther NVARCHAR(200) = NULL,
    @PhotoUrl NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ListingRoom (ListingId, Name, RoomTypeId, RoomTypeOther, PhotoUrl, CreatedAt, UpdatedAt)
    VALUES (@ListingId, @Name, @RoomTypeId, @RoomTypeOther, @PhotoUrl, GETUTCDATE(), GETUTCDATE());

    DECLARE @Id INT = SCOPE_IDENTITY();

    SELECT Id, ListingId, Name, RoomTypeId, RoomTypeOther, PhotoUrl, CreatedAt, UpdatedAt
    FROM ListingRoom
    WHERE Id = @Id;
END

GO

