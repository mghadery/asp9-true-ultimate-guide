using Section12.Assignment22.Models;

namespace Section12.Assignment22.Interfaces;

public interface IWeatherService
{
    CityWeather? GetWeatherByCityCode(string cityCode);
    List<CityWeather> GetWeatherDetails();
}