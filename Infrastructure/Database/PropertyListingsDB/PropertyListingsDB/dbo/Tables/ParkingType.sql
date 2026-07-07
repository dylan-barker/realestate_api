CREATE TABLE [dbo].[ParkingType] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Description] NVARCHAR (60) NULL,
    CONSTRAINT [PK_ParkingType] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

