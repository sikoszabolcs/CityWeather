using System.Text.Json;
using CityWeather.Models.Weather;

namespace CityWeather.Services;

public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly string _apiKey;
    private const string ApiKeyConfigName = "OpenWeatherMap:ApiKey";

    public WeatherService(HttpClient client, IConfiguration configuration)
    {
        _httpClient = client ?? throw new ArgumentNullException(nameof(client));
        _jsonOptions = new JsonSerializerOptions();
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        _apiKey = configuration[ApiKeyConfigName] ??
                  throw new InvalidOperationException($"Could not load the {ApiKeyConfigName}");
    }

    public async Task<WeatherRoot?> GetWeatherAsync(string cityName, string countryCode)
    {
        var uri = $"/data/2.5/weather?q={cityName},{countryCode}&appid={_apiKey}&units=metric";
        var response = await _httpClient.GetAsync(uri);

        if (!response.IsSuccessStatusCode) return null;
        
        var contentStream = await response.Content.ReadAsStreamAsync();
        var weather = await JsonSerializer.DeserializeAsync<WeatherRoot>(contentStream, _jsonOptions);
        return weather;
    }
}

