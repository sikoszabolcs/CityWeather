using CityWeather.Models;
using CityWeather.Models.City;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace CityWeather.Dapper.Tests
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
                    Country = "France",
                    Name = "Paris",
                    EstablishedDate = "250 B.C.",
                    Rating = TouristRating.Good,
                    EstimatedPopulation = 2165423,
                    State = "�le-de-France"
                });

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var cityCountryWeatherCollection = await client.GetFromJsonAsync<CityCountryWeather[]>("/searchByName/Paris");
            Assert.NotNull(cityCountryWeatherCollection);
            Assert.Single(cityCountryWeatherCollection);
            Assert.Equal("Paris", cityCountryWeatherCollection?[0].City.Name);
            Assert.Equal("France", cityCountryWeatherCollection?[0].City.Country);
            Assert.Equal("250 B.C.", cityCountryWeatherCollection?[0].City.EstablishedDate);
            Assert.Equal(TouristRating.Good, cityCountryWeatherCollection?[0].City.Rating);
            Assert.Equal((uint)2165423, cityCountryWeatherCollection?[0].City.EstimatedPopulation);
            Assert.Equal("�le-de-France", cityCountryWeatherCollection?[0].City.State);
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
                    EstablishedDate = "250 B.C.",
                    Rating = TouristRating.Good,
                    EstimatedPopulation = 2165423,
                    State = "�le-de-France"
                });
            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
            var cityCountryWeatherCollection = await client.GetFromJsonAsync<CityCountryWeather[]>("/searchByName/Paris");
            Assert.NotNull(cityCountryWeatherCollection);
            Assert.Single(cityCountryWeatherCollection);
            Assert.Equal("Paris", cityCountryWeatherCollection?[0].City.Name);
            Assert.Equal("France", cityCountryWeatherCollection?[0].City.Country);
            Assert.Equal("250 B.C.", cityCountryWeatherCollection?[0].City.EstablishedDate);
            Assert.Equal(TouristRating.Good, cityCountryWeatherCollection?[0].City.Rating);
            Assert.Equal((uint)2165423, cityCountryWeatherCollection?[0].City.EstimatedPopulation);
            Assert.Equal("�le-de-France", cityCountryWeatherCollection?[0].City.State);

            var putResponse = await client.PutAsJsonAsync(
                $"/city/{cityCountryWeatherCollection?[0].City.Id}",
                new City
                {
                    EstablishedDate = "250 B.C.",
                    Rating = TouristRating.Meh,
                    EstimatedPopulation = 2165423
                });
            Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode);

            cityCountryWeatherCollection = await client.GetFromJsonAsync<CityCountryWeather[]>("/searchByName/Paris");
            Assert.NotNull(cityCountryWeatherCollection);
            Assert.Single(cityCountryWeatherCollection);
            Assert.Equal("Paris", cityCountryWeatherCollection?[0].City.Name);
            Assert.Equal("France", cityCountryWeatherCollection?[0].City.Country);
            Assert.Equal("250 B.C.", cityCountryWeatherCollection?[0].City.EstablishedDate);
            Assert.Equal(TouristRating.Meh, cityCountryWeatherCollection?[0].City.Rating);
            Assert.Equal((uint)2165423, cityCountryWeatherCollection?[0].City.EstimatedPopulation);
            Assert.Equal("�le-de-France", cityCountryWeatherCollection?[0].City.State);
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
                    EstablishedDate = "250 B.C.",
                    Rating = TouristRating.Good,
                    EstimatedPopulation = 2165423,
                    State = "�le-de-France"
                });
            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
            var cityCountryWeatherCollection = await client.GetFromJsonAsync<CityCountryWeather[]>("/searchByName/Paris");
            Assert.NotNull(cityCountryWeatherCollection);
            Assert.Single(cityCountryWeatherCollection);
            Assert.Equal("Paris", cityCountryWeatherCollection?[0].City.Name);
            Assert.Equal("France", cityCountryWeatherCollection?[0].City.Country);
            Assert.Equal("250 B.C.", cityCountryWeatherCollection?[0].City.EstablishedDate);
            Assert.Equal(TouristRating.Good, cityCountryWeatherCollection?[0].City.Rating);
            Assert.Equal((uint)2165423, cityCountryWeatherCollection?[0].City.EstimatedPopulation);
            Assert.Equal("�le-de-France", cityCountryWeatherCollection?[0].City.State);

            var deleteResponse = await client.DeleteAsync($"/city/{cityCountryWeatherCollection?[0].City.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

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
                    EstablishedDate = "250 B.C.",
                    Rating = TouristRating.Good,
                    EstimatedPopulation = 2165423,
                    State = "�le-de-France"
                });
            Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
            var cityCountryWeatherCollection = await client.GetFromJsonAsync<CityCountryWeather[]>("/searchByName/Paris");
            Assert.NotNull(cityCountryWeatherCollection);
            Assert.Single(cityCountryWeatherCollection);
            Assert.Equal("Paris", cityCountryWeatherCollection?[0].City.Name);
            Assert.Equal("France", cityCountryWeatherCollection?[0].City.Country);
            Assert.Equal("250 B.C.", cityCountryWeatherCollection?[0].City.EstablishedDate);
            Assert.Equal(TouristRating.Good, cityCountryWeatherCollection?[0].City.Rating);
            Assert.Equal((uint)2165423, cityCountryWeatherCollection?[0].City.EstimatedPopulation);
            Assert.Equal("�le-de-France", cityCountryWeatherCollection?[0].City.State);
        }
    }
}