using BL.Contracts;
using BL.DTOConfiguration;
using DAL.Exceptions;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        ICity _City;
        public CityController(ICity City)
        {
            _City = City;
        }
        // GET: api/<ShippingTypesController>
        [HttpGet]
        public ActionResult<ApiResponse<List<DTOCity>>> Get()
        {
            try
            {
                // Fetch all shipping types from the service
                var data = _City.GetAll();
                // Check if data is null or empty and return an appropriate response if sussessful or failure 
                return Ok(ApiResponse<List<DTOCity>>.SuccessResponse(data));
            }
            catch (DataAccessException daEx)
            {
                // Log the exception and return a 500 status code with a failure response
                return StatusCode(500, ApiResponse<List<DTOCity>>.FailResponse
                    ("data access exception", new List<string>() { daEx.Message }));
            }
            catch (Exception ex)
            {
                // Log the exception and return a 500 status code with a failure response
                return StatusCode(500, ApiResponse<List<DTOCity>>.FailResponse
                    ("general exception", new List<string>() { ex.Message }));
            }

        }

        // GET api/<ShippingTypesController>/5
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<DTOCity>> Get(Guid id)
        {
            try
            {
                var data = _City.GetById(id);

                return Ok(ApiResponse<DTOCity>.SuccessResponse(data));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<DTOCity>.FailResponse
                    ("data access exception", new List<string>() { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<DTOCity>.FailResponse
                    ("general exception", new List<string>() { ex.Message }));
            }
        }

        // GET api/<ShippingTypesController>/5
        [HttpGet("country/{countryId}")]
        public ActionResult<ApiResponse<DTOCity>> GetByCountry(Guid id)
        {
            try
            {
                var data = _City.GetByCountry(id);

                return Ok(ApiResponse<List<DTOCity>>.SuccessResponse(data));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<DTOCity>.FailResponse
                    ("data access exception", new List<string>() { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<DTOCity>.FailResponse
                    ("general exception", new List<string>() { ex.Message }));
            }
        }
    }
}

