using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityWeather.Models.City;

public enum TouristRating
{
    None = 0, // Initial value to signal the absence of a rating
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
    [Required] public string Name { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    [Required]
    public string Country { get; set; } = string.Empty;
    public TouristRating Rating { get; set; } = TouristRating.None;
    public string EstablishedDate { get; set; } = string.Empty;
    public UInt32 EstimatedPopulation { get; set; }
}