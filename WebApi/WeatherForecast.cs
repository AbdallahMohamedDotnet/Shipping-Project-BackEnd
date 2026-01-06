using System.ComponentModel.DataAnnotations;

namespace WebApi
{
    /// <summary>
    /// Represents a weather forecast for a specific date
    /// </summary>
    public class WeatherForecast
    {
        /// <summary>
        /// The date of the forecast
        /// </summary>
        [Required]
        public DateOnly Date { get; set; }

        /// <summary>
        /// Temperature in Celsius
        /// </summary>
        [Range(-100, 100, ErrorMessage = "Temperature must be between -100 and 100 degrees Celsius")]
        public int TemperatureC { get; set; }

        /// <summary>
        /// Temperature in Fahrenheit (calculated from Celsius)
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC * 1.8);

        /// <summary>
        /// Weather condition summary
        /// </summary>
        [Required(ErrorMessage = "Summary is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Summary must be between 3 and 100 characters")]
        public string Summary { get; set; } = string.Empty;

        /// <summary>
        /// Weather condition code for categorization
        /// </summary>
        public WeatherCondition Condition { get; set; }

        /// <summary>
        /// Humidity percentage (0-100)
        /// </summary>
        [Range(0, 100, ErrorMessage = "Humidity must be between 0 and 100 percent")]
        public int? Humidity { get; set; }

        /// <summary>
        /// Wind speed in km/h
        /// </summary>
        [Range(0, 500, ErrorMessage = "Wind speed must be between 0 and 500 km/h")]
        public double? WindSpeed { get; set; }
    }

    /// <summary>
    /// Weather condition categories
    /// </summary>
    public enum WeatherCondition
    {
        Freezing = 0,
        Cold = 1,
        Cool = 2,
        Mild = 3,
        Warm = 4,
        Hot = 5,
        VeryHot = 6
    }
}
