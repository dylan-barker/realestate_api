CREATE TABLE [dbo].[ConditionCategory] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Description] NVARCHAR (60) NULL,
    CONSTRAINT [PK_ConditionCategory] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

