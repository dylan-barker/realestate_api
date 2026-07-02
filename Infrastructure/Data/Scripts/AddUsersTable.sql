-- ============================================================
-- Add Users table for JWT authentication
-- Run this against your RealEstateDb database
-- ============================================================

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL,
    PasswordHash NVARCHAR(500) NOT NULL,
    DisplayName NVARCHAR(200) NOT NULL,
    Role NVARCHAR(20) NOT NULL DEFAULT 'Agent',
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

-- Unique index on username
CREATE UNIQUE INDEX UX_Users_Username ON Users (Username);

-- ============================================================
-- Seed default users
-- Password for both: admin123
-- ============================================================

INSERT INTO Users (Username, PasswordHash, DisplayName, Role)
VALUES
    ('admin',  '$2a$11$LKti979qGxQfaqvIvU2LkeHtd92boQIYtuqq1n5Z/TWAJzYtT7Vsa', 'Admin User',  'Admin'),
    ('agent1', '$2a$11$LKti979qGxQfaqvIvU2LkeHtd92boQIYtuqq1n5Z/TWAJzYtT7Vsa', 'Alice Agent', 'Agent');
