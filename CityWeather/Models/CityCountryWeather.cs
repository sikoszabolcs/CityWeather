using CityWeather.Models.Weather;

namespace CityWeather.Models;

public class CityCountryWeather
{
    public City.City City { get; set; }
    public Country.Country Country { get; set; }
    public WeatherRoot Weather { get; set; }
}