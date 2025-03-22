namespace OrderManager.Core.DTOs;

public class AuthenticationResponse
{
    public string? UserName { get; set; } = string.Empty;
    public string? PersonName { get; set; } = string.Empty;
    public string? Token { get; set; } = string.Empty;
    public string? RefreshToken { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public DateTime RefreshExpiration { get; set; }
}
