using System.Text.Json;
using CityWeather.Models.Country;

namespace CityWeather.Services;

public class CountryInfoService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public CountryInfoService(HttpClient client)
    {
        _httpClient = client ?? throw new ArgumentNullException(nameof(client));
        _jsonOptions = new JsonSerializerOptions();
    }

    public async Task<Country?> GetCountryInfoAsync(string city)
    {
        var uri = "/v3.1/all";
        var response = await _httpClient.GetAsync(uri);
        var contentStream = await response.Content.ReadAsStreamAsync();
        var countryInfo = await JsonSerializer.DeserializeAsync<Country>(contentStream, _jsonOptions);
        return countryInfo;
    }
}
