using System.Text.Json;
using CityWeather.Models.Country;

namespace CityWeather.Services;

public class CountryInfoService
{
    private readonly HttpClient _httpClient;

    public CountryInfoService(HttpClient client)
    {
        _httpClient = client ?? throw new ArgumentNullException(nameof(client));
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
    
    public async Task<Country?> GetCountryInfoAsync(string country)
    {
        var uri = $"/v3.1/name/{country}";
        var response = await _httpClient.GetFromJsonAsync<CountryDetailsRoot[]>(uri);
        var countryInfo = response?.SingleOrDefault();
        if (countryInfo != null)
        {
            return new Country
            {
                Currencies = GetCurrencies(countryInfo.currencies),
                Alpha2Code = countryInfo.cca2,
                Alpha3Code = countryInfo.cca3
            };
        }

        return null;
    }
}
