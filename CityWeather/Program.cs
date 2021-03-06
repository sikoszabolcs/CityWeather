using CityWeather;
using CityWeather.Models;
using CityWeather.Models.City;
using CityWeather.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CityDb>(opt => opt.UseInMemoryDatabase("CityDb"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddHttpClient<WeatherService>(
    client =>
    {
        const string weatherApiEndpointUri = "https://api.openweathermap.org";
        client.BaseAddress = new Uri(weatherApiEndpointUri);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
    });
builder.Services.AddHttpClient<CountryInfoService>(client =>
{
    const string countriesApiEndpointUri = "https://restcountries.com";
    client.BaseAddress = new Uri(countriesApiEndpointUri);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/city", async (City city, CityDb db) =>
{
    db.Cities.Add(city);
    await db.SaveChangesAsync();

    return Results.Created($"/city/{city.Id}", city);

})
    .WithName("AddCity")
    .Produces(StatusCodes.Status201Created);

app.MapPut("/city/{id:int}", async (int id, City input, CityDb db) =>
{
    var city = await db.Cities.FindAsync(id);

    if (city is null) return Results.NotFound();

    city.Rating = input.Rating;
    city.EstablishedDate = input.EstablishedDate;
    city.EstimatedPopulation = input.EstimatedPopulation;

    await db.SaveChangesAsync();

    return Results.Ok();
})
    .WithName("UpdateCity")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

app.MapGet("/searchByName/{name}",
    async (
        string name,
        CityDb db,
        WeatherService weatherService,
        CountryInfoService countryInfoService) =>
{
    var cities = db.Cities.Where(city => string.Equals(city.Name, name, StringComparison.OrdinalIgnoreCase)).ToArray();
    if (!cities.Any())
    {
        return Results.NotFound();
    }

    var result = new List<CityCountryWeather>();
    foreach (var city in cities)
    {
        var countryInfo = await countryInfoService.GetCountryInfoAsync(city.Country);
        var weather = await weatherService.GetWeatherAsync(city.Name, countryInfo?.Alpha2Code ?? string.Empty);
        result.Add(new CityCountryWeather
        {
            City = city,
            Country = countryInfo!,
            Weather = weather!
        });
    }

    return Results.Ok(result);

})
    .WithName("SearchCity")
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status200OK);

app.MapDelete("/city/{id:int}", async (int id, CityDb db) =>
{
    if (await db.Cities.FindAsync(id) is City city)
    {
        db.Cities.Remove(city);
        await db.SaveChangesAsync();
        return Results.Ok(city);
    }

    return Results.NotFound();
})
    .WithName("DeleteCity")
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status200OK);

app.Run();

// We need to expose the implicitly defined program class, so that the test project can target it
// See https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0#sut-environment
public partial class Program { }
