-- ============================================
-- ListingValuation & PropertyRunningCosts Stored Procedures
-- ============================================

CREATE OR ALTER PROCEDURE sp_ListingValuation_Upsert
    @ListingId INT,
    @OwnersNetPrice DECIMAL(18,2) = NULL,
    @AgentValuation DECIMAL(18,2) = NULL,
    @CommissionPercent DECIMAL(5,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ValuationId INT;

    SELECT @ValuationId = ListingValuationId FROM Listings WHERE Id = @ListingId;

    IF @ValuationId IS NOT NULL
    BEGIN
        UPDATE ListingValuation
        SET OwnersNetPrice = @OwnersNetPrice,
            AgentValuation = @AgentValuation,
            CommissionPercent = @CommissionPercent
        WHERE Id = @ValuationId;
    END
    ELSE
    BEGIN
        INSERT INTO ListingValuation (OwnersNetPrice, AgentValuation, CommissionPercent)
        VALUES (@OwnersNetPrice, @AgentValuation, @CommissionPercent);

        SET @ValuationId = SCOPE_IDENTITY();

        UPDATE Listings
        SET ListingValuationId = @ValuationId, UpdatedAt = GETUTCDATE()
        WHERE Id = @ListingId;
    END

    SELECT Id, OwnersNetPrice, AgentValuation, CommissionPercent
    FROM ListingValuation
    WHERE Id = @ValuationId;
END
GO

CREATE OR ALTER PROCEDURE sp_ListingValuation_GetByListingId
    @ListingId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT lv.Id, lv.OwnersNetPrice, lv.AgentValuation, lv.CommissionPercent
    FROM ListingValuation lv
    INNER JOIN Listings l ON l.ListingValuationId = lv.Id
    WHERE l.Id = @ListingId;
END
GO

-- PropertyRunningCosts
CREATE OR ALTER PROCEDURE sp_PropertyRunningCosts_Upsert
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

CREATE OR ALTER PROCEDURE sp_PropertyRunningCosts_GetByListingId
    @ListingId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, ListingId, MonthlyLevy, MonthlyRates, Electricity, Water
    FROM PropertyRunningCosts
    WHERE ListingId = @ListingId;
END
GO
