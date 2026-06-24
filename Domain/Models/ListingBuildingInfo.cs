namespace RealEstateApi.Domain.Models;

public class ListingBuildingInfo
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public decimal? ErfSize { get; set; }
    public decimal? FloorArea { get; set; }
    public int? ConstructionYear { get; set; }
    public int? FacingId { get; set; }
    public int? ZoningId { get; set; }
}
