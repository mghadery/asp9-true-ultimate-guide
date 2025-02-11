using Section14.Practice.Models;

namespace Section14.Practice.Interfaces;

public interface IWeatherService
{
    CityWeather? GetWeatherByCityCode(string cityCode);
    List<CityWeather> GetWeatherDetails();
}