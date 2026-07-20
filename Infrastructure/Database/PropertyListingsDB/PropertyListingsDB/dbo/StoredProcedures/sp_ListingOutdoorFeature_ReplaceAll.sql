CREATE PROCEDURE sp_ListingOutdoorFeature_ReplaceAll
    @ListingId INT,
    @Descriptions NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    DELETE FROM ListingOutdoorFeature WHERE ListingId = @ListingId;

    INSERT INTO ListingOutdoorFeature (ListingId, Description)
    SELECT @ListingId, [value]
    FROM OPENJSON(@Descriptions);

    SELECT Id, ListingId, Description
    FROM ListingOutdoorFeature
    WHERE ListingId = @ListingId
    ORDER BY Id;

    COMMIT TRANSACTION;
END
