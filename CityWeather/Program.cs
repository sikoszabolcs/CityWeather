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

app.MapPut("/city/{id:int}", async (int id, City input, CityDb db) =>
{
    var city = await db.Cities.FindAsync(id);

    if (city is null) return Results.NotFound();

    city.Rating = input.Rating;
    city.EstablishedDate = input.EstablishedDate;
    city.EstimatedPopulation = input.EstimatedPopulation;

    await db.SaveChangesAsync();

    return Results.Ok();
}).WithName("UpdateCity");

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
        //var weather = await weatherService.GetWeatherAsync(city);
        //var countryInfo = await countryInfoService.GetCountryInfoAsync(name);
        result.Add(new CityCountryWeather
        {
            City = city,
            Country = null,
            Weather = null
        });
    }

    return Results.Ok(result);

}).WithName("SearchCity");

app.MapDelete("/city/{id:int}", async (int id, CityDb db) =>
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


// We need to expose the implicitly defined program class, so that the test project can target it
// See https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0#sut-environment
public partial class Program { }
