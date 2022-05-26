using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading.Tasks;
using CityWeather.Models.Country;
using Xunit;

namespace CityWeather.Tests;

public class ExternalCountryApiTests
{
    [Fact]
    public async Task AllEndpointTest()
    {
        const string countryApiAllEndpoint = "https://restcountries.com/v3.1/all";
        var client = new HttpClient();
        var responseString = await client.GetStringAsync(countryApiAllEndpoint);

        Assert.NotNull(responseString);
    }
    
    [Fact]
    public async Task NameEndpointTest()
    {
        const string countryApiNameEndpoint = "https://restcountries.com/v3.1/name/France";
        var client = new HttpClient();
        var responseString = await client.GetStringAsync(countryApiNameEndpoint);

        Assert.NotNull(responseString);
    }

    private static string GetCurrencies(Currencies currencies)
    {
        if (currencies == null) throw new ArgumentNullException(nameof(currencies));
        
        var type = currencies.GetType();
        var properties = type.GetProperties();
        var currencyCodes = 
            properties
                .Where(property => property.GetValue(currencies, null) != null)
                .Select(property => property.Name)
                .ToList();
        
        return string.Join(',', currencyCodes);
    }
    
    [Fact]
    public async Task NameEndpointJsonTestWithSingleCurrency()
    {
        const string countryApiNameEndpoint = "https://restcountries.com/v3.1/name/France";
        var client = new HttpClient();
        var countryDetails = await client.GetFromJsonAsync<CountryDetailsRoot[]>(countryApiNameEndpoint);
        Assert.NotNull(countryDetails);
        Assert.Single(countryDetails);
        Assert.Equal("FR", countryDetails?[0].cca2);
        Assert.Equal("FRA", countryDetails?[0].cca3);
        Assert.Equal("EUR", GetCurrencies(countryDetails?[0].currencies));
    }
    
    [Fact]
    public async Task NameEndpointJsonTestWithMultipleCurrencies()
    {
        const string countryApiNameEndpoint = "https://restcountries.com/v3.1/name/Cambodia";
        var client = new HttpClient();
        var countryDetails = await client.GetFromJsonAsync<CountryDetailsRoot[]>(countryApiNameEndpoint);
        Assert.NotNull(countryDetails);
        Assert.Single(countryDetails);
        Assert.Equal("KH", countryDetails?[0].cca2);
        Assert.Equal("KHM", countryDetails?[0].cca3);
        Assert.Equal("KHR,USD", GetCurrencies(countryDetails?[0].currencies));
    }
}