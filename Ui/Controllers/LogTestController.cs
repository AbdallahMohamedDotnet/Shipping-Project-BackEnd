using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ui.Logging;

namespace Ui.Controllers
{
    /// <summary>
    /// Controller for testing the advanced logging implementation
    /// Restricted to administrators only
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class LogTestController : Controller
    {
        private readonly ILogger<LogTestController> _logger;

        public LogTestController(ILogger<LogTestController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Test page to verify logging functionality
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Test various log levels and structured logging
        /// </summary>
        [HttpPost]
        public IActionResult TestLogging(string logType)
        {
            var userName = User?.Identity?.Name ?? "TestUser";

            switch (logType)
            {
                case "info":
                    LoggerMessageDefinitions.UserLoggedIn(_logger, userName);
                    TempData["Message"] = "✅ Information log created";
                    break;

                case "warning":
                    LoggerMessageDefinitions.AccessDenied(_logger, userName, "/Admin/SecretPage");
                    TempData["Message"] = "⚠️ Warning log created";
                    break;

                case "error":
                    try
                    {
                        throw new InvalidOperationException("This is a test exception for logging");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Test error log with exception");
                        TempData["Message"] = "❌ Error log created with exception";
                    }
                    break;

                case "performance":
                    LoggerMessageDefinitions.SlowOperationDetected(_logger, "TestOperation", 5500, 3000);
                    TempData["Message"] = "🐢 Performance warning log created";
                    break;

                case "shipment":
                    LoggerMessageDefinitions.ShipmentCreated(_logger, "SHIP-TEST-12345", "USER-001", "USER-002");
                    TempData["Message"] = "📦 Shipment log created";
                    break;

                case "exception":
                    // This will be caught by GlobalExceptionHandlerMiddleware
                    throw new InvalidOperationException("Unhandled exception test - will be caught by middleware");

                default:
                    TempData["Message"] = "❓ Unknown log type";
                    break;
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Test API endpoint that throws exception (for JSON error response)
        /// </summary>
        [HttpGet]
        [Route("/api/logtest/exception")]
        public IActionResult TestApiException()
        {
            throw new ArgumentNullException("testParam", "This is a test API exception");
        }

        /// <summary>
        /// Test correlation ID propagation
        /// </summary>
        [HttpGet]
        public IActionResult TestCorrelationId()
        {
            var correlationId = HttpContext.Response.Headers["X-Correlation-ID"].FirstOrDefault();
            
            _logger.LogInformation("Testing correlation ID: {CorrelationId}", correlationId);
            
            ViewBag.CorrelationId = correlationId;
            return View("Index");
        }
    }
}
