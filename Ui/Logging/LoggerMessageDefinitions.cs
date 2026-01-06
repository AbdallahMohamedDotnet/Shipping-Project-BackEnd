using Microsoft.Extensions.Logging;

namespace Ui.Logging
{
    /// <summary>
    /// High-performance source-generated logging methods for common application events.
    /// Using [LoggerMessage] attribute for zero-allocation logging in hot paths.
    /// </summary>
    public static partial class LoggerMessageDefinitions
    {
        // ============================================
        // USER ACTION LOGS (EventId: 1000-1999)
        // ============================================

        [LoggerMessage(
            EventId = 1001,
            Level = LogLevel.Information,
            Message = "User {UserName} logged in successfully")]
        public static partial void UserLoggedIn(ILogger logger, string userName);

        [LoggerMessage(
            EventId = 1002,
            Level = LogLevel.Information,
            Message = "User {UserName} logged out")]
        public static partial void UserLoggedOut(ILogger logger, string userName);

        [LoggerMessage(
            EventId = 1003,
            Level = LogLevel.Warning,
            Message = "Failed login attempt for user {UserName}. Reason: {Reason}")]
        public static partial void LoginFailed(ILogger logger, string userName, string reason);

        [LoggerMessage(
            EventId = 1004,
            Level = LogLevel.Information,
            Message = "User {UserName} accessed resource: {ResourcePath}")]
        public static partial void ResourceAccessed(ILogger logger, string userName, string resourcePath);

        [LoggerMessage(
            EventId = 1005,
            Level = LogLevel.Warning,
            Message = "Access denied for user {UserName} to resource: {ResourcePath}")]
        public static partial void AccessDenied(ILogger logger, string userName, string resourcePath);

        // ============================================
        // DATA ACCESS LOGS (EventId: 2000-2999)
        // ============================================

        [LoggerMessage(
            EventId = 2001,
            Level = LogLevel.Debug,
            Message = "Entity {EntityType} with ID {EntityId} was retrieved")]
        public static partial void EntityRetrieved(ILogger logger, string entityType, string entityId);

        [LoggerMessage(
            EventId = 2002,
            Level = LogLevel.Information,
            Message = "Entity {EntityType} with ID {EntityId} was created by {UserName}")]
        public static partial void EntityCreated(ILogger logger, string entityType, string entityId, string userName);

        [LoggerMessage(
            EventId = 2003,
            Level = LogLevel.Information,
            Message = "Entity {EntityType} with ID {EntityId} was updated by {UserName}")]
        public static partial void EntityUpdated(ILogger logger, string entityType, string entityId, string userName);

        [LoggerMessage(
            EventId = 2004,
            Level = LogLevel.Warning,
            Message = "Entity {EntityType} with ID {EntityId} was deleted by {UserName}")]
        public static partial void EntityDeleted(ILogger logger, string entityType, string entityId, string userName);

        [LoggerMessage(
            EventId = 2005,
            Level = LogLevel.Warning,
            Message = "Entity {EntityType} with ID {EntityId} was not found")]
        public static partial void EntityNotFound(ILogger logger, string entityType, string entityId);

        // ============================================
        // SECURITY LOGS (EventId: 3000-3999)
        // ============================================

        [LoggerMessage(
            EventId = 3001,
            Level = LogLevel.Warning,
            Message = "Invalid token received. TokenType: {TokenType}, Reason: {Reason}")]
        public static partial void InvalidToken(ILogger logger, string tokenType, string reason);

        [LoggerMessage(
            EventId = 3002,
            Level = LogLevel.Information,
            Message = "Token refreshed for user {UserName}")]
        public static partial void TokenRefreshed(ILogger logger, string userName);

        [LoggerMessage(
            EventId = 3003,
            Level = LogLevel.Critical,
            Message = "Potential security breach detected. Type: {ThreatType}, Details: {Details}")]
        public static partial void SecurityThreatDetected(ILogger logger, string threatType, string details);

