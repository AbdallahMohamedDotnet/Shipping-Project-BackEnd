# 🧪 Advanced Logging System - Testing Guide

## Overview
This guide helps you test the newly implemented advanced logging system across both **Ui** and **WebApi** projects.

---

## 📊 Database Changes

### New Table: `ApplicationLogs`
The Serilog SQL Server sink will automatically create this table on first log.

**Columns:**
- `Id` (BIGINT, PK, Identity)
- `Message` (NVARCHAR(MAX))
- `MessageTemplate` (NVARCHAR(MAX))
- `Level` (NVARCHAR(128))
- `TimeStamp` (DATETIME2)
- `Exception` (NVARCHAR(MAX))
- `LogEvent` (NVARCHAR(MAX))
- **Custom Columns:**
  - `UserId` (NVARCHAR(450))
  - `UserName` (NVARCHAR(256))
  - `RequestPath` (NVARCHAR(500))
  - `RequestMethod` (NVARCHAR(10))
  - `ClientIp` (NVARCHAR(50))
  - `CorrelationId` (NVARCHAR(50))
  - `MachineName` (NVARCHAR(100))

### Verify Table Creation
```sql
-- Check if table exists
SELECT * FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME = 'ApplicationLogs'

-- View recent logs
SELECT TOP 20 
    TimeStamp,
    Level,
    Message,
    UserName,
    RequestPath,
    RequestMethod,
    ClientIp,
    CorrelationId
FROM ApplicationLogs 
ORDER BY TimeStamp DESC
```

---

## 🚀 Testing Steps

### Phase 1: Start Applications

#### 1. Start WebApi (Port 7048)
```powershell
cd WebApi
dotnet run
```
**Expected Output:**
```
[12:00:00 INF] Now listening on: https://localhost:7048
```

#### 2. Start Ui (Port 7279)
```powershell
cd Ui
dotnet run
```
**Expected Output:**
```
[12:00:05 INF] Now listening on: https://localhost:7279
```

---

### Phase 2: Test Ui Logging

#### Test Page Access
1. Navigate to: `https://localhost:7279/LogTest`
2. You should see the **Logging Test Dashboard**

#### Test Each Log Type

| Button | Expected Log | EventId | Level |
|--------|--------------|---------|-------|
| **Test Information Log** | User logged in | 1001 | Information |
| **Test Warning Log** | Access denied | 1005 | Warning |
| **Test Error Log** | Test error with exception | - | Error |
| **Test Performance Log** | Slow operation detected | 4001 | Warning |
| **Test Shipment Log** | Shipment created | 6001 | Information |
| **Test Exception Handler** | Unhandled exception (redirects to error page) | - | Error |

#### Test Correlation ID
1. Click **Test Correlation ID**
2. Check that a correlation ID is displayed (e.g., `A3F9C2B4E1D7`)
3. Verify in database:
```sql
SELECT TOP 5 CorrelationId, Message, TimeStamp 
FROM ApplicationLogs 
WHERE CorrelationId IS NOT NULL
ORDER BY TimeStamp DESC
```

#### Test API Exception (JSON Response)
1. Click **Test API Exception (JSON)**
2. You should see a JSON ProblemDetails response:
```json
{
  "status": 400,
  "title": "Invalid Request",
  "detail": "Value cannot be null. (Parameter 'testParam')",
  "instance": "/api/logtest/exception"
}
```

---

### Phase 3: Test WebApi Logging

#### Test Weather Forecast Endpoints

1. Open Swagger: `https://localhost:7048/swagger`

2. **Test GET /api/WeatherForecast**
   ```
   GET /api/WeatherForecast?days=7
   ```
   **Expected:** 200 OK with forecast data
   
   **Check Database:**
   ```sql
   SELECT TOP 5 * FROM ApplicationLogs 
   WHERE RequestPath LIKE '%WeatherForecast%'
   ORDER BY TimeStamp DESC
   ```

3. **Test Invalid Parameter (Validation Error)**
   ```
   GET /api/WeatherForecast?days=50
   ```
   **Expected:** 400 Bad Request
   
   **Check Log:** Should log warning with `EventId = 1003` or error message

4. **Test GET /api/WeatherForecast/current**
   ```
   GET /api/WeatherForecast/current
   ```
   **Expected:** 200 OK with current weather

5. **Test GET /api/WeatherForecast/date/{date}**
   ```
   GET /api/WeatherForecast/date/2025-01-15
   ```
   **Expected:** 200 OK or 404 Not Found

6. **Test GET /api/WeatherForecast/statistics**
   ```
   GET /api/WeatherForecast/statistics?days=7
   ```
   **Expected:** 200 OK with statistics

---

### Phase 4: Test User Context Enrichment

#### Login and Test Authenticated Logs

1. **Login to Ui**
   - Navigate to: `https://localhost:7279/Account/Login`
   - Login with test credentials

2. **Check User Context in Logs**
   ```sql
   SELECT 
       TimeStamp,
       Level,
       Message,
       UserId,
       UserName,
       RequestPath
   FROM ApplicationLogs 
   WHERE UserName IS NOT NULL AND UserName != 'Anonymous'
   ORDER BY TimeStamp DESC
   ```

3. **Verify UserId and UserName are captured**

---

### Phase 5: Test Exception Handling

#### Test Unhandled Exceptions

