
-- RoomTypes
CREATE   PROCEDURE sp_RoomTypes_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Description
    FROM RoomTypes
    ORDER BY Description;
END

GO

