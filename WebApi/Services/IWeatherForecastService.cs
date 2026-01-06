namespace WebApi.Services
{
    /// <summary>
    /// Service interface for weather forecast operations
    /// </summary>
    public interface IWeatherForecastService
    {
        /// <summary>
        /// Get weather forecast for the specified number of days
        /// </summary>
        /// <param name="days">Number of days to forecast (1-30)</param>
        /// <returns>List of weather forecasts</returns>
        Task<List<WeatherForecast>> GetForecastAsync(int days = 5);

        /// <summary>
        /// Get weather forecast for a specific date
        /// </summary>
        /// <param name="date">The date to get forecast for</param>
        /// <returns>Weather forecast for the specified date, or null if not found</returns>
        Task<WeatherForecast?> GetForecastByDateAsync(DateOnly date);

        /// <summary>
        /// Get weather forecast within a date range
        /// </summary>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of weather forecasts within the date range</returns>
        Task<List<WeatherForecast>> GetForecastRangeAsync(DateOnly startDate, DateOnly endDate);

        /// <summary>
        /// Get current weather
        /// </summary>
        /// <returns>Current weather forecast</returns>
        Task<WeatherForecast> GetCurrentWeatherAsync();
    }
}
