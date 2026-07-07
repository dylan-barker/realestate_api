
-- PropertyRunningCosts
CREATE   PROCEDURE sp_PropertyRunningCosts_Upsert
    @ListingId INT,
    @MonthlyLevy DECIMAL(18,2) = NULL,
    @MonthlyRates DECIMAL(18,2) = NULL,
    @Electricity DECIMAL(18,2) = NULL,
    @Water DECIMAL(18,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    MERGE PropertyRunningCosts AS target
    USING (SELECT @ListingId AS ListingId) AS source
    ON target.ListingId = source.ListingId
    WHEN MATCHED THEN
        UPDATE SET
            MonthlyLevy = @MonthlyLevy,
            MonthlyRates = @MonthlyRates,
            Electricity = @Electricity,
            Water = @Water
    WHEN NOT MATCHED THEN
        INSERT (ListingId, MonthlyLevy, MonthlyRates, Electricity, Water)
        VALUES (@ListingId, @MonthlyLevy, @MonthlyRates, @Electricity, @Water);

    SELECT Id, ListingId, MonthlyLevy, MonthlyRates, Electricity, Water
    FROM PropertyRunningCosts
    WHERE ListingId = @ListingId;
END

GO

