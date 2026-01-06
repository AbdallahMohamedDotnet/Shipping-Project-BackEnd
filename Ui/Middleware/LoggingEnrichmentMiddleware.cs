using Serilog.Context;
using System.Security.Claims;

namespace Ui.Middleware
{
    /// <summary>
    /// Middleware to enrich Serilog logs with user context and request information
    /// </summary>
    public class LoggingEnrichmentMiddleware
    {
        private readonly RequestDelegate _next;
        private const string CorrelationIdHeader = "X-Correlation-ID";

        public LoggingEnrichmentMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Generate or retrieve correlation ID
            var correlationId = GetOrCreateCorrelationId(context);

            // Get user information if authenticated
            var userId = context.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Anonymous";
            var userName = context.User?.Identity?.Name ?? "Anonymous";

            // Get request details
            var requestPath = context.Request.Path.ToString();
            var requestMethod = context.Request.Method;
            var clientIp = GetClientIpAddress(context);

            // Push properties to Serilog LogContext
            using (LogContext.PushProperty("UserId", userId))
            using (LogContext.PushProperty("UserName", userName))
            using (LogContext.PushProperty("RequestPath", requestPath))
            using (LogContext.PushProperty("RequestMethod", requestMethod))
            using (LogContext.PushProperty("ClientIp", clientIp))
            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                // Add correlation ID to response headers for tracing
                context.Response.Headers[CorrelationIdHeader] = correlationId;

                await _next(context);
            }
        }

        private static string GetOrCreateCorrelationId(HttpContext context)
        {
            // Check if correlation ID exists in request headers
            if (context.Request.Headers.TryGetValue(CorrelationIdHeader, out var existingId) &&
                !string.IsNullOrWhiteSpace(existingId))
            {
                return existingId.ToString();
            }

            // Generate new correlation ID
            return Guid.NewGuid().ToString("N")[..12].ToUpperInvariant();
        }

        private static string GetClientIpAddress(HttpContext context)
        {
            // Check for forwarded header (when behind proxy/load balancer)
            var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                // X-Forwarded-For can contain multiple IPs, take the first one
                return forwardedFor.Split(',')[0].Trim();
            }

            // Check for real IP header
            var realIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(realIp))
            {
                return realIp;
            }

            // Fall back to connection remote IP
            return context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        }
    }

    /// <summary>
    /// Extension method to register the middleware
    /// </summary>
    public static class LoggingEnrichmentMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingEnrichment(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingEnrichmentMiddleware>();
        }
    }
}
