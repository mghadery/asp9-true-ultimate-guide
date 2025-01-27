using Microsoft.AspNetCore.Mvc;

namespace Section10.Practice.Controllers;

public class HomeController : Controller
{
    [Route("/")]
    public IActionResult Index()
    {
        return View();
    }
    [Route("/Contact")]
    public IActionResult Contact()
    {
        return View();
    }
    [Route("/About")]
    public IActionResult About()
    {
        return View();
    }

    [Route("/PartialList/{title}")]
    public IActionResult PartialList(string listTitle)
    {
        ViewBag.ListTitle = listTitle;
        return PartialView("_List");
    }
}

