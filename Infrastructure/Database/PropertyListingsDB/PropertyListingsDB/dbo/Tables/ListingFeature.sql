CREATE TABLE [dbo].[ListingFeature] (
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [ListingId] INT NULL,
    [FeatureId] INT NULL,
    [Quantity]  INT NULL,
    CONSTRAINT [PK_ListingFeature] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ListingFeature_Feature] FOREIGN KEY ([FeatureId]) REFERENCES [dbo].[Feature] ([Id]),
    CONSTRAINT [FK_ListingFeature_Listing] FOREIGN KEY ([ListingId]) REFERENCES [dbo].[Listing] ([Id])
);


GO

