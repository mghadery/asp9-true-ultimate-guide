using Microsoft.AspNetCore.Mvc;
using Section12.Assignment22.Models;

namespace Section12.Assignment22.ViewComponents;

public class CityWeatherViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(CityWeather cityWeatherInfo,
        bool showDate, bool showDetailsLink)
    {
        int tempCat = 1;
        if (cityWeatherInfo.TemperatureFahrenheit > 74)
            tempCat = 3;
        else if (cityWeatherInfo.TemperatureFahrenheit >= 44)
            tempCat = 2;

        ViewBag.TempCat = tempCat;
        ViewBag.ShowDate = showDate;
        ViewBag.ShowDetailsLink = showDetailsLink;
        ViewBag.Time = cityWeatherInfo.DateAndTime.ToString("hh:mm tt");
        ViewBag.Date = DateTime.Today.ToString("dd MMM yyyy");

        return View("_CityWeather", cityWeatherInfo);
    }
}
