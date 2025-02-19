using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Section14.Assignment24.Models;

namespace Section14.Assignment24.Controllers;

public class HomeController(IOptions<SocialMediaLinksOptions> options) : Controller
{
    [Route("/")]
    [HttpGet]
    public IActionResult Index()
    {
        SocialMediaLinksOptions socialMediaLinksOptions = options.Value;
        ViewBag.Links = socialMediaLinksOptions;
        return View();
    }
}
