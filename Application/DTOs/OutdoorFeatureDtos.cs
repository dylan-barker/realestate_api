namespace RealEstateApi.Application.DTOs;

public record OutdoorFeatureDto(int Id, int ListingId, string Description);

public record AddOutdoorFeatureRequest(string Description);

public record ReplaceOutdoorFeaturesRequest(List<string> Descriptions);
