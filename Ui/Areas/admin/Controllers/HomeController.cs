using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ui.Services;
using Ui.Models;

namespace Ui.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly GenericApiClient _apiClient;

        public HomeController(GenericApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetWeatherData()
        {
            try
            {
                var response = await _apiClient.GetAsync<dynamic>("api/WeatherForecast");
                
                // Check if the response is wrapped in ApiResponse
                if (response?.data != null)
                {
                    return Json(new { success = true, data = response.data });
                }
                else if (response?.Data != null)
                {
                    return Json(new { success = true, data = response.Data });
                }
                else
                {
                    // If response is directly the data array
                    return Json(new { success = true, data = response });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error fetching weather data: {ex.Message}" });
            }
        }
    }
}
