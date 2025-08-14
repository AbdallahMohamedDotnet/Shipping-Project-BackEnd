using BL.Contracts;
using BL.DTOConfiguration;
using DAL.Exceptions;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingTypesController : ControllerBase
    {
        IShippingType _shippingTypes;
        public ShippingTypesController(IShippingType shippingTypes)
        {
            _shippingTypes = shippingTypes;
        }
        // GET: api/<ShippingTypesController>
        [HttpGet]
        public ActionResult<ApiResponse<List<DTOShippingType>>> Get()
        {
            try
            {
                // Fetch all shipping types from the service
                var data = _shippingTypes.GetAll();
                // Check if data is null or empty and return an appropriate response if sussessful or failure 
                return Ok(ApiResponse<List<DTOShippingType>>.SuccessResponse(data));
            }
            catch (DataAccessException daEx)
            {
                // Log the exception and return a 500 status code with a failure response
                return StatusCode(500, ApiResponse<List<DTOShippingType>>.FailResponse
                    ("data access exception", new List<string>() { daEx.Message }));
            }
            catch (Exception ex)
            {
                // Log the exception and return a 500 status code with a failure response
                return StatusCode(500, ApiResponse<List<DTOShippingType>>.FailResponse
                    ("general exception", new List<string>() { ex.Message }));
            }

        }

        // GET api/<ShippingTypesController>/5
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<DTOShippingType>> Get(Guid id)
        {
            try
            {
                var data = _shippingTypes.GetById(id);

                return Ok(ApiResponse<DTOShippingType>.SuccessResponse(data));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<DTOShippingType>.FailResponse
                    ("data access exception", new List<string>() { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<DTOShippingType>.FailResponse
                    ("general exception", new List<string>() { ex.Message }));
            }
        }
    }
}
