using Section12.Assignment22.Interfaces;
using Section12.Assignment22.Models;
using System.Collections.Generic;

namespace Section12.Assignment22.Services;

public class WeatherService : IWeatherService
{
    private List<CityWeather> weatherList = new()
    {
        new CityWeather(){
            CityUniqueCode = "LDN",
            CityName = "London",
            DateAndTime = Convert.ToDateTime("2030-01-01 8:00"),
            TemperatureFahrenheit = 33
        },
        new CityWeather(){
            CityUniqueCode = "NYC",
            CityName = "Newyork",
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

    public List<CityWeather> GetWeatherDetails()
    {
        return weatherList;
    }
    public CityWeather? GetWeatherByCityCode(string cityCode)
    {
        var r = weatherList.FirstOrDefault(x => x.CityUniqueCode == cityCode);
        return r;
    }
}
