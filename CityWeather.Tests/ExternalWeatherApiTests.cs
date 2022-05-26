using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CityWeather.Models.Weather;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace CityWeather.Tests;

public class ExternalWeatherApiTests
{
    private readonly string _apiKey;
    public ExternalWeatherApiTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        _apiKey = configuration.GetValue<string>("OpenWeatherMap:ApiKey");
    }
    [Fact]
    public async Task NameEndpointTest()
    {
        const string cityName = "Paris";
        const string country = "France";

        var countryApiAllEndpoint = 
            $"https://api.openweathermap.org/data/2.5/weather?q={cityName},{country}&appid={_apiKey}&units=metric";
        var client = new HttpClient();
        var responseString = await client.GetStringAsync(countryApiAllEndpoint);

        Assert.NotNull(responseString);
    }
    
    [Fact]
    public async Task NameEndpointJsonTest()
    {
        const string cityName = "Paris";
        const string country = "France";
        
        var countryApiAllEndpoint = 
            $"https://api.openweathermap.org/data/2.5/weather?q={cityName},{country}&appid={_apiKey}&units=metric";
        var client = new HttpClient();
        var responseString = await client.GetFromJsonAsync<WeatherRoot>(countryApiAllEndpoint);

        Assert.NotNull(responseString);
        Assert.Equal("Paris", responseString?.name);
    }
    
    [Fact]
    public async Task NameEndpointJsonWithMultipleResultsTest()
    {
        const string cityName = "London";
         var countryApiAllEndpoint = 
            $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={_apiKey}&units=metric";
        var client = new HttpClient();
        var responseString = await client.GetFromJsonAsync<WeatherRoot>(countryApiAllEndpoint);

        Assert.NotNull(responseString);
        Assert.Equal("London", responseString?.name);
    }
}