
-- Facing
CREATE   PROCEDURE sp_Facing_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Description
    FROM Facing
    ORDER BY Description;
END

GO

