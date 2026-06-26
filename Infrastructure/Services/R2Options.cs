namespace RealEstateApi.Infrastructure.Services;

public class R2Options
{
    public const string SectionName = "R2";
    public string BucketName { get; init; } = string.Empty;
    public string AccessKeyId { get; init; } = string.Empty;
    public string SecretAccessKey { get; init; } = string.Empty;
    public string Endpoint { get; init; } = string.Empty;
    public string PublicUrl { get; init; } = string.Empty;
}
