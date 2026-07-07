CREATE TABLE [dbo].[PropertyType] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (50) NULL,
    [SortOrder] INT           NULL,
    [IsActive]  BIT           NULL,
    CONSTRAINT [PK_PropertyType] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

