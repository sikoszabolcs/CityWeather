using CityWeather.Models;
using CityWeather.Models.City;
using CityWeather.Services;
using Dapper;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("CityDb") ?? "Data Source=CityWeather.db;Cache=Shared";
builder.Services.AddScoped(_ => new SqliteConnection(connectionString));
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

await EnsureDb(app.Services, app.Logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/error", () => Results.Problem("An error occurred.", statusCode: 500))
   .ExcludeFromDescription();

app.MapPost("/city", async (City city, SqliteConnection db) =>
{
    var ratingAsInt = Convert.ToUInt32(city.Rating);
    
    try
    {
        var newCity = await db.QuerySingleAsync<City>(
            "INSERT INTO Cities(Name, State, Country, Rating, EstablishedDate, EstimatedPopulation) Values(@name, @state, @country, @rating, @establishedDate, @estimatedPopulation) RETURNING * ",
            new { name = city.Name, state = city.State, country = city.Country, rating = ratingAsInt, establishedDate = city.EstablishedDate, estimatedPopulation = city.EstimatedPopulation });
        
        return Results.Created($"/city/{city.Id}", newCity);
    }
    catch (SqliteException e)
    {
        return Results.UnprocessableEntity(e.Message);
    }
})
    .WithName("AddCity")
    .Produces(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status422UnprocessableEntity);

app.MapPut("/city/{id:int}", async (int id, City city, SqliteConnection db) =>
{
    city.Id = id;
    return await db.ExecuteAsync("UPDATE Cities SET Rating = @Rating, EstablishedDate = @EstablishedDate, EstimatedPopulation = @EstimatedPopulation WHERE Id = @Id", city) == 1
        ? Results.NoContent()
        : Results.NotFound();
})
    .WithName("UpdateCity")
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound);

app.MapGet("/searchByName/{name}",
    async (
        string name,
        SqliteConnection db,
        WeatherService weatherService,
        CountryInfoService countryInfoService) =>
    {
        var cities = (await db.QueryAsync<City>("SELECT * FROM Cities WHERE Name = @name", new { name })).ToArray();
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

app.MapDelete("/city/{id:int}", async (int id, SqliteConnection db) =>
    await db.ExecuteAsync("DELETE FROM Cities WHERE Id = @id", new { id }) == 1
        ? Results.NoContent()
        : Results.NotFound())
    .WithName("DeleteCity")
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status204NoContent);

app.Run();

async Task EnsureDb(IServiceProvider services, ILogger logger)
{
    logger.LogInformation("Ensuring database exists at connection string '{connectionString}'", connectionString);

    await using var db = services.CreateScope().ServiceProvider.GetRequiredService<SqliteConnection>();
    var sql =
        $@"CREATE TABLE IF NOT EXISTS Cities (
            {nameof(City.Id)} INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
            {nameof(City.Name)} TEXT NOT NULL,
            {nameof(City.State)} TEXT,
            {nameof(City.Country)} TEXT NOT NULL,
            {nameof(City.Rating)} INTEGER DEFAULT 0 NOT NULL CHECK({nameof(City.Rating)} IN (0, 1, 2, 3, 4, 5)),
            {nameof(City.EstablishedDate)} TEXT NOT NULL,
            {nameof(City.EstimatedPopulation)} INTEGER DEFAULT 0 NOT NULL,
            UNIQUE ({nameof(City.Name)}, {nameof(City.State)}, {nameof(City.Country)})
    );";
    
    await db.ExecuteAsync(sql);
}

namespace CityWeather.Dapper
{
    // We need to expose the implicitly defined program class, so that the test project can target it
    // See https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0#sut-environment
    public partial class Program { }
}