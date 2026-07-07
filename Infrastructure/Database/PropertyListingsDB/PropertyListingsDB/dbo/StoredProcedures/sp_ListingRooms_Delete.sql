
CREATE   PROCEDURE sp_ListingRooms_Delete
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM ListingRoom WHERE Id = @Id;
END

GO

