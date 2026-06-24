namespace RealEstateApi.Application.DTOs;

public record UpsertAddressRequest(
    string? ErfNumber,
    string? EstateName,
    string? StreetNumber,
    string? UnitNumber,
    string? Street,
    string? Suburb,
    string? City,
    string? Province,
    string? Country,
    string? PostalCode,
    decimal? Latitude,
    decimal? Longitude
);

public record ListingAddressDto(
    int ListingAddressId,
    int ListingId,
    string? ErfNumber,
    string? EstateName,
    string? StreetNumber,
    string? UnitNumber,
    string? Street,
    string? Suburb,
    string? City,
    string? Province,
    string? Country,
    string? PostalCode,
    decimal? Latitude,
    decimal? Longitude
);
