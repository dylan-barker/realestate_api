namespace RealEstateApi.Application.Services;

public class JwtOptions
{
    public const string SectionName = "Jwt";
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = "RealEstateApi";
    public string Audience { get; set; } = "RealEstateApi";
    public int ExpiryHours { get; set; } = 1;
}
