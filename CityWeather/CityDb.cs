using CityWeather.Models.City;
using Microsoft.EntityFrameworkCore;

namespace CityWeather;

public class CityDb : DbContext
{
    public CityDb(DbContextOptions<CityDb> options)
        : base(options) { }

    public DbSet<City> Cities => Set<City>();
}