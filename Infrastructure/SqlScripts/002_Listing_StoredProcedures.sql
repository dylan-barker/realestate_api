-- ============================================
-- Listing CRUD Stored Procedures
-- ============================================

-- Create listing (draft status, auto-generate ReferenceNumber)
CREATE OR ALTER PROCEDURE sp_Listings_Create
    @PropertyTypeId INT,
    @P24Ref NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Id INT;
    DECLARE @RefNum NVARCHAR(20);
    DECLARE @Year CHAR(4) = CAST(YEAR(GETDATE()) AS CHAR(4));
    DECLARE @NextNum INT;

    -- Generate reference number: LST-YYYY-NNNNN
    SELECT @NextNum = ISNULL(MAX(CAST(SUBSTRING(ReferenceNumber, 10, 5) AS INT)), 0) + 1
    FROM Listings
    WHERE ReferenceNumber LIKE 'LST-' + @Year + '-%';

    SET @RefNum = 'LST-' + @Year + '-' + RIGHT('00000' + CAST(@NextNum AS NVARCHAR(5)), 5);

    INSERT INTO Listings (ReferenceNumber, P24Ref, PropertyTypeId, Status, CreatedAt, UpdatedAt)
    VALUES (@RefNum, @P24Ref, @PropertyTypeId, 'draft', GETUTCDATE(), GETUTCDATE());

    SET @Id = SCOPE_IDENTITY();

    SELECT Id, ReferenceNumber, P24Ref, PropertyTypeId, ListingValuationId, ListDate, Status, CreatedAt, UpdatedAt
    FROM Listings
    WHERE Id = @Id;
END
GO

-- Get listing by ID
CREATE OR ALTER PROCEDURE sp_Listings_GetById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, ReferenceNumber, P24Ref, PropertyTypeId, ListingValuationId, ListDate, Status, CreatedAt, UpdatedAt
    FROM Listings
    WHERE Id = @Id;
END
GO

-- List all listings with optional filters
CREATE OR ALTER PROCEDURE sp_Listings_GetAll
    @Status NVARCHAR(20) = NULL,
    @DateFrom DATETIME = NULL,
    @DateTo DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, ReferenceNumber, P24Ref, PropertyTypeId, ListingValuationId, ListDate, Status, CreatedAt, UpdatedAt
    FROM Listings
    WHERE (@Status IS NULL OR Status = @Status)
      AND (@DateFrom IS NULL OR CreatedAt >= @DateFrom)
      AND (@DateTo IS NULL OR CreatedAt <= @DateTo)
    ORDER BY CreatedAt DESC;
END
GO

-- Update listing-level fields
CREATE OR ALTER PROCEDURE sp_Listings_Update
    @Id INT,
    @Status NVARCHAR(20) = NULL,
    @P24Ref NVARCHAR(50) = NULL,
    @PropertyTypeId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Listings
    SET Status = COALESCE(@Status, Status),
        P24Ref = COALESCE(@P24Ref, P24Ref),
        PropertyTypeId = COALESCE(@PropertyTypeId, PropertyTypeId),
        UpdatedAt = GETUTCDATE()
    WHERE Id = @Id;

    SELECT Id, ReferenceNumber, P24Ref, PropertyTypeId, ListingValuationId, ListDate, Status, CreatedAt, UpdatedAt
    FROM Listings
    WHERE Id = @Id;
END
GO

-- Delete listing (cascade handled via FK or application)
CREATE OR ALTER PROCEDURE sp_Listings_Delete
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Listings WHERE Id = @Id;
END
GO

-- Submit listing (draft -> submitted)
CREATE OR ALTER PROCEDURE sp_Listings_Submit
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Listings
    SET Status = 'submitted',
        ListDate = GETUTCDATE(),
        UpdatedAt = GETUTCDATE()
    WHERE Id = @Id AND Status = 'draft';

    SELECT Id, ReferenceNumber, P24Ref, PropertyTypeId, ListingValuationId, ListDate, Status, CreatedAt, UpdatedAt
    FROM Listings
    WHERE Id = @Id;
END
GO
