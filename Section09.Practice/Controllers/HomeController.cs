using Microsoft.AspNetCore.Mvc;

namespace Section09.Practice.Controllers;

public class HomeController : Controller
{
    [Route("/")]
    public IActionResult Index()
    {
        ViewBag.LayoutOpt = 1;
        return View();
    }
    [Route("/Contact")]
    public IActionResult Contact()
    {
        ViewBag.LayoutOpt = 2;
        return View();
    }
    [Route("/About")]
    public IActionResult About()
    {
        ViewBag.LayoutOpt = 2;
        return View();
    }
}
