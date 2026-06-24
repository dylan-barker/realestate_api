namespace RealEstateApi.Application.DTOs;

public record PropertyTypeDto(int Id, string Name, int SortOrder, bool IsActive);
public record RoomTypeDto(int Id, string Description);
public record FeatureDto(int Id, string Category, string Description);
public record ConditionCategoryDto(int Id, string Description);
public record ParkingTypeDto(int Id, string Description);
public record FacingDto(int Id, string Description);
public record ZoningDto(int Id, string Description);
