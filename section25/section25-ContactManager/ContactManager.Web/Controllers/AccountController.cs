using ContactManager.Core.Domain.RepositorieContracts;
using ContactManager.Core.DTOs;
using ContactManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.Web.Controllers;

[Route("[Controller]")]
//[AllowAnonymous]
[Authorize(Policy = "NotLoggedIn")]
public class AccountController(IIdentityService identityService) : Controller
{
    [Route("[Action]")]
    public IActionResult Register()
    {
        return View();
    }

    [Route("[Action]")]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterDTO registerDTO)
    {
        if (!ModelState.IsValid)
            return View(registerDTO);

        var res = await identityService.CreateUserAsync(registerDTO);
        if (!res.Succeeded)
        {
            res.Errors.ForEach(x => ModelState.AddModelError("register", x));
            return View(registerDTO);
        }
        await identityService.SignInAsync(registerDTO.UserName, false);
        return RedirectToAction("Index", "Person");
    }

    [Route("[Action]")]
    public IActionResult Login()
    {
        return View();
    }

    [Route("[Action]/returnurl")]
    [HttpPost]
    public async Task<IActionResult> Login(LoginDTO loginDTO, string? returnUrl)
    {
        if (!ModelState.IsValid)
            return View(loginDTO);

        var res = await identityService.PasswordSignInAsync(loginDTO.UserName, loginDTO.Password, false, false);
        if (!res)
            return View(loginDTO);
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            return LocalRedirect(returnUrl);
        return RedirectToAction("Index", "Person");
    }
}
