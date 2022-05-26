using System.Text.Json;
using CityWeather.Models.City;
using CityWeather.Models.Weather;

namespace CityWeather.Services;

public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public WeatherService(HttpClient client)
    {
        _httpClient = client ?? throw new ArgumentNullException(nameof(client));
        _jsonOptions = new JsonSerializerOptions();
    }

    public async Task<WeatherRoot?> GetWeatherAsync(string cityName, string countryCode)
    {
        const string apiKey = "40e731b3833b662d8440f70f58e3b731";
        var uri = $"/data/2.5/weather?q={cityName},{countryCode}&appid={apiKey}&units=metric";
        var response = await _httpClient.GetAsync(uri);

        if (!response.IsSuccessStatusCode) return null;
        
        var contentStream = await response.Content.ReadAsStreamAsync();
        var weather = await JsonSerializer.DeserializeAsync<WeatherRoot>(contentStream, _jsonOptions);
        return weather;
    }
}

