using CityWeather.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace CityWeather.Tests;

internal class WeatherApplication : WebApplicationFactory<Program>
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
            services.AddScoped(sp => new DbContextOptionsBuilder<CityDb>()
                .UseInMemoryDatabase("Test")
                .UseApplicationServiceProvider(sp)
                .Options);
            services.AddHttpClient<WeatherService>();
            services.AddHttpClient<CountryInfoService>();
                
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var cityDb = scopedServices.GetRequiredService<CityDb>();
            // Ensure the database is deleted and re-created.
            cityDb.Database.EnsureDeleted();
            cityDb.Database.EnsureCreated();
        });

        return base.CreateHost(builder);
    }
}