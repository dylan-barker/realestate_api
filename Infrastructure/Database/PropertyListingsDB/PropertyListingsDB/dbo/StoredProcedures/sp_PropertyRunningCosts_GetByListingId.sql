
CREATE   PROCEDURE sp_PropertyRunningCosts_GetByListingId
    @ListingId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, ListingId, MonthlyLevy, MonthlyRates, Electricity, Water
    FROM PropertyRunningCosts
    WHERE ListingId = @ListingId;
END

GO

