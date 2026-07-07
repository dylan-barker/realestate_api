
CREATE   PROCEDURE sp_ListingAddress_GetByListingId
    @ListingId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ListingAddressId, ListingId, ErfNumber, EstateName, StreetNumber, UnitNumber, Street, Suburb, City, Province, Country, PostalCode, Latitude, Longitude
    FROM ListingAddress
    WHERE ListingId = @ListingId;
END

GO

