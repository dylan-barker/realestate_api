namespace RealEstateApi.Domain.Models;

public class ListingOutdoorFeature
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public string Description { get; set; } = string.Empty;
}
