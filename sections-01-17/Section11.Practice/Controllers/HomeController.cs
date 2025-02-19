using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace Section11.Practice.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("test-vc/{title}/{y}")]
        [HttpGet]
        public IActionResult testVc(string title, int y)
        {
            var res = ViewComponent("Grid", new { title, x = y });
            return res;
        }
    }
}
