-- ============================================
-- ListingParking Stored Procedures
-- ============================================

CREATE OR ALTER PROCEDURE sp_ListingParking_GetByListingId
    @ListingId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT lp.Id, lp.ListingId, lp.ParkingTypeId, lp.Quantity, pt.Description AS ParkingTypeDescription
    FROM ListingParking lp
    INNER JOIN ParkingTypes pt ON pt.Id = lp.ParkingTypeId
    WHERE lp.ListingId = @ListingId
    ORDER BY pt.Description;
END
GO

CREATE OR ALTER PROCEDURE sp_ListingParking_Create
    @ListingId INT,
    @ParkingTypeId INT,
    @Quantity INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ListingParking (ListingId, ParkingTypeId, Quantity)
    VALUES (@ListingId, @ParkingTypeId, @Quantity);

    DECLARE @Id INT = SCOPE_IDENTITY();

    SELECT lp.Id, lp.ListingId, lp.ParkingTypeId, lp.Quantity, pt.Description AS ParkingTypeDescription
    FROM ListingParking lp
    INNER JOIN ParkingTypes pt ON pt.Id = lp.ParkingTypeId
    WHERE lp.Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE sp_ListingParking_Update
    @Id INT,
    @Quantity INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ListingParking
    SET Quantity = @Quantity
    WHERE Id = @Id;

    SELECT lp.Id, lp.ListingId, lp.ParkingTypeId, lp.Quantity, pt.Description AS ParkingTypeDescription
    FROM ListingParking lp
    INNER JOIN ParkingTypes pt ON pt.Id = lp.ParkingTypeId
    WHERE lp.Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE sp_ListingParking_Delete
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM ListingParking WHERE Id = @Id;
END
GO
