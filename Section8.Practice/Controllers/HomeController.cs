using Microsoft.AspNetCore.Mvc;

namespace Section8.Practice.Controllers
{
    
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
