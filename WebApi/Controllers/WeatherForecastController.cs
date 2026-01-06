using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    /// <summary>
    /// Controller for weather forecast operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public partial class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherForecastService _weatherService;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(
            IWeatherForecastService weatherService,
            ILogger<WeatherForecastController> logger)
        {
            _weatherService = weatherService;
            _logger = logger;
        }

        [LoggerMessage(Level = LogLevel.Warning, Message = "Invalid argument provided: {Message}")]
        partial void LogInvalidArgument(string Message);

        [LoggerMessage(Level = LogLevel.Error, Message = "Error fetching forecast for days: {Days}")]
        partial void LogFetchError(Exception ex, int Days);

        [LoggerMessage(Level = LogLevel.Error, Message = "Error fetching current weather")]
        partial void LogCurrentWeatherError(Exception ex);

        [LoggerMessage(Level = LogLevel.Error, Message = "Error fetching forecast for specific date: {Date}")]
        partial void LogDateFetchError(Exception ex, DateOnly Date);

        [LoggerMessage(Level = LogLevel.Error, Message = "Error fetching forecast range")]
        partial void LogRangeFetchError(Exception ex);

        [LoggerMessage(Level = LogLevel.Error, Message = "Error calculating statistics")]
        partial void LogStatsError(Exception ex);

        /// <summary>
        /// Get weather forecast for multiple days
        /// </summary>
        /// <param name="days">Number of days to forecast (default: 5, max: 30)</param>
        /// <returns>List of weather forecasts</returns>
        /// <response code="200">Returns the weather forecast</response>
        /// <response code="400">If the days parameter is invalid</response>
        /// <response code="500">If there's an internal server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<WeatherForecast>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<WeatherForecast>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<List<WeatherForecast>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<List<WeatherForecast>>>> GetWeatherForecast([FromQuery] int days = 5)
        {
            try
            {
                if (days < 1 || days > 30)
                {
                    return BadRequest(ApiResponse<List<WeatherForecast>>.FailResponse(
                        "Invalid days parameter",
                        new List<string> { "Days must be between 1 and 30" }));
                }

                var forecasts = await _weatherService.GetForecastAsync(days);

                return Ok(ApiResponse<List<WeatherForecast>>.SuccessResponse(
                    forecasts,
                    $"Successfully retrieved {forecasts.Count} day forecast"));
            }
            catch (ArgumentException argEx)
            {
                LogInvalidArgument(argEx.Message);
                return BadRequest(ApiResponse<List<WeatherForecast>>.FailResponse(
                    "Invalid request",
                    new List<string> { argEx.Message }));
            }
            catch (Exception ex)
            {
                LogFetchError(ex, days);
                return StatusCode(500, ApiResponse<List<WeatherForecast>>.FailResponse(
                    "An error occurred while fetching weather forecast",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get current weather
        /// </summary>
        /// <returns>Current weather forecast</returns>
        /// <response code="200">Returns the current weather</response>
        /// <response code="500">If there's an internal server error</response>
        [HttpGet("current")]
        [ProducesResponseType(typeof(ApiResponse<WeatherForecast>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<WeatherForecast>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<WeatherForecast>>> GetCurrentWeather()
        {
            try
            {
                var weather = await _weatherService.GetCurrentWeatherAsync();

                return Ok(ApiResponse<WeatherForecast>.SuccessResponse(
                    weather,
                    "Successfully retrieved current weather"));
            }
            catch (Exception ex)
            {
                LogCurrentWeatherError(ex);
                return StatusCode(500, ApiResponse<WeatherForecast>.FailResponse(
                    "An error occurred while fetching current weather",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get weather forecast for a specific date
        /// </summary>
        /// <param name="date">Date in format YYYY-MM-DD</param>
        /// <returns>Weather forecast for the specified date</returns>
        /// <response code="200">Returns the weather forecast for the date</response>
        /// <response code="404">If the date is out of forecast range</response>
        /// <response code="500">If there's an internal server error</response>
        [HttpGet("date/{date}")]
        [ProducesResponseType(typeof(ApiResponse<WeatherForecast>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<WeatherForecast>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<WeatherForecast>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<WeatherForecast>>> GetForecastByDate(DateOnly date)
        {
            try
            {
                var forecast = await _weatherService.GetForecastByDateAsync(date);

                if (forecast == null)
                {
                    return NotFound(ApiResponse<WeatherForecast>.FailResponse(
                        "Date not found",
                        new List<string> { "The requested date is out of forecast range (next 30 days only)" }));
                }

                return Ok(ApiResponse<WeatherForecast>.SuccessResponse(
                    forecast,
                    $"Successfully retrieved forecast for {date}"));
            }
            catch (Exception ex)
            {
                LogDateFetchError(ex, date);
                return StatusCode(500, ApiResponse<WeatherForecast>.FailResponse(
                    "An error occurred while fetching weather forecast",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get weather forecast for a date range
        /// </summary>
        /// <param name="startDate">Start date in format YYYY-MM-DD</param>
        /// <param name="endDate">End date in format YYYY-MM-DD</param>
        /// <returns>List of weather forecasts within the date range</returns>
        /// <response code="200">Returns the weather forecast for the date range</response>
        /// <response code="400">If the date range is invalid</response>
        /// <response code="500">If there's an internal server error</response>
        [HttpGet("range")]
        [ProducesResponseType(typeof(ApiResponse<List<WeatherForecast>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<WeatherForecast>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<List<WeatherForecast>>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<List<WeatherForecast>>>> GetForecastRange(
            [FromQuery] DateOnly startDate,
            [FromQuery] DateOnly endDate)
        {
            try
            {
                var forecasts = await _weatherService.GetForecastRangeAsync(startDate, endDate);

                return Ok(ApiResponse<List<WeatherForecast>>.SuccessResponse(
                    forecasts,
                    $"Successfully retrieved forecast from {startDate} to {endDate}"));
            }
            catch (ArgumentException argEx)
            {
                LogInvalidArgument(argEx.Message);
                return BadRequest(ApiResponse<List<WeatherForecast>>.FailResponse(
                    "Invalid date range",
                    new List<string> { argEx.Message }));
            }
            catch (Exception ex)
            {
                LogRangeFetchError(ex);
                return StatusCode(500, ApiResponse<List<WeatherForecast>>.FailResponse(
                    "An error occurred while fetching weather forecast",
                    new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get weather statistics summary
        /// </summary>
        /// <param name="days">Number of days to analyze (default: 7, max: 30)</param>
        /// <returns>Weather statistics including average temperature, min, max, etc.</returns>
        /// <response code="200">Returns the weather statistics</response>
        /// <response code="400">If the days parameter is invalid</response>
        /// <response code="500">If there's an internal server error</response>
        [HttpGet("statistics")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> GetWeatherStatistics([FromQuery] int days = 7)
        {
            try
            {
                if (days < 1 || days > 30)
                {
                    return BadRequest(ApiResponse<object>.FailResponse(
                        "Invalid days parameter",
                        new List<string> { "Days must be between 1 and 30" }));
                }

                var forecasts = await _weatherService.GetForecastAsync(days);

                var statistics = new
                {
                    PeriodStart = forecasts.First().Date,
                    PeriodEnd = forecasts.Last().Date,
                    TotalDays = forecasts.Count,
                    AverageTemperatureC = Math.Round(forecasts.Average(f => f.TemperatureC), 2),
                    AverageTemperatureF = Math.Round(forecasts.Average(f => f.TemperatureF), 2),
                    MinTemperatureC = forecasts.Min(f => f.TemperatureC),
                    MaxTemperatureC = forecasts.Max(f => f.TemperatureC),
                    AverageHumidity = forecasts.Average(f => f.Humidity),
                    AverageWindSpeed = Math.Round(forecasts.Average(f => f.WindSpeed ?? 0), 2),
                    MostCommonCondition = forecasts.GroupBy(f => f.Condition)
                        .OrderByDescending(g => g.Count())
                        .First().Key.ToString()
                };

                return Ok(ApiResponse<object>.SuccessResponse(
                    statistics,
                    $"Successfully retrieved statistics for {days} days"));
            }
            catch (Exception ex)
            {
                LogStatsError(ex);
                return StatusCode(500, ApiResponse<object>.FailResponse(
                    "An error occurred while calculating weather statistics",
                    new List<string> { ex.Message }));
            }
        }
    }
}