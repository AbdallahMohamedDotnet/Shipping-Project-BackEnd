using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;
using System.Text.Json;

namespace Ui.Middleware
{
    /// <summary>
    /// Global exception handler middleware for catching unhandled exceptions,
    /// logging them with full context, and returning consistent error responses
    /// </summary>
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, IHostEnvironment environment)
        {
            _next = next;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Log the exception with full details
            LogException(context, exception);

            // Determine the status code based on exception type
            var (statusCode, title) = GetExceptionDetails(exception);

            // Check if this is an API request or a page request
            if (IsApiRequest(context))
            {
                await WriteJsonErrorResponse(context, statusCode, title, exception);
            }
            else
            {
                // For page requests, redirect to error page
                context.Response.Redirect($"/Home/Error?statusCode={(int)statusCode}");
            }
        }

        private void LogException(HttpContext context, Exception exception)
        {
            var requestBody = GetRequestBodySafe(context);

            Log.Error(exception,
                "Unhandled exception occurred. " +
                "ExceptionType: {ExceptionType}, " +
                "Message: {ExceptionMessage}, " +
                "Path: {RequestPath}, " +
                "Method: {RequestMethod}, " +
                "QueryString: {QueryString}, " +
                "RequestBody: {RequestBody}",
                exception.GetType().FullName,
                exception.Message,
                context.Request.Path,
                context.Request.Method,
                context.Request.QueryString.ToString(),
                requestBody);

            // Log inner exceptions
            var innerException = exception.InnerException;
            var depth = 1;
            while (innerException != null && depth <= 5)
            {
                Log.Error(
                    "Inner Exception (Depth {Depth}): Type: {InnerExceptionType}, Message: {InnerExceptionMessage}",
                    depth,
                    innerException.GetType().FullName,
                    innerException.Message);

                innerException = innerException.InnerException;
                depth++;
            }
        }

        private static (HttpStatusCode statusCode, string title) GetExceptionDetails(Exception exception)
        {
            return exception switch
            {
                ArgumentNullException => (HttpStatusCode.BadRequest, "Invalid Request"),
                ArgumentException => (HttpStatusCode.BadRequest, "Invalid Argument"),
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized"),
                KeyNotFoundException => (HttpStatusCode.NotFound, "Resource Not Found"),
                InvalidOperationException => (HttpStatusCode.Conflict, "Invalid Operation"),
                TimeoutException => (HttpStatusCode.RequestTimeout, "Request Timeout"),
                OperationCanceledException => (HttpStatusCode.RequestTimeout, "Operation Cancelled"),
                _ => (HttpStatusCode.InternalServerError, "Internal Server Error")
            };
        }

        private static bool IsApiRequest(HttpContext context)
        {
            // Check if the request accepts JSON or is under /api path
            return context.Request.Path.StartsWithSegments("/api") ||
                   context.Request.Headers.Accept.Any(h => h?.Contains("application/json") == true);
        }

        private async Task WriteJsonErrorResponse(HttpContext context, HttpStatusCode statusCode, string title, Exception exception)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var problemDetails = new ProblemDetails
            {
                Status = (int)statusCode,
                Title = title,
                Detail = _environment.IsDevelopment() ? exception.Message : "An error occurred while processing your request.",
                Instance = context.Request.Path
            };

            // Add additional details in development
            if (_environment.IsDevelopment())
            {
                problemDetails.Extensions["exceptionType"] = exception.GetType().FullName;
                problemDetails.Extensions["stackTrace"] = exception.StackTrace;
            }

            var json = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            await context.Response.WriteAsync(json);
        }

        private static string GetRequestBodySafe(HttpContext context)
        {
            try
            {
                // Only attempt to read body for methods that typically have one
                if (context.Request.Method is "POST" or "PUT" or "PATCH")
                {
                    context.Request.EnableBuffering();
                    context.Request.Body.Position = 0;

                    using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                    var body = reader.ReadToEndAsync().GetAwaiter().GetResult();
                    context.Request.Body.Position = 0;

                    // Truncate large bodies and sanitize sensitive data
                    if (body.Length > 1000)
                    {
                        body = body[..1000] + "... [truncated]";
                    }

                    // Basic sanitization - remove potential sensitive fields
                    return SanitizeRequestBody(body);
                }
            }
            catch
            {
                // Ignore errors reading body
            }

            return "[Not Available]";
        }

        private static string SanitizeRequestBody(string body)
        {
            // Basic sanitization for common sensitive fields
            var sensitivePatterns = new[] { "password", "token", "secret", "apikey", "authorization" };

            foreach (var pattern in sensitivePatterns)
            {
                // Simple pattern replacement (for production, use regex)
                if (body.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                {
                    return "[Body contains sensitive data - redacted]";
                }
            }

            return body;
        }
    }

    /// <summary>
    /// Extension method to register the global exception handler middleware
    /// </summary>
    public static class GlobalExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }
}
