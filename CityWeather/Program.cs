using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        client.BaseAddress = new Uri("https://api.openweathermap.org");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
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

}).WithName("AddCity");

app.MapPut("/city", async (int id, City input, CityDb db) =>
{
    var city = await db.Cities.FindAsync(id);

    if (city is null) return Results.NotFound();

    city.Rating = input.Rating;
    city.EstablishedDate = input.EstablishedDate;
    city.EstimatedPopulation = input.EstimatedPopulation;

    await db.SaveChangesAsync();

    return Results.NoContent();
}).WithName("UpdateCity");

app.MapGet("/searchByName/{name}",
    async (
        string cityName,
        CityDb db,
        WeatherService weatherService,
        CountryInfoService countryInfoService) =>
{
    var cities = db.Cities.Where(city => string.Equals(city.Name, cityName, StringComparison.OrdinalIgnoreCase)).ToArray();
    if (!cities.Any())
    {
        return Results.NotFound();
    }

    var result = new List<CityCountryWeather>();
    foreach (var city in cities)
    {
        var weather = await weatherService.GetWeatherAsync(city);
        var countryInfo = await countryInfoService.GetCountryInfoAsync(cityName);
        result.Add(new CityCountryWeather
        {
            City = cities.First(),
            Country = countryInfo,
            Weather = weather
        });
    }

    return Results.Ok(result);

}).WithName("SearchCity");

app.MapDelete("/city/{id}", async (int id, CityDb db) =>
{
    if (await db.Cities.FindAsync(id) is City city)
    {
        db.Cities.Remove(city);
        await db.SaveChangesAsync();
        return Results.Ok(city);
    }

    return Results.NotFound();
}).WithName("DeleteCity");

app.Run();

internal class City
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public TouristRating Rating { get; set; }
    public DateTime EstablishedDate { get; set; }
    public UInt32 EstimatedPopulation { get; set; }
}

internal class CityCountryWeather
{
    public City City { get; set; }
    public Country Country { get; set; }
    public WeatherRoot Weather { get; set; }
}

internal enum TouristRating
{
    Bad = 1,
    Meh = 2,
    Good = 3,
    VeryGood = 4,
    Amazing = 5
}

class CityDb : DbContext
{
    public CityDb(DbContextOptions<CityDb> options)
        : base(options) { }

    public DbSet<City> Cities => Set<City>();
}

/// <summary>
/// Country codes according to ISO 3166
/// </summary>
public class Country
{
    public string Alpha2Code { get; set; }
    public string Alpha3Code { get; set; }
}



// References:
// https://docs.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio
// Swagger
// Postman
