namespace RealEstateApi.Domain.Models;

public class ListingParking
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public int ParkingTypeId { get; set; }
    public int Quantity { get; set; }
    public string? ParkingTypeDescription { get; set; }
}
