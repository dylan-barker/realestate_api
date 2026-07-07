CREATE TABLE [dbo].[Users] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Username]     NVARCHAR (100) NOT NULL,
    [PasswordHash] NVARCHAR (500) NOT NULL,
    [DisplayName]  NVARCHAR (200) NOT NULL,
    [Role]         NVARCHAR (20)  DEFAULT ('Agent') NOT NULL,
    [IsActive]     BIT            DEFAULT ((1)) NOT NULL,
    [CreatedAt]    DATETIME2 (7)  DEFAULT (sysutcdatetime()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

CREATE UNIQUE NONCLUSTERED INDEX [UX_Users_Username]
    ON [dbo].[Users]([Username] ASC);


GO

