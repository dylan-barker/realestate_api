namespace RealEstateApi.Domain.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Role { get; set; } = "Agent";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
}
