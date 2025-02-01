using Section13.Practice.Models;

namespace Section13.Practice.Interfaces;

public interface IWeatherService
{
    CityWeather? GetWeatherByCityCode(string cityCode);
    List<CityWeather> GetWeatherDetails();
}