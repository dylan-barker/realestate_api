
-- ParkingTypes
CREATE   PROCEDURE sp_ParkingTypes_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Description
    FROM ParkingTypes
    ORDER BY Description;
END

GO

