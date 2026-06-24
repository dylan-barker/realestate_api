-- ============================================
-- Contact Stored Procedures
-- ============================================

CREATE OR ALTER PROCEDURE sp_Contacts_GetByListingId
    @ListingId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, FullName, IdNumber, CompanyName, CompanyRegistrationNumber, MobilePhone, EmailAddress, Role, ListingId
    FROM Contacts
    WHERE ListingId = @ListingId
    ORDER BY FullName;
END
GO

CREATE OR ALTER PROCEDURE sp_Contacts_Create
    @ListingId INT,
    @FullName NVARCHAR(200) = NULL,
    @IdNumber NVARCHAR(50) = NULL,
    @CompanyName NVARCHAR(200) = NULL,
    @CompanyRegistrationNumber NVARCHAR(50) = NULL,
    @MobilePhone NVARCHAR(50) = NULL,
    @EmailAddress NVARCHAR(200) = NULL,
    @Role NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Contacts (ListingId, FullName, IdNumber, CompanyName, CompanyRegistrationNumber, MobilePhone, EmailAddress, Role)
    VALUES (@ListingId, @FullName, @IdNumber, @CompanyName, @CompanyRegistrationNumber, @MobilePhone, @EmailAddress, @Role);

    DECLARE @Id INT = SCOPE_IDENTITY();

    SELECT Id, FullName, IdNumber, CompanyName, CompanyRegistrationNumber, MobilePhone, EmailAddress, Role, ListingId
    FROM Contacts
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE sp_Contacts_Update
    @Id INT,
    @FullName NVARCHAR(200) = NULL,
    @IdNumber NVARCHAR(50) = NULL,
    @CompanyName NVARCHAR(200) = NULL,
    @CompanyRegistrationNumber NVARCHAR(50) = NULL,
    @MobilePhone NVARCHAR(50) = NULL,
    @EmailAddress NVARCHAR(200) = NULL,
    @Role NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Contacts
    SET FullName = COALESCE(@FullName, FullName),
        IdNumber = COALESCE(@IdNumber, IdNumber),
        CompanyName = COALESCE(@CompanyName, CompanyName),
        CompanyRegistrationNumber = COALESCE(@CompanyRegistrationNumber, CompanyRegistrationNumber),
        MobilePhone = COALESCE(@MobilePhone, MobilePhone),
        EmailAddress = COALESCE(@EmailAddress, EmailAddress),
        Role = COALESCE(@Role, Role)
    WHERE Id = @Id;

    SELECT Id, FullName, IdNumber, CompanyName, CompanyRegistrationNumber, MobilePhone, EmailAddress, Role, ListingId
    FROM Contacts
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE sp_Contacts_Delete
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Contacts WHERE Id = @Id;
END
GO
