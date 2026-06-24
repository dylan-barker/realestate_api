namespace RealEstateApi.Application.DTOs;

public record UpsertBuildingInfoRequest(
    decimal? ErfSize,
    decimal? FloorArea,
    int? ConstructionYear,
    int? FacingId,
    int? ZoningId
);

public record BuildingInfoDto(
    int Id,
    int ListingId,
    decimal? ErfSize,
    decimal? FloorArea,
    int? ConstructionYear,
    int? FacingId,
    int? ZoningId
);
