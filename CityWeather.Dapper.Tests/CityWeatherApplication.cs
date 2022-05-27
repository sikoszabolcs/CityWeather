using CityWeather.Models.City;
using CityWeather.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Data.Sqlite;
using Dapper;

namespace CityWeather.Dapper.Tests;

internal class WeatherApplication : WebApplicationFactory<CityWeather.Dapper.Program>
{
    private readonly string _environment;

    public WeatherApplication(string environment = "Development")
    {
        _environment = environment;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment(_environment);

        // Add mock/test services to the builder here
        builder.ConfigureServices(services =>
        {
            services.AddScoped(sp =>
            {
                return new SqliteConnection("Data Source=TestCityWeather.db");
            });
            
            services.AddHttpClient<WeatherService>();
            services.AddHttpClient<CountryInfoService>();

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var cityDb = scopedServices.GetRequiredService<SqliteConnection>();
            cityDb.Execute("DROP TABLE IF EXISTS Cities");
            EnsureDb(cityDb);
        });

        return base.CreateHost(builder);
    }

    private void EnsureDb(SqliteConnection db)
    {
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
        db.Execute(sql);
    }
}