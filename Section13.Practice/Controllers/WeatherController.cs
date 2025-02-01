using Microsoft.AspNetCore.Mvc;
using Section13.Practice.Interfaces;

namespace Section13.Practice.Controllers
{
    public class WeatherController(IWeatherService weatherService, IWebHostEnvironment webHostEnvironment) : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            var weatherList = weatherService.GetWeatherDetails();
            return View(weatherList);
        }
        [Route("/weather/{cityCode}")]
        public IActionResult GetCityWeather(string cityCode)
        {
            var r = weatherService.GetWeatherByCityCode(cityCode);
            if (r is null)
                return View("_Error");
            return View(r);
        }
        [Route("/error")]
        public IActionResult Error()
        {
            throw new Exception("test exception handling");
        }
        [Route("/env")]
        public IActionResult Env()
        {
            return Content(webHostEnvironment.EnvironmentName);
        }
    }
}
