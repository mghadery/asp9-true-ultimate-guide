using Microsoft.AspNetCore.Identity;

namespace OrderManager.Infrastructure.Persistent.IdentityEntities;

public class User : IdentityUser<Guid>
{
    public string? PersonName { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshExpiration { get; set; }
}
