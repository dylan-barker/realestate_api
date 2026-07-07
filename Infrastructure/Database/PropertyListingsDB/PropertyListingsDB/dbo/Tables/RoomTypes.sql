CREATE TABLE [dbo].[RoomTypes] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Description] NVARCHAR (60) NULL,
    CONSTRAINT [PK_RoomTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

