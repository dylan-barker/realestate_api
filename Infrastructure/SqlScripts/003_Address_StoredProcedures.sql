-- ============================================
-- ListingAddress Stored Procedures
-- ============================================

CREATE OR ALTER PROCEDURE sp_ListingAddress_Upsert
    @ListingId INT,
    @ErfNumber NVARCHAR(50) = NULL,
    @EstateName NVARCHAR(200) = NULL,
    @StreetNumber NVARCHAR(50) = NULL,
    @UnitNumber NVARCHAR(50) = NULL,
    @Street NVARCHAR(200) = NULL,
    @Suburb NVARCHAR(200) = NULL,
    @City NVARCHAR(200) = NULL,
    @Province NVARCHAR(100) = NULL,
    @Country NVARCHAR(100) = NULL,
    @PostalCode NVARCHAR(20) = NULL,
    @Latitude DECIMAL(9,6) = NULL,
    @Longitude DECIMAL(9,6) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    MERGE ListingAddress AS target
    USING (SELECT @ListingId AS ListingId) AS source
    ON target.ListingId = source.ListingId
    WHEN MATCHED THEN
        UPDATE SET
            ErfNumber = @ErfNumber,
            EstateName = @EstateName,
            StreetNumber = @StreetNumber,
            UnitNumber = @UnitNumber,
            Street = @Street,
            Suburb = @Suburb,
            City = @City,
            Province = @Province,
            Country = @Country,
            PostalCode = @PostalCode,
            Latitude = @Latitude,
            Longitude = @Longitude
    WHEN NOT MATCHED THEN
        INSERT (ListingId, ErfNumber, EstateName, StreetNumber, UnitNumber, Street, Suburb, City, Province, Country, PostalCode, Latitude, Longitude)
        VALUES (@ListingId, @ErfNumber, @EstateName, @StreetNumber, @UnitNumber, @Street, @Suburb, @City, @Province, @Country, @PostalCode, @Latitude, @Longitude);

    SELECT ListingAddressId, ListingId, ErfNumber, EstateName, StreetNumber, UnitNumber, Street, Suburb, City, Province, Country, PostalCode, Latitude, Longitude
    FROM ListingAddress
    WHERE ListingId = @ListingId;
END
GO

CREATE OR ALTER PROCEDURE sp_ListingAddress_GetByListingId
    @ListingId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ListingAddressId, ListingId, ErfNumber, EstateName, StreetNumber, UnitNumber, Street, Suburb, City, Province, Country, PostalCode, Latitude, Longitude
    FROM ListingAddress
    WHERE ListingId = @ListingId;
END
GO
