namespace RealEstateApi.Domain.Models;

public class ListingRoomCustomFeature
{
    public int Id { get; set; }
    public int ListingRoomId { get; set; }
    public string Description { get; set; } = string.Empty;
}
