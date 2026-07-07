CREATE TABLE [dbo].[Feature] (
    [Id]          INT            NOT NULL,
    [Category]    NVARCHAR (100) NULL,
    [Description] NVARCHAR (100) NULL,
    CONSTRAINT [PK_Feature] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

