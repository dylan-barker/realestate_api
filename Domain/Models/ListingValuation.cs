namespace RealEstateApi.Domain.Models;

public class ListingValuation
{
    public int Id { get; set; }
    public decimal? OwnersNetPrice { get; set; }
    public decimal? AgentValuation { get; set; }
    public decimal? CommissionPercent { get; set; }
}
