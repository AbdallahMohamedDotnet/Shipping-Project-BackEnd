using BL.Contracts;
using BL.DTOConfiguration;
using DAL.Exceptions;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        ICountry _Country;
        public CountryController(ICountry Country)
        {
            _Country = Country;
        }
        // GET: api/<ShippingTypesController>
        [HttpGet]
        public ActionResult<ApiResponse<List<DTOCountry>>> Get()
        {
            try
            {
                // Fetch all shipping types from the service
                var data = _Country.GetAll();
                // Check if data is null or empty and return an appropriate response if sussessful or failure 
                return Ok(ApiResponse<List<DTOCountry>>.SuccessResponse(data));
            }
            catch (DataAccessException daEx)
            {
                // Log the exception and return a 500 status code with a failure response
                return StatusCode(500, ApiResponse<List<DTOCountry>>.FailResponse
                    ("data access exception", new List<string>() { daEx.Message }));
            }
            catch (Exception ex)
            {
                // Log the exception and return a 500 status code with a failure response
                return StatusCode(500, ApiResponse<List<DTOCountry>>.FailResponse
                    ("general exception", new List<string>() { ex.Message }));
            }

        }

        // GET api/<ShippingTypesController>/5
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<DTOCountry>> Get(Guid id)
        {
            try
            {
                var data = _Country.GetById(id);

                return Ok(ApiResponse<DTOCountry>.SuccessResponse(data));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<DTOCountry>.FailResponse
                    ("data access exception", new List<string>() { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<DTOCountry>.FailResponse
                    ("general exception", new List<string>() { ex.Message }));
            }
        }
    }
}
