# API Changes Required

## 1. Outdoor Features — New Table + Endpoints

The Flutter app tracks outdoor features (pool, patio, solar, etc.) in `PropertyState.outdoorFeatures: List<String>` but never persists them to the API or loads them back. These are displayed in the **Property Features** screen under an "OUTDOOR FEATURES" heading (both predefined extras and custom free-text entries).

### Data shape

```dart
// Flutter PropertyState
List<String> outdoorFeatures;  // e.g. ['Swimming Pool', 'Solar Panels', 'Custom Feature']
```

### DB — New table

```sql
CREATE TABLE [dbo].[ListingOutdoorFeature] (
    [Id]          INT IDENTITY (1, 1) NOT NULL,
    [ListingId]   INT NOT NULL,
    [Description] NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_ListingOutdoorFeature] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ListingOutdoorFeature_Listing]
        FOREIGN KEY ([ListingId]) REFERENCES [dbo].[Listing] ([Id]) ON DELETE CASCADE
);
```

(Mirrors the pattern of `ListingRoomCustomFeature` but at listing level.)

### DB — Stored procedures

**`sp_ListingOutdoorFeature_GetByListingId.sql`**
```sql
CREATE PROCEDURE sp_ListingOutdoorFeature_GetByListingId
    @ListingId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, ListingId, Description
    FROM ListingOutdoorFeature
    WHERE ListingId = @ListingId
    ORDER BY Id;
END
```

**`sp_ListingOutdoorFeature_Add.sql`**
```sql
CREATE PROCEDURE sp_ListingOutdoorFeature_Add
    @ListingId INT,
    @Description NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ListingOutdoorFeature (ListingId, Description)
    VALUES (@ListingId, @Description);

    SELECT Id, ListingId, Description
    FROM ListingOutdoorFeature
    WHERE Id = SCOPE_IDENTITY();
END
```

**`sp_ListingOutdoorFeature_Delete.sql`**
```sql
CREATE PROCEDURE sp_ListingOutdoorFeature_Delete
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM ListingOutdoorFeature WHERE Id = @Id;
END
```

**`sp_ListingOutdoorFeature_ReplaceAll.sql`** (or use delete+insert in a transaction)
```sql
CREATE PROCEDURE sp_ListingOutdoorFeature_ReplaceAll
    @ListingId INT,
    @Descriptions NVARCHAR(MAX)  -- JSON array of strings
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    DELETE FROM ListingOutdoorFeature WHERE ListingId = @ListingId;

    INSERT INTO ListingOutdoorFeature (ListingId, Description)
    SELECT @ListingId, [value]
    FROM OPENJSON(@Descriptions);

    SELECT Id, ListingId, Description
    FROM ListingOutdoorFeature
    WHERE ListingId = @ListingId
    ORDER BY Id;
    COMMIT TRANSACTION;
END
```

### Domain model

```csharp
// Domain/Models/ListingOutdoorFeature.cs
namespace RealEstateApi.Domain.Models;

public class ListingOutdoorFeature
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public string Description { get; set; } = string.Empty;
}
```

### DTOs

```csharp
// Application/DTOs/OutdoorFeatureDtos.cs
namespace RealEstateApi.Application.DTOs;

public record OutdoorFeatureDto(int Id, int ListingId, string Description);

public record AddOutdoorFeatureRequest(string Description);

public record ReplaceOutdoorFeaturesRequest(List<string> Descriptions);
```

### Repository interface

```csharp
// Application/Interfaces/IListingOutdoorFeatureRepository.cs
namespace RealEstateApi.Application.Interfaces;

public interface IListingOutdoorFeatureRepository
{
    Task<IEnumerable<ListingOutdoorFeature>> GetByListingIdAsync(int listingId);
    Task<ListingOutdoorFeature> AddAsync(ListingOutdoorFeature feature);
    Task DeleteAsync(int id);
    Task<IEnumerable<ListingOutdoorFeature>> ReplaceAllAsync(int listingId, IEnumerable<string> descriptions);
}
```

### Repository implementation

```csharp
// Infrastructure/Repositories/ListingOutdoorFeatureRepository.cs
// (follow pattern of ListingRoomCustomFeature repository — Dapper, sp calls)
```

### Service interface + implementation

```csharp
// Application/Interfaces/IListingOutdoorFeatureService.cs
// Application/Services/ListingOutdoorFeatureService.cs
// (follow pattern of other listing sub-services like ListingParkingService)
```

### Controller endpoints

Add a new controller or add to `ListingsController`:

