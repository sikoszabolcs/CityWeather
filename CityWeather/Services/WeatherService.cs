using System.Text.Json;

internal class WeatherService
{
    private HttpClient _httpClient;
    private JsonSerializerOptions _jsonOptions;

    public WeatherService(HttpClient client)
    {
        _httpClient = client ?? throw new ArgumentNullException(nameof(client));
        _jsonOptions = new JsonSerializerOptions();
    }

    public async Task<WeatherRoot?> GetWeatherAsync(City city)
    {
        var apiKey = "40e731b3833b662d8440f70f58e3b731";
        var uri = 
            string.Format("/data/2.5/weather?q={0},{1},{2}&appid={3}&units=metric", 
            city.Name, city.State, city.Country, apiKey);
        var response = await _httpClient.GetAsync(uri);
        WeatherRoot? weather = null!;
        if (response.IsSuccessStatusCode)
        {
            var contentStream = await response.Content.ReadAsStreamAsync();
            weather = await JsonSerializer.DeserializeAsync<WeatherRoot>(contentStream, _jsonOptions);
        }
        return weather;
    }
}

// References:
// https://docs.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio
// Swagger
// Postman
