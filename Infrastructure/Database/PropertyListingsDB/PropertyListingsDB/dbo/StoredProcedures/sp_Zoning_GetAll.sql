
-- Zoning
CREATE   PROCEDURE sp_Zoning_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Description
    FROM Zoning
    ORDER BY Description;
END

GO

