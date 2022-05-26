using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityWeather.Models.City;

public enum TouristRating
{
    Bad = 1,
    Meh = 2,
    Good = 3,
    VeryGood = 4,
    Amazing = 5
}

public class City
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