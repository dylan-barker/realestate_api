CREATE PROCEDURE sp_ListingOutdoorFeature_Add
    @ListingId INT,
    @Description NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ListingOutdoorFeature (ListingId, Description)
    VALUES (@ListingId, @Description);

    SELECT Id, ListingId, Description
    FROM ListingOutdoorFeature
    WHERE Id = SCOPE_IDENTITY();
END
