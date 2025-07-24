using BL.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShippingController : ControllerBase
    {
        private readonly ICity _cityService;
        private readonly IShippingType _shippingTypeService;
        private readonly ILogger<ShippingController> _logger;

        public ShippingController(
            ICity cityService, 
            IShippingType shippingTypeService, 
            ILogger<ShippingController> logger)
        {
            _cityService = cityService;
            _shippingTypeService = shippingTypeService;
            _logger = logger;
        }

        [HttpGet("cities")]
        public IActionResult GetCities()
        {
            try
            {
                var cities = _cityService.GetAll();
                return Ok(cities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cities");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("shipping-types")]
        public IActionResult GetShippingTypes()
        {
            try
            {
                var shippingTypes = _shippingTypeService.GetAll();
                return Ok(shippingTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving shipping types");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new { Status = "Healthy", Message = "API services are properly registered and running" });
        }
    }
}