```csharp
// Option A: New controller
[ApiController]
[Authorize(Roles = "Admin,Agent")]
[Route("api/listings/{listingId}/outdoor-features")]
public class ListingOutdoorFeaturesController : ControllerBase
{
    // GET  /api/listings/{listingId}/outdoor-features
    // POST /api/listings/{listingId}/outdoor-features
    // DELETE /api/listings/{listingId}/outdoor-features/{id}
    // PUT  /api/listings/{listingId}/outdoor-features  (replace all)
}
```

### AutoMapper profile

Add to `MappingProfile.cs`:
```csharp
CreateMap<ListingOutdoorFeature, OutdoorFeatureDto>();
CreateMap<AddOutdoorFeatureRequest, ListingOutdoorFeature>();
```

### Modify `ListingResponse` to include outdoor features

Add to `ListingResponse` record:
```csharp
List<OutdoorFeatureDto> OutdoorFeatures  // default: new List<OutdoorFeatureDto>()
```

Update `BuildFullResponseAsync` in `ListingService.cs` to load and include outdoor features (run in parallel with the other tasks).

### DI registration

Add to `Program.cs`:
```csharp
builder.Services.AddScoped<IListingOutdoorFeatureRepository, ListingOutdoorFeatureRepository>();
builder.Services.AddScoped<IListingOutdoorFeatureService, ListingOutdoorFeatureService>();
```

---

## 2. Flutter-side — Send latitude/longitude in upsertAddress

**No API change needed** — the API already supports `latitude`/`longitude` in both `UpsertAddressRequest` DTO (`decimal?`) and the `ListingAddress` DB table.

**Fix needed in Flutter** (`property_repository_impl.dart` `upsertAddress()` method) — pass `latitude` and `longitude` from `PropertyState` to the `UpsertAddressRequest`:

```dart
// Currently NOT sent (lines 131-147 of property_repository_impl.dart)
// Add these to the request:
latitude: state.latitude,
longitude: state.longitude,
```

Also update `loadListing()` to read lat/lng from the API response (already done, lines 53-54 of property_repository_impl.dart — already reads them).

---

## 3. Flutter-side — ListingResponse DTO needs outdoorFeatures field

**No API change needed for shape** — the API response will include `OutdoorFeatures` (once the new endpoint/data is built).

**Fix needed in Flutter** (`listing_dtos.dart` `ListingResponse`) — add:
```dart
final List<OutdoorFeatureDto> outdoorFeatures;

// In constructor, default to const []
// In fromJson:
outdoorFeatures: (json['outdoorFeatures'] as List<dynamic>?)
    ?.map((e) => OutdoorFeatureDto.fromJson(e as Map<String, dynamic>))
    .toList() ?? [],
```

And create `OutdoorFeatureDto` in `lib/core/network/dto/`:
```dart
class OutdoorFeatureDto {
  final int id;
  final int listingId;
  final String description;

  const OutdoorFeatureDto({...});

  factory OutdoorFeatureDto.fromJson(Map<String, dynamic> json) => ...;
}
```

**Fix needed in Flutter** (`property_repository_impl.dart` `loadListing()`) — map outdoor features from `response.outdoorFeatures`:
```dart
outdoorFeatures: response.outdoorFeatures.map((f) => f.description).toList(),
```

---

## 4. Cleanup — `ListingFeature` table (optional)

The `ListingFeature` table exists in the DB schema but has:
- No stored procedures
- No API endpoints
- No service/repository code

It links listing-level features from the `Feature` lookup table but is completely unused by both API and Flutter app (room-level features use `ListingRoomFeature` instead).

**Recommendation:** Either:
- **Remove** the table and its `.sql` file if it's not needed
- Or **keep** as a placeholder for future listing-level (not room-level) feature linking

---

## Implementation order

1. DB: create `ListingOutdoorFeature` table + 4 stored procedures
2. Domain: add `ListingOutdoorFeature` model
3. DTOs: add `OutdoorFeatureDto`, `AddOutdoorFeatureRequest`, `ReplaceOutdoorFeaturesRequest`
4. Repository: interface + Dapper implementation
5. Service: interface + implementation
6. Controller: `ListingOutdoorFeaturesController` (or add to `ListingsController`)
7. Add to `ListingResponse` + `BuildFullResponseAsync`
8. AutoMapper: add new mappings
9. DI: register new types in `Program.cs`
10. Flutter: add `OutdoorFeatureDto`, update `ListingResponse`, update `loadListing()`, pass lat/lng in `upsertAddress()`
