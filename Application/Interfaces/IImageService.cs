namespace RealEstateApi.Application.Interfaces;

public interface IImageService
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType);
    Task DeleteAsync(string fileName);
}
