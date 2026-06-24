-- ============================================
-- Reference / Lookup Data Stored Procedures
-- ============================================

-- PropertyTypes
CREATE OR ALTER PROCEDURE sp_PropertyTypes_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Name, SortOrder, IsActive
    FROM PropertyTypes
    WHERE IsActive = 1
    ORDER BY SortOrder ASC;
END
GO

-- RoomTypes
CREATE OR ALTER PROCEDURE sp_RoomTypes_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Description
    FROM RoomTypes
    ORDER BY Description;
END
GO

-- Features
CREATE OR ALTER PROCEDURE sp_Features_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Category, Description
    FROM Features
    ORDER BY Category, Description;
END
GO

-- ConditionCategories
CREATE OR ALTER PROCEDURE sp_ConditionCategories_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Description
    FROM ConditionCategories
    ORDER BY Description;
END
GO

-- ParkingTypes
CREATE OR ALTER PROCEDURE sp_ParkingTypes_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Description
    FROM ParkingTypes
    ORDER BY Description;
END
GO

-- Facing
CREATE OR ALTER PROCEDURE sp_Facing_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Description
    FROM Facing
    ORDER BY Description;
END
GO

-- Zoning
CREATE OR ALTER PROCEDURE sp_Zoning_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Description
    FROM Zoning
    ORDER BY Description;
END
GO
