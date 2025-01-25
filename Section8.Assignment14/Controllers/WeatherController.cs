using Microsoft.AspNetCore.Mvc;
using Section8.Assignment14.Models;

namespace Section8.Assignment14.Controllers
{
    public class WeatherController : Controller
    {
        List<CityWeather> weatherList = new()
        {
            new CityWeather(){
                CityUniqueCode = "LDN",
                CityName = "London",
                DateAndTime = Convert.ToDateTime("2030-01-01 8:00"),
                TemperatureFahrenheit = 33
            },
            new CityWeather(){
                CityUniqueCode = "NYC",
                CityName = "London",
                DateAndTime = Convert.ToDateTime("2030-01-01 3:00"),
                TemperatureFahrenheit = 60
            },
            new CityWeather(){
                CityUniqueCode = "PAR",
                CityName = "Paris",
                DateAndTime = Convert.ToDateTime("2030-01-01 9:00"),
                TemperatureFahrenheit = 82
            }
        };

        [Route("/")]
        public IActionResult Index()
        {
            return View(weatherList);
        }
        [Route("/weather/{cityCode}")]
        public IActionResult GetCityWeather(string cityCode)
        {
            var r = weatherList.FirstOrDefault(x => x.CityUniqueCode == cityCode);
            if (r is null)
                return View("Error");
            return View(r);
        }
    }
}
