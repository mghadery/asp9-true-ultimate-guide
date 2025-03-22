using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OrderManager.Core.DTOs;
using OrderManager.Core.ServiceContracts;
using System.Security.Claims;

namespace OrderManager.API.Controllers.ver1;

[ApiVersion("1.0")]
[Route("api/ver{version:apiVersion}/[Controller]")]
[ApiController]
[AllowAnonymous]
//[EnableCors("TestPolicy")]
public class AccountController(IIdentityService identityService, IJwtService jwtService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDTO registerDTO)
    {
        var resp = await identityService.CreateUserAsync(registerDTO);
        if (resp.Succeeded)
        {
            await identityService.SignInAsync(registerDTO.UserName, false);
            AuthenticationResponse authenticationResponse = jwtService.CreateJwtToken(resp.UserDTO);

            resp.UserDTO.RefreshExpiration = authenticationResponse.RefreshExpiration;
            resp.UserDTO.RefreshToken = authenticationResponse.RefreshToken;
            await identityService.UpdateUserAsync(resp.UserDTO);
            return Ok(authenticationResponse);
        }
        else
        {
            return Problem("Failed to register"); //TODO more details
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody]LoginDTO loginDTO)
    {
        var resp = await identityService.PasswordSignInAsync(loginDTO.UserName, loginDTO.Password, false, false);
        if (resp.Succeeded)
        {
            AuthenticationResponse authenticationResponse = jwtService.CreateJwtToken(resp.UserDTO);

            resp.UserDTO.RefreshExpiration = authenticationResponse.RefreshExpiration;
            resp.UserDTO.RefreshToken = authenticationResponse.RefreshToken;
            await identityService.UpdateUserAsync(resp.UserDTO);
            return Ok(authenticationResponse);
        }
        else
        {
            return Problem("Wrong info", statusCode: 400); //TODO more details
        }
    }

    [HttpGet("[Action]")]
    public async Task<ActionResult> Logout()
    {
        await identityService.SignOut();
        return NoContent();
    }

    [HttpPost("getnewtoken")]
    public async Task<ActionResult> GetNewToken(TokensPair tokensPair)
    {
        var principal = jwtService.GetPrincipalFromJwtToken(tokensPair.JwtToken);
        var userName = principal.FindAll(ClaimTypes.NameIdentifier).First(x => !x.Value.Contains("-")).Value; //TODO

        var user = await identityService.FindUserAsync(userName);
        if (user is null ||
            user.RefreshToken != tokensPair.RefreshToken ||
            user.RefreshExpiration < DateTime.Now)
            return BadRequest("Invalid tokens");

        var r = jwtService.CreateJwtToken(user);

        user.RefreshExpiration = r.RefreshExpiration;
        user.RefreshToken = r.RefreshToken;
        await identityService.UpdateUserAsync(user);

        return Ok(r);
    }
}
