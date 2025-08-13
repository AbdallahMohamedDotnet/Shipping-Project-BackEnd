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

        public WeatherController(GenericApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var weatherData = await _apiClient.GetAsync<List<WeatherForecast>>("api/WeatherForecast");
                return View(weatherData);
            }
            catch (Exception ex)
            {
                // Handle error
                ViewBag.ErrorMessage = $"Error fetching weather data: {ex.Message}";
                return View(new List<WeatherForecast>());
            }
        }
    }
}