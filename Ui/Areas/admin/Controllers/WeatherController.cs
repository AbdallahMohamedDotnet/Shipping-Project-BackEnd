using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ui.Services;
using Ui.Models;

namespace Ui.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class WeatherController : Controller
    {
        private readonly GenericApiClient _apiClient;
        private readonly ILogger<WeatherController> _logger;

        public WeatherController(GenericApiClient apiClient, ILogger<WeatherController> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _apiClient.GetAsync<dynamic>("api/WeatherForecast");
                
                List<WeatherForecast> weatherData;
                
                // Handle ApiResponse wrapper
                if (response?.data != null)
                {
                    weatherData = System.Text.Json.JsonSerializer.Deserialize<List<WeatherForecast>>(
                        response.data.ToString(),
                        new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else if (response?.Data != null)
                {
                    weatherData = System.Text.Json.JsonSerializer.Deserialize<List<WeatherForecast>>(
                        response.Data.ToString(),
                        new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    weatherData = System.Text.Json.JsonSerializer.Deserialize<List<WeatherForecast>>(
                        response.ToString(),
                        new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                return View(weatherData ?? new List<WeatherForecast>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching weather data");
                ViewBag.ErrorMessage = $"Error fetching weather data: {ex.Message}";
                return View(new List<WeatherForecast>());
            }
        }
    }
}