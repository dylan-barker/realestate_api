-- ============================================
-- ListingRoom, Condition, Features Stored Procedures
-- ============================================

-- Rooms CRUD
CREATE OR ALTER PROCEDURE sp_ListingRooms_GetByListingId
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

CREATE OR ALTER PROCEDURE sp_ListingRooms_Create
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

CREATE OR ALTER PROCEDURE sp_ListingRooms_Update
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

CREATE OR ALTER PROCEDURE sp_ListingRooms_Delete
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM ListingRoom WHERE Id = @Id;
END
GO

-- Room Condition
CREATE OR ALTER PROCEDURE sp_Condition_Upsert
    @ListingRoomId INT,
    @ConditionRating INT = NULL,
    @Notes NVARCHAR(MAX) = NULL,
    @ConditionCategoryId INT
AS
BEGIN
    SET NOCOUNT ON;

    MERGE Condition AS target
    USING (SELECT @ListingRoomId AS ListingRoomId) AS source
    ON target.ListingRoomId = source.ListingRoomId
    WHEN MATCHED THEN
        UPDATE SET
            ConditionRating = @ConditionRating,
            Notes = @Notes,
            ConditionCategoryId = @ConditionCategoryId
    WHEN NOT MATCHED THEN
        INSERT (ListingRoomId, ConditionRating, Notes, ConditionCategoryId)
        VALUES (@ListingRoomId, @ConditionRating, @Notes, @ConditionCategoryId);

    SELECT Id, ListingRoomId, ConditionRating, Notes, ConditionCategoryId
    FROM Condition
    WHERE ListingRoomId = @ListingRoomId;
END
GO

CREATE OR ALTER PROCEDURE sp_Condition_GetByListingRoomId
    @ListingRoomId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, ListingRoomId, ConditionRating, Notes, ConditionCategoryId
    FROM Condition
    WHERE ListingRoomId = @ListingRoomId;
END
GO

-- Room Features (junction)
CREATE OR ALTER PROCEDURE sp_ListingRoomFeatures_GetByListingRoomId
    @ListingRoomId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT f.Id, f.Category, f.Description
    FROM ListingRoomFeature lrf
    INNER JOIN Features f ON f.Id = lrf.FeatureId
    WHERE lrf.ListingRoomId = @ListingRoomId;
END
GO

CREATE OR ALTER PROCEDURE sp_ListingRoomFeatures_Link
    @ListingRoomId INT,
    @FeatureId INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM ListingRoomFeature WHERE ListingRoomId = @ListingRoomId AND FeatureId = @FeatureId)
    BEGIN
        INSERT INTO ListingRoomFeature (ListingRoomId, FeatureId)
        VALUES (@ListingRoomId, @FeatureId);
    END

    SELECT f.Id, f.Category, f.Description
    FROM ListingRoomFeature lrf
    INNER JOIN Features f ON f.Id = lrf.FeatureId
    WHERE lrf.ListingRoomId = @ListingRoomId;
END
GO

CREATE OR ALTER PROCEDURE sp_ListingRoomFeatures_Unlink
    @ListingRoomId INT,
    @FeatureId INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM ListingRoomFeature
    WHERE ListingRoomId = @ListingRoomId AND FeatureId = @FeatureId;

    SELECT f.Id, f.Category, f.Description
    FROM ListingRoomFeature lrf
    INNER JOIN Features f ON f.Id = lrf.FeatureId
    WHERE lrf.ListingRoomId = @ListingRoomId;
END
GO

-- Room Custom Features
CREATE OR ALTER PROCEDURE sp_ListingRoomCustomFeatures_Add
    @ListingRoomId INT,
    @Description NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ListingRoomCustomFeature (ListingRoomId, Description)
    VALUES (@ListingRoomId, @Description);

    DECLARE @Id INT = SCOPE_IDENTITY();

    SELECT Id, ListingRoomId, Description
    FROM ListingRoomCustomFeature
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE sp_ListingRoomCustomFeatures_Remove
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM ListingRoomCustomFeature WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE sp_ListingRoomCustomFeatures_GetByListingRoomId
    @ListingRoomId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, ListingRoomId, Description
    FROM ListingRoomCustomFeature
    WHERE ListingRoomId = @ListingRoomId;
END
GO
