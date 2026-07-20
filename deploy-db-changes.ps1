$connString = "Server=.\SQLEXPRESS;Database=PropertyListingsDB;Trusted_Connection=True;TrustServerCertificate=True;"

Write-Host "Creating ListingOutdoorFeature table..." -ForegroundColor Cyan
sqlcmd -S ".\SQLEXPRESS" -d "PropertyListingsDB" -E -Q @"
CREATE TABLE [dbo].[ListingOutdoorFeature] (
    [Id]          INT IDENTITY (1, 1) NOT NULL,
    [ListingId]   INT NOT NULL,
    [Description] NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_ListingOutdoorFeature] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ListingOutdoorFeature_Listing]
        FOREIGN KEY ([ListingId]) REFERENCES [dbo].[Listing] ([Id]) ON DELETE CASCADE
);
"@

Write-Host "Creating stored procedures..." -ForegroundColor Cyan
Get-ChildItem "$PSScriptRoot\Infrastructure\Database\PropertyListingsDB\PropertyListingsDB\dbo\StoredProcedures\sp_ListingOutdoorFeature_*.sql" | ForEach-Object {
    Write-Host "  $($_.Name)" -ForegroundColor Gray
    sqlcmd -S ".\SQLEXPRESS" -d "PropertyListingsDB" -E -i $_.FullName
}

Write-Host "Dropping unused ListingFeature table (if exists)..." -ForegroundColor Cyan
sqlcmd -S ".\SQLEXPRESS" -d "PropertyListingsDB" -E -Q "DROP TABLE IF EXISTS [dbo].[ListingFeature];"

Write-Host "Done!" -ForegroundColor Green
