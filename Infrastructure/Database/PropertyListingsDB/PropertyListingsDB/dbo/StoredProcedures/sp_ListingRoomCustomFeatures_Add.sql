
-- Room Custom Features
CREATE   PROCEDURE sp_ListingRoomCustomFeatures_Add
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