        [LoggerMessage(
            EventId = 3004,
            Level = LogLevel.Warning,
            Message = "Rate limit exceeded for IP {ClientIp}. Endpoint: {Endpoint}")]
        public static partial void RateLimitExceeded(ILogger logger, string clientIp, string endpoint);

        // ============================================
        // PERFORMANCE LOGS (EventId: 4000-4999)
        // ============================================

        [LoggerMessage(
            EventId = 4001,
            Level = LogLevel.Warning,
            Message = "Slow operation detected. Operation: {OperationName}, Duration: {DurationMs}ms, Threshold: {ThresholdMs}ms")]
        public static partial void SlowOperationDetected(ILogger logger, string operationName, long durationMs, long thresholdMs);

        [LoggerMessage(
            EventId = 4002,
            Level = LogLevel.Debug,
            Message = "Operation completed. Operation: {OperationName}, Duration: {DurationMs}ms")]
        public static partial void OperationCompleted(ILogger logger, string operationName, long durationMs);

        [LoggerMessage(
            EventId = 4003,
            Level = LogLevel.Warning,
            Message = "High memory usage detected. UsedMB: {UsedMb}, ThresholdMB: {ThresholdMb}")]
        public static partial void HighMemoryUsage(ILogger logger, long usedMb, long thresholdMb);

        // ============================================
        // EXTERNAL SERVICE LOGS (EventId: 5000-5999)
        // ============================================

        [LoggerMessage(
            EventId = 5001,
            Level = LogLevel.Information,
            Message = "API call initiated. Service: {ServiceName}, Endpoint: {Endpoint}")]
        public static partial void ApiCallInitiated(ILogger logger, string serviceName, string endpoint);

        [LoggerMessage(
            EventId = 5002,
            Level = LogLevel.Information,
            Message = "API call completed. Service: {ServiceName}, Endpoint: {Endpoint}, StatusCode: {StatusCode}, Duration: {DurationMs}ms")]
        public static partial void ApiCallCompleted(ILogger logger, string serviceName, string endpoint, int statusCode, long durationMs);

        [LoggerMessage(
            EventId = 5003,
            Level = LogLevel.Error,
            Message = "API call failed. Service: {ServiceName}, Endpoint: {Endpoint}, Error: {ErrorMessage}")]
        public static partial void ApiCallFailed(ILogger logger, Exception exception, string serviceName, string endpoint, string errorMessage);

        // ============================================
        // SHIPMENT BUSINESS LOGS (EventId: 6000-6999)
        // ============================================

        [LoggerMessage(
            EventId = 6001,
            Level = LogLevel.Information,
            Message = "Shipment created. TrackingNumber: {TrackingNumber}, SenderId: {SenderId}, ReceiverId: {ReceiverId}")]
        public static partial void ShipmentCreated(ILogger logger, string trackingNumber, string senderId, string receiverId);

        [LoggerMessage(
            EventId = 6002,
            Level = LogLevel.Information,
            Message = "Shipment status updated. TrackingNumber: {TrackingNumber}, OldStatus: {OldStatus}, NewStatus: {NewStatus}")]
        public static partial void ShipmentStatusUpdated(ILogger logger, string trackingNumber, string oldStatus, string newStatus);

        [LoggerMessage(
            EventId = 6003,
            Level = LogLevel.Warning,
            Message = "Shipment delivery failed. TrackingNumber: {TrackingNumber}, Reason: {Reason}")]
        public static partial void ShipmentDeliveryFailed(ILogger logger, string trackingNumber, string reason);

        [LoggerMessage(
            EventId = 6004,
            Level = LogLevel.Information,
            Message = "Rate calculated. ShipmentType: {ShipmentType}, Weight: {Weight}, CalculatedRate: {Rate}")]
        public static partial void RateCalculated(ILogger logger, string shipmentType, decimal weight, decimal rate);
    }
}
