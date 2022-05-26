using System.Text.Json;

public class CountryInfoService
{
    private HttpClient _httpClient;
    private JsonSerializerOptions _jsonOptions;

    public CountryInfoService(HttpClient client)
    {
        _httpClient = client ?? throw new ArgumentNullException(nameof(client));
        _jsonOptions = new JsonSerializerOptions();
    }

    public async Task<Country?> GetCountryInfoAsync(string city)
    {
        var uri = "";
        var response = await _httpClient.GetAsync(uri);
        var contentStream = await response.Content.ReadAsStreamAsync();
        var countryInfo = await JsonSerializer.DeserializeAsync<Country>(contentStream, _jsonOptions);
        return countryInfo;
    }
}

// References:
// https://docs.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio
// Swagger
// Postman
