using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            
            var error = new
            {
                Message = context?.Error?.Message ?? "An error occurred",
                Type = context?.Error?.GetType().Name,
                StackTrace = context?.Error?.StackTrace
            };

            return Problem(
                detail: error.Message,
                title: "An error occurred while processing your request.",
                type: error.Type);
        }
    }
}