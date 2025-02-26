using Microsoft.AspNetCore.Mvc;

namespace Section6.Assignment10.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return Ok("Welcome to the Best Bank");
        }
    }
}
