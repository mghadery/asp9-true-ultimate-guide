using System.ComponentModel.DataAnnotations;

namespace OrderManager.Core.DTOs;

public class TokensPair
{
    [Required(ErrorMessage = "JwtToken is blank")]
    public string? JwtToken { get; set; }
    [Required(ErrorMessage = "RefreshToken is blank")]
    public string? RefreshToken { get; set; }
}
