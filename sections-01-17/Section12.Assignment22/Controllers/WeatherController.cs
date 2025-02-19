using Microsoft.AspNetCore.Mvc;
using Section12.Assignment22.Interfaces;

namespace Section12.Assignment22.Controllers
{
    public class WeatherController(IWeatherService weatherService) : Controller
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
    }
}
