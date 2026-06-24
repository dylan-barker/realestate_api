namespace RealEstateApi.Application.DTOs;

public record AddParkingRequest(int ParkingTypeId, int Quantity);
public record UpdateParkingRequest(int Quantity);

public record ParkingDto(
    int Id,
    int ListingId,
    int ParkingTypeId,
    int Quantity,
    string ParkingTypeDescription
);
