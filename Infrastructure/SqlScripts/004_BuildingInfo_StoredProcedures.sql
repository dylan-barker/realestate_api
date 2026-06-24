-- ============================================
-- ListingBuildingInfo Stored Procedures
-- ============================================

CREATE OR ALTER PROCEDURE sp_ListingBuildingInfo_Upsert
    @ListingId INT,
    @ErfSize DECIMAL(12,2) = NULL,
    @FloorArea DECIMAL(12,2) = NULL,
    @ConstructionYear INT = NULL,
    @FacingId INT = NULL,
    @ZoningId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    MERGE ListingBuildingInfo AS target
    USING (SELECT @ListingId AS ListingId) AS source
    ON target.ListingId = source.ListingId
    WHEN MATCHED THEN
        UPDATE SET
            ErfSize = @ErfSize,
            FloorArea = @FloorArea,
            ConstructionYear = @ConstructionYear,
            FacingId = @FacingId,
            ZoningId = @ZoningId
    WHEN NOT MATCHED THEN
        INSERT (ListingId, ErfSize, FloorArea, ConstructionYear, FacingId, ZoningId)
        VALUES (@ListingId, @ErfSize, @FloorArea, @ConstructionYear, @FacingId, @ZoningId);

    SELECT Id, ListingId, ErfSize, FloorArea, ConstructionYear, FacingId, ZoningId
    FROM ListingBuildingInfo
    WHERE ListingId = @ListingId;
END
GO

CREATE OR ALTER PROCEDURE sp_ListingBuildingInfo_GetByListingId
    @ListingId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, ListingId, ErfSize, FloorArea, ConstructionYear, FacingId, ZoningId
    FROM ListingBuildingInfo
    WHERE ListingId = @ListingId;
END
GO
