using OrderManager.Core.DTOs;
using System.Security.Claims;

namespace OrderManager.Core.ServiceContracts;

public interface IJwtService
{
    AuthenticationResponse CreateJwtToken(UserDTO userDTO);
    ClaimsPrincipal? GetPrincipalFromJwtToken(string? jwtToken);
}