1. **Ui Exception Test**
   - Go to: `https://localhost:7279/LogTest`
   - Click **Test Exception Handler**
   - Should redirect to error page
   
2. **Check Exception Logging**
   ```sql
   SELECT TOP 5
       TimeStamp,
       Level,
       Message,
       Exception,
       RequestPath
   FROM ApplicationLogs 
   WHERE Exception IS NOT NULL
   ORDER BY TimeStamp DESC
   ```

3. **Verify Exception Details**
   - Exception type should be logged
   - Stack trace should be present
   - Inner exceptions should be logged (up to 5 levels)

---

### Phase 6: Test Performance Logging

#### Check Source-Generated LoggerMessage Performance

1. **Compare Allocations** (Advanced)
   ```powershell
   dotnet-counters monitor --process-id <PID> --counters System.Runtime
   ```

2. **Verify No Allocations in Hot Path**
   - The `[LoggerMessage]` methods should show zero allocation
   - Standard `_logger.LogInformation()` causes boxing allocations

---

## 🔍 Verification Queries

### 1. Check All Log Levels
```sql
SELECT Level, COUNT(*) as Count
FROM ApplicationLogs
GROUP BY Level
ORDER BY Count DESC
```

### 2. Check Request Paths
```sql
SELECT DISTINCT RequestPath, COUNT(*) as Count
FROM ApplicationLogs
WHERE RequestPath IS NOT NULL
GROUP BY RequestPath
ORDER BY Count DESC
```

### 3. Check User Activity
```sql
SELECT 
    UserName,
    COUNT(*) as LogCount,
    MIN(TimeStamp) as FirstActivity,
    MAX(TimeStamp) as LastActivity
FROM ApplicationLogs
WHERE UserName IS NOT NULL
GROUP BY UserName
```

### 4. Check Errors and Warnings
```sql
SELECT 
    Level,
    Message,
    Exception,
    TimeStamp,
    UserName,
    RequestPath
FROM ApplicationLogs
WHERE Level IN ('Error', 'Warning')
ORDER BY TimeStamp DESC
```

### 5. Check Correlation ID Chains
```sql
SELECT 
    CorrelationId,
    COUNT(*) as RequestCount,
    MIN(TimeStamp) as StartTime,
    MAX(TimeStamp) as EndTime,
    DATEDIFF(MILLISECOND, MIN(TimeStamp), MAX(TimeStamp)) as DurationMs
FROM ApplicationLogs
WHERE CorrelationId IS NOT NULL
GROUP BY CorrelationId
ORDER BY StartTime DESC
```

---

## ✅ Success Criteria

### Ui Project
- ✅ ApplicationLogs table created automatically
- ✅ All test buttons log correctly
- ✅ User context (UserId, UserName) captured when authenticated
- ✅ Correlation ID generated and propagated
- ✅ Exceptions logged with full stack trace
- ✅ Console output shows structured logs

### WebApi Project
- ✅ All Weather Forecast endpoints logged
- ✅ Optimized `[LoggerMessage]` methods used
- ✅ No "entry" noise logs (removed)
- ✅ Errors logged with context
- ✅ Request/response logging working

---

## 🐛 Troubleshooting

### Issue: Table Not Created
**Solution:** 
1. Check connection string in `appsettings.json`
2. Ensure SQL Server is running
3. Check user permissions for table creation

### Issue: No Logs Appearing
**Solution:**
1. Check Serilog configuration in `appsettings.json`
2. Verify `builder.Host.UseSerilog()` is called
3. Check console for Serilog initialization errors

### Issue: Custom Columns Empty
**Solution:**
1. Ensure `LoggingEnrichmentMiddleware` is registered **after** `UseAuthentication()`
2. Check middleware order in `Program.cs`
3. Verify `LogContext.PushProperty()` calls

### Issue: Performance Issues
**Solution:**
1. Check `BatchPostingLimit` and `BatchPeriod` in Serilog config
2. Consider reducing log level to Warning for production
3. Add index on `TimeStamp` column:
   ```sql
   CREATE INDEX IX_ApplicationLogs_TimeStamp 
   ON ApplicationLogs(TimeStamp DESC)
   ```

---

## 📈 Production Recommendations

### Log Retention
```sql
-- Create job to delete old logs (older than 30 days)
DELETE FROM ApplicationLogs 
WHERE TimeStamp < DATEADD(DAY, -30, GETDATE())
```

### Performance Optimization
```sql
-- Add indexes
CREATE INDEX IX_ApplicationLogs_Level 
ON ApplicationLogs(Level) INCLUDE (TimeStamp)

CREATE INDEX IX_ApplicationLogs_UserName 
ON ApplicationLogs(UserName) INCLUDE (TimeStamp)

CREATE INDEX IX_ApplicationLogs_CorrelationId 
ON ApplicationLogs(CorrelationId) INCLUDE (TimeStamp)
```

### Log Level for Production
```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Warning",
      "Override": {
        "Microsoft": "Error",
        "System": "Error"
      }
    }
  }
}
```

---

## 📚 Additional Resources

- **Serilog Documentation:** https://serilog.net/
- **LoggerMessage Source Generators:** https://learn.microsoft.com/en-us/dotnet/core/extensions/logger-message-generator
- **Structured Logging Best Practices:** https://stackify.com/what-is-structured-logging-and-why-developers-need-it/

---

**Last Updated:** January 2025
**Status:** ✅ All phases implemented and tested
