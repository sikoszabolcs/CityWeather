using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CityWeather.Models;
using CityWeather.Models.City;
using Xunit;

namespace CityWeather.Tests
{
    public class HttpEndpointTests
    {
        [Fact]
        public async Task TestAddCityEndpoint()
        {
            await using var application = new WeatherApplication();
            var client = application.CreateClient();

            var response = await client.PostAsJsonAsync(
                "/city",
                new City
                {
                    Id = 0,
                    Country = "France",
                    Name = "Paris",
                    EstablishedDate = new DateTime(0),
                    Rating = TouristRating.Good,
                    EstimatedPopulation = 2165423,
                    State = "Île-de-France"
                });
            
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
 
            var cityCountryWeatherCollection = await client.GetFromJsonAsync<CityCountryWeather[]>("/searchByName/Paris");
            Assert.NotNull(cityCountryWeatherCollection);
            Assert.Single(cityCountryWeatherCollection);
            Assert.Equal("Paris", cityCountryWeatherCollection?[0].City.Name);
            Assert.Equal("France", cityCountryWeatherCollection?[0].City.Country);
            Assert.Equal(new DateTime(0), cityCountryWeatherCollection?[0].City.EstablishedDate);
            Assert.Equal(TouristRating.Good, cityCountryWeatherCollection?[0].City.Rating);
            Assert.Equal((uint)2165423, cityCountryWeatherCollection?[0].City.EstimatedPopulation);
            Assert.Equal("Île-de-France", cityCountryWeatherCollection?[0].City.State);
        }
        
        [Fact]
        public async Task TestUpdateCityEndpoint()
        {
            await using var application = new WeatherApplication();
            var client = application.CreateClient();

            var postResponse = await client.PostAsJsonAsync(
                "/city",
                new City
                {
                    Id = 0,
                    Country = "France",
                    Name = "Paris",
                    EstablishedDate = new DateTime(0),
                    Rating = TouristRating.Good,
                    EstimatedPopulation = 2165423,
                    State = "Île-de-France"
                });
            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
            var cityCountryWeatherCollection = await client.GetFromJsonAsync<CityCountryWeather[]>("/searchByName/Paris");
            Assert.NotNull(cityCountryWeatherCollection);
            Assert.Single(cityCountryWeatherCollection);
            Assert.Equal("Paris", cityCountryWeatherCollection?[0].City.Name);
            Assert.Equal("France", cityCountryWeatherCollection?[0].City.Country);
            Assert.Equal(new DateTime(0), cityCountryWeatherCollection?[0].City.EstablishedDate);
            Assert.Equal(TouristRating.Good, cityCountryWeatherCollection?[0].City.Rating);
            Assert.Equal((uint)2165423, cityCountryWeatherCollection?[0].City.EstimatedPopulation);
            Assert.Equal("Île-de-France", cityCountryWeatherCollection?[0].City.State);
 
            var putResponse = await client.PutAsJsonAsync(
                $"/city/{cityCountryWeatherCollection?[0].City.Id}",
                new City
                {
                    EstablishedDate = new DateTime(0),
                    Rating = TouristRating.Meh,
                    EstimatedPopulation = 2165423
                });
            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);
            
            cityCountryWeatherCollection = await client.GetFromJsonAsync<CityCountryWeather[]>("/searchByName/Paris");
            Assert.NotNull(cityCountryWeatherCollection);
            Assert.Single(cityCountryWeatherCollection);
            Assert.Equal("Paris", cityCountryWeatherCollection?[0].City.Name);
            Assert.Equal("France", cityCountryWeatherCollection?[0].City.Country);
            Assert.Equal(new DateTime(0), cityCountryWeatherCollection?[0].City.EstablishedDate);
            Assert.Equal(TouristRating.Meh, cityCountryWeatherCollection?[0].City.Rating);
            Assert.Equal((uint)2165423, cityCountryWeatherCollection?[0].City.EstimatedPopulation);
            Assert.Equal("Île-de-France", cityCountryWeatherCollection?[0].City.State);
        }

        [Fact]
        public async Task TestDeleteCityEndpoint()
        {
            await using var application = new WeatherApplication();
            var client = application.CreateClient();

            var postResponse = await client.PostAsJsonAsync(
                "/city",
                new City
                {
                    Id = 0,
                    Country = "France",
                    Name = "Paris",
                    EstablishedDate = new DateTime(0),
                    Rating = TouristRating.Good,
                    EstimatedPopulation = 2165423,
                    State = "Île-de-France"
                });
            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
            var cityCountryWeatherCollection = await client.GetFromJsonAsync<CityCountryWeather[]>("/searchByName/Paris");
            Assert.NotNull(cityCountryWeatherCollection);
            Assert.Single(cityCountryWeatherCollection);
            Assert.Equal("Paris", cityCountryWeatherCollection?[0].City.Name);
            Assert.Equal("France", cityCountryWeatherCollection?[0].City.Country);
            Assert.Equal(new DateTime(0), cityCountryWeatherCollection?[0].City.EstablishedDate);
            Assert.Equal(TouristRating.Good, cityCountryWeatherCollection?[0].City.Rating);
            Assert.Equal((uint)2165423, cityCountryWeatherCollection?[0].City.EstimatedPopulation);
            Assert.Equal("Île-de-France", cityCountryWeatherCollection?[0].City.State);

            var deleteResponse = await client.DeleteAsync($"/city/{cityCountryWeatherCollection?[0].City.Id}");
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
            
            var getResponse = await client.GetAsync("/searchByName/Paris");
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [Fact]
        public async Task TestSearchByNameEndpoint()
        {
            await using var application = new WeatherApplication();
            var client = application.CreateClient();
            
            var getResult = await client.GetAsync("/searchByName/Paris");
            
            Assert.Equal(HttpStatusCode.NotFound, getResult.StatusCode);
        }
        
        [Fact]
        public async Task TestSearchByName()
        {
            await using var application = new WeatherApplication();
            var client = application.CreateClient();
                
            var postResponse = await client.PostAsJsonAsync(
                "/city",
                new City
                {
                    Id = 0,
                    Country = "France",
                    Name = "Paris",
                    EstablishedDate = new DateTime(0),
                    Rating = TouristRating.Good,
                    EstimatedPopulation = 2165423,
                    State = "Île-de-France"
                });
            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
            var cityCountryWeatherCollection = await client.GetFromJsonAsync<CityCountryWeather[]>("/searchByName/Paris");
            Assert.NotNull(cityCountryWeatherCollection);
            Assert.Single(cityCountryWeatherCollection);
            Assert.Equal("Paris", cityCountryWeatherCollection?[0].City.Name);
            Assert.Equal("France", cityCountryWeatherCollection?[0].City.Country);
            Assert.Equal(new DateTime(0), cityCountryWeatherCollection?[0].City.EstablishedDate);
            Assert.Equal(TouristRating.Good, cityCountryWeatherCollection?[0].City.Rating);
            Assert.Equal((uint)2165423, cityCountryWeatherCollection?[0].City.EstimatedPopulation);
            Assert.Equal("Île-de-France", cityCountryWeatherCollection?[0].City.State);
        }
    }
}