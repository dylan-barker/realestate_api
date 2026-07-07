
CREATE   PROCEDURE sp_ListingParking_Update
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

