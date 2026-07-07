
CREATE   PROCEDURE sp_Contacts_Delete
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Contacts WHERE Id = @Id;
END

GO

