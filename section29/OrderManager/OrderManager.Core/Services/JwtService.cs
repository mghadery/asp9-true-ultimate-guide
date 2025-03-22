using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrderManager.Core.DTOs;
using OrderManager.Core.ServiceContracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OrderManager.Core.Services;

public class JwtService(IConfiguration configuration): IJwtService
{
    public AuthenticationResponse CreateJwtToken(UserDTO userDTO)
    {
        var section = configuration.GetSection("Jwt");
        string? issuer = section.GetValue<string>("Issuer");
        string? audience = section.GetValue<string>("Audience");
        double expirationMinutes = section.GetValue<double>("ExpirationMinutes");
        DateTime expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);
        string? key = section.GetValue<string>("Key");
        double refreshExpirationMinutes = configuration.GetValue<double>("RefreshToken:ExpirationMinutes");
        var refreshExpiration = DateTime.Now.AddMinutes(refreshExpirationMinutes);

        Claim[] claims = [
            new Claim(JwtRegisteredClaimNames.Sub, userDTO.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds().ToString()),
            new Claim(ClaimTypes.NameIdentifier, userDTO.UserName),
            new Claim(ClaimTypes.Name, userDTO.PersonName),
            ];

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(issuer, audience, claims, null, expiration, signingCredentials);

        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        string token = handler.WriteToken(jwtSecurityToken);
        return new AuthenticationResponse()
        {
            Expiration = expiration,
            PersonName = userDTO.PersonName,
            UserName = userDTO.UserName,
            Token = token,
            RefreshToken = GenerateRefreshToken(),
            RefreshExpiration = refreshExpiration
        };
    }

    public ClaimsPrincipal? GetPrincipalFromJwtToken(string? jwtToken)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = configuration.GetValue<string>("Jwt:Issuer"),
            ValidateAudience = true,
            ValidAudience = configuration.GetValue<string>("Jwt:Audience"),
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Key")))
        };
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        ClaimsPrincipal claimsPrincipal = jwtSecurityTokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out SecurityToken validatedToken);

        if (validatedToken is not JwtSecurityToken jst ||
            !jst.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase) ||
            claimsPrincipal is null)
            throw new SecurityTokenException("Invalid token");
        return claimsPrincipal;
    }

    private string GenerateRefreshToken()
    {
        byte[] x = new byte[64];
        RandomNumberGenerator.Create().GetBytes(x);
        return Convert.ToBase64String(x);
    }
}
