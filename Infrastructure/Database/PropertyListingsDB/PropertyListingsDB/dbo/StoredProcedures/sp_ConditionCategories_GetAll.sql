
-- ConditionCategories
CREATE   PROCEDURE sp_ConditionCategories_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Description
    FROM ConditionCategories
    ORDER BY Description;
END

GO

