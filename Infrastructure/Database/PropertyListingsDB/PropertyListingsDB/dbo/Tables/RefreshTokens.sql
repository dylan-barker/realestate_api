CREATE TABLE [dbo].[RefreshTokens] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [UserId]    INT            NOT NULL,
    [TokenHash] NVARCHAR (500) NOT NULL,
    [ExpiresAt] DATETIME2 (7)  NOT NULL,
    [CreatedAt] DATETIME2 (7)  DEFAULT (sysutcdatetime()) NOT NULL,
    [IsRevoked] BIT            DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RefreshTokens_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);


GO

CREATE NONCLUSTERED INDEX [IX_RefreshTokens_TokenHash]
    ON [dbo].[RefreshTokens]([TokenHash] ASC);


GO

CREATE NONCLUSTERED INDEX [IX_RefreshTokens_UserId]
    ON [dbo].[RefreshTokens]([UserId] ASC);


GO
