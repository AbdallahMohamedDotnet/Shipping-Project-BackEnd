using BL.Contract;
using BL.Dtos;
using DAL.Exceptions;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingPackgingController : ControllerBase
    {
        IPackgingTypes _packgingTypes;
        public ShippingPackgingController(IPackgingTypes packgingTypes)
        {
            _packgingTypes = packgingTypes;
        }
        
        // GET: api/<ShippingPackgingController>
        [HttpGet]
        public ActionResult<ApiResponse<List<DTOShipingPackging>>> Get()
        {
            try
            {
                var data = _packgingTypes.GetAll();
                return Ok(ApiResponse<List<DTOShipingPackging>>.SuccessResponse(data));
            }
            catch(DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<List<DTOShipingPackging>>.FailResponse
                    ("data access exception", new List<string>() { daEx.Message }));
            }
            catch(Exception ex)
            {
                return StatusCode(500, ApiResponse<List<DTOShipingPackging>>.FailResponse
                    ("general exception", new List<string>() { ex.Message }));
            }
        }

        // GET api/<ShippingPackgingController>/5
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<DTOShipingPackging>> Get(Guid id)
        {
            try
            {
                var data = _packgingTypes.GetById(id);
                return Ok(ApiResponse<DTOShipingPackging>.SuccessResponse(data));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<DTOShipingPackging>.FailResponse
                    ("data access exception", new List<string>() { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<DTOShipingPackging>.FailResponse
                    ("general exception", new List<string>() { ex.Message }));
            }
        }

        // POST api/<ShippingPackgingController>
        [HttpPost]
        public ActionResult<ApiResponse<bool>> Post([FromBody] DTOShipingPackging packging)
        {
            try
            {
                if (packging == null)
                {
                    return BadRequest(ApiResponse<bool>.FailResponse("Packaging data is required"));
                }

                var result = _packgingTypes.Add(packging);
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Packaging added successfully"));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<bool>.FailResponse
                    ("data access exception", new List<string>() { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.FailResponse
                    ("general exception", new List<string>() { ex.Message }));
            }
        }

        // PUT api/<ShippingPackgingController>/5
        [HttpPut("{id}")]
        public ActionResult<ApiResponse<bool>> Put(Guid id, [FromBody] DTOShipingPackging packging)
        {
            try
            {
                if (packging == null || packging.Id != id)
                {
                    return BadRequest(ApiResponse<bool>.FailResponse("Invalid packaging data"));
                }

                var result = _packgingTypes.Update(packging);
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Packaging updated successfully"));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<bool>.FailResponse
                    ("data access exception", new List<string>() { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.FailResponse
                    ("general exception", new List<string>() { ex.Message }));
            }
        }

        // DELETE api/<ShippingPackgingController>/5
        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<bool>> Delete(Guid id)
        {
            try
            {
                var result = _packgingTypes.ChangeStatus(id, 0); // Soft delete by changing status to 0
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Packaging deleted successfully"));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<bool>.FailResponse
                    ("data access exception", new List<string>() { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.FailResponse
                    ("general exception", new List<string>() { ex.Message }));
            }
        }

        // PUT api/<ShippingPackgingController>/5/status
        [HttpPut("{id}/status")]
        public ActionResult<ApiResponse<bool>> ChangeStatus(Guid id, [FromBody] object statusObj)
        {
            try
            {
                // Extract status from the request body
                var statusProperty = statusObj.GetType().GetProperty("status");
                if (statusProperty == null)
                {
                    return BadRequest(ApiResponse<bool>.FailResponse("Status property is required"));
                }

                var status = (int)statusProperty.GetValue(statusObj);
                var result = _packgingTypes.ChangeStatus(id, status);
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Packaging status changed successfully"));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<bool>.FailResponse
                    ("data access exception", new List<string>() { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.FailResponse
                    ("general exception", new List<string>() { ex.Message }));
            }
        }
    }
}
