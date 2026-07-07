CREATE TABLE [dbo].[ListingValuation] (
    [Id]                INT             IDENTITY (1, 1) NOT NULL,
    [OwnersNetPrice]    DECIMAL (14, 2) NULL,
    [AgentValuation]    DECIMAL (14, 2) NULL,
    [CommissionPercent] DECIMAL (5, 2)  NULL,
    CONSTRAINT [PK_ListingValuation] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

