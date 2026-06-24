using RealEstateApi.Domain.Models;
using RealEstateApi.Application.DTOs;

namespace RealEstateApi.Mappings;

public static class DtoMappings
{
    // Lookup
    public static PropertyTypeDto ToDto(this PropertyType entity) =>
        new(entity.Id, entity.Name, entity.SortOrder, entity.IsActive);

    public static RoomTypeDto ToDto(this RoomType entity) =>
        new(entity.Id, entity.Description);

    public static FeatureDto ToDto(this Feature entity) =>
        new(entity.Id, entity.Category, entity.Description);

    public static ConditionCategoryDto ToDto(this ConditionCategory entity) =>
        new(entity.Id, entity.Description);

    public static ParkingTypeDto ToDto(this ParkingType entity) =>
        new(entity.Id, entity.Description);

    public static FacingDto ToDto(this Facing entity) =>
        new(entity.Id, entity.Description);

    public static ZoningDto ToDto(this Zoning entity) =>
        new(entity.Id, entity.Description);

    // Listing summaries
    public static ListingSummaryDto ToSummaryDto(this Listing entity) =>
        new(entity.Id, entity.ReferenceNumber, entity.P24Ref, entity.PropertyTypeId,
            entity.ListingValuationId, entity.ListDate, entity.Status,
            entity.CreatedAt, entity.UpdatedAt);

    // Address
    public static ListingAddressDto ToDto(this ListingAddress entity) =>
        new(entity.ListingAddressId, entity.ListingId, entity.ErfNumber,
            entity.EstateName, entity.StreetNumber, entity.UnitNumber, entity.Street,
            entity.Suburb, entity.City, entity.Province, entity.Country,
            entity.PostalCode, entity.Latitude, entity.Longitude);

    // Building Info
    public static BuildingInfoDto ToDto(this ListingBuildingInfo entity) =>
        new(entity.Id, entity.ListingId, entity.ErfSize, entity.FloorArea,
            entity.ConstructionYear, entity.FacingId, entity.ZoningId);

    // Valuation
    public static ValuationDto ToDto(this ListingValuation entity) =>
        new(entity.Id, entity.OwnersNetPrice, entity.AgentValuation, entity.CommissionPercent);

    // Running Costs
    public static RunningCostsDto ToDto(this PropertyRunningCosts entity) =>
        new(entity.Id, entity.ListingId, entity.MonthlyLevy, entity.MonthlyRates,
            entity.Electricity, entity.Water);

    // Room
    public static RoomDto ToDto(this ListingRoom entity, RoomConditionDto? condition,
        List<FeatureDto> features, List<CustomFeatureDto> customFeatures) =>
        new(entity.Id, entity.ListingId, entity.Name, entity.RoomTypeId,
            entity.RoomTypeOther, entity.PhotoUrl, entity.CreatedAt, entity.UpdatedAt,
            condition, features, customFeatures);

    // Condition
    public static RoomConditionDto ToDto(this Condition entity) =>
        new(entity.Id, entity.ListingRoomId, entity.ConditionRating, entity.Notes,
            entity.ConditionCategoryId);

    // Custom Feature
    public static CustomFeatureDto ToDto(this ListingRoomCustomFeature entity) =>
        new(entity.Id, entity.ListingRoomId, entity.Description);

    // Parking (with description)
    public static ParkingDto ToParkingDto(this ListingParking entity, string parkingTypeDescription) =>
        new(entity.Id, entity.ListingId, entity.ParkingTypeId, entity.Quantity, parkingTypeDescription);

    // Contact
    public static ContactDto ToDto(this Contact entity) =>
        new(entity.Id, entity.FullName, entity.IdNumber, entity.CompanyName,
            entity.CompanyRegistrationNumber, entity.MobilePhone, entity.EmailAddress,
            entity.Role, entity.ListingId);

    // Listing response (full nested)
    public static ListingResponse ToFullDto(this Listing listing,
        ListingAddressDto? address,
        BuildingInfoDto? buildingInfo,
        ValuationDto? valuation,
        RunningCostsDto? runningCosts,
        List<RoomDto> rooms,
        List<ParkingDto> parking,
        List<ContactDto> contacts) =>
        new(listing.Id, listing.ReferenceNumber, listing.P24Ref, listing.PropertyTypeId,
            listing.ListingValuationId, listing.ListDate, listing.Status,
            listing.CreatedAt, listing.UpdatedAt,
            address, buildingInfo, valuation, runningCosts, rooms, parking, contacts);
}
