using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CityWeather.Models.Weather;
using Xunit;

namespace CityWeather.Tests;

public class ExternalWeatherApiTests
{
    [Fact]
    public async Task NameEndpointTest()
    {
        const string cityName = "Paris";
        const string country = "France";
        const string apiKey = "40e731b3833b662d8440f70f58e3b731";
        const string countryApiAllEndpoint = 
            $"https://api.openweathermap.org/data/2.5/weather?q={cityName},{country}&appid={apiKey}&units=metric";
        var client = new HttpClient();
        var responseString = await client.GetStringAsync(countryApiAllEndpoint);

        Assert.NotNull(responseString);
    }
    
    [Fact]
    public async Task NameEndpointJsonTest()
    {
        const string cityName = "Paris";
        const string country = "France";
        const string apiKey = "40e731b3833b662d8440f70f58e3b731";
        const string countryApiAllEndpoint = 
            $"https://api.openweathermap.org/data/2.5/weather?q={cityName},{country}&appid={apiKey}&units=metric";
        var client = new HttpClient();
        var responseString = await client.GetFromJsonAsync<WeatherRoot>(countryApiAllEndpoint);

        Assert.NotNull(responseString);
        Assert.Equal("Paris", responseString?.name);
    }
    
    [Fact]
    public async Task NameEndpointJsonWithMultipleResultsTest()
    {
        const string cityName = "London";
        const string apiKey = "40e731b3833b662d8440f70f58e3b731";
        const string countryApiAllEndpoint = 
            $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={apiKey}&units=metric";
        var client = new HttpClient();
        var responseString = await client.GetFromJsonAsync<WeatherRoot>(countryApiAllEndpoint);

        Assert.NotNull(responseString);
        Assert.Equal("London", responseString?.name);
    }
}