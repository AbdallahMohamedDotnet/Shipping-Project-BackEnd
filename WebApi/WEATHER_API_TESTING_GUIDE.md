# Weather Forecast API Testing Guide

## Overview
This document provides comprehensive testing instructions for the refactored Weather Forecast API.

## Base URL
- Local Development: `https://localhost:{port}/api/WeatherForecast`
- Replace `{port}` with your WebApi project port number

## Endpoints

### 1. Get Weather Forecast
**Endpoint:** `GET /api/WeatherForecast`

**Description:** Get weather forecast for multiple days (default: 5 days, max: 30 days)

**Query Parameters:**
- `days` (optional): Number of days (1-30). Default: 5

**Example Requests:**
```http
GET https://localhost:7001/api/WeatherForecast
GET https://localhost:7001/api/WeatherForecast?days=7
GET https://localhost:7001/api/WeatherForecast?days=14
```

**Success Response (200 OK):**
```json
{
  "success": true,
  "message": "Successfully retrieved 5 day forecast",
  "data": [
    {
      "date": "2025-01-08",
      "temperatureC": 25,
      "temperatureF": 77,
      "summary": "Warm",
      "condition": 3,
      "humidity": 65,
      "windSpeed": 15.5
    }
  ],
  "errors": null
}
```

**Error Response (400 Bad Request):**
```json
{
  "success": false,
  "message": "Invalid days parameter",
  "data": null,
  "errors": ["Days must be between 1 and 30"]
}
```

---

### 2. Get Current Weather
**Endpoint:** `GET /api/WeatherForecast/current`

**Description:** Get current weather conditions

**Example Request:**
```http
GET https://localhost:7001/api/WeatherForecast/current
```

**Success Response (200 OK):**
```json
{
  "success": true,
  "message": "Successfully retrieved current weather",
  "data": {
    "date": "2025-01-07",
    "temperatureC": 22,
    "temperatureF": 71,
    "summary": "Mild",
    "condition": 3,
    "humidity": 70,
    "windSpeed": 12.3
  },
  "errors": null
}
```

---

### 3. Get Forecast by Date
**Endpoint:** `GET /api/WeatherForecast/date/{date}`

**Description:** Get weather forecast for a specific date

**Path Parameters:**
- `date`: Date in format YYYY-MM-DD (next 30 days only)

**Example Requests:**
```http
GET https://localhost:7001/api/WeatherForecast/date/2025-01-10
GET https://localhost:7001/api/WeatherForecast/date/2025-01-15
```

**Success Response (200 OK):**
```json
{
  "success": true,
  "message": "Successfully retrieved forecast for 2025-01-10",
  "data": {
    "date": "2025-01-10",
    "temperatureC": 18,
    "temperatureF": 64,
    "summary": "Cool",
    "condition": 2,
    "humidity": 55,
    "windSpeed": 8.7
  },
  "errors": null
}
```

**Error Response (404 Not Found):**
```json
{
  "success": false,
  "message": "Date not found",
  "data": null,
  "errors": ["The requested date is out of forecast range (next 30 days only)"]
}
```

---

### 4. Get Forecast Range
**Endpoint:** `GET /api/WeatherForecast/range`

**Description:** Get weather forecast for a date range

**Query Parameters:**
- `startDate`: Start date in format YYYY-MM-DD
- `endDate`: End date in format YYYY-MM-DD

**Example Requests:**
```http
GET https://localhost:7001/api/WeatherForecast/range?startDate=2025-01-08&endDate=2025-01-12
GET https://localhost:7001/api/WeatherForecast/range?startDate=2025-01-10&endDate=2025-01-20
```

**Success Response (200 OK):**
```json
{
  "success": true,
  "message": "Successfully retrieved forecast from 2025-01-08 to 2025-01-12",
  "data": [
    {
      "date": "2025-01-08",
      "temperatureC": 24,
      "temperatureF": 75,
      "summary": "Warm",
      "condition": 4,
      "humidity": 60,
      "windSpeed": 11.2
    },
    {
      "date": "2025-01-09",
      "temperatureC": 26,
      "temperatureF": 78,
      "summary": "Warm",
      "condition": 4,
      "humidity": 58,
      "windSpeed": 9.8
    }
  ],
  "errors": null
}
```

**Error Response (400 Bad Request):**
```json
{
  "success": false,
  "message": "Invalid date range",
  "data": null,
  "errors": ["End date must be after start date"]
}
```

---

### 5. Get Weather Statistics
**Endpoint:** `GET /api/WeatherForecast/statistics`

**Description:** Get weather statistics summary (averages, min, max, etc.)

**Query Parameters:**
- `days` (optional): Number of days to analyze (1-30). Default: 7

**Example Requests:**
```http
GET https://localhost:7001/api/WeatherForecast/statistics
GET https://localhost:7001/api/WeatherForecast/statistics?days=14
GET https://localhost:7001/api/WeatherForecast/statistics?days=30
```

**Success Response (200 OK):**
```json
{
  "success": true,
  "message": "Successfully retrieved statistics for 7 days",
  "data": {
    "periodStart": "2025-01-08",
    "periodEnd": "2025-01-14",
    "totalDays": 7,
    "averageTemperatureC": 22.5,
    "averageTemperatureF": 72.5,
    "minTemperatureC": 10,
    "maxTemperatureC": 35,
    "averageHumidity": 65.5,
    "averageWindSpeed": 14.3,
    "mostCommonCondition": "Warm"
  },
  "errors": null
}
```

---

## Weather Condition Enum Values

| Value | Condition | Temperature Range (°C) |
|-------|-----------|------------------------|
| 0     | Freezing  | < 0                    |
| 1     | Cold      | 0 - 9                  |
| 2     | Cool      | 10 - 14                |
| 3     | Mild      | 15 - 19                |
| 4     | Warm      | 20 - 24                |
| 5     | Hot       | 25 - 34                |
| 6     | VeryHot   | >= 35                  |

---

## Testing with cURL

### Basic forecast (5 days)
```bash
curl -X GET "https://localhost:7001/api/WeatherForecast" -H "accept: application/json"
```

### Custom number of days
```bash
curl -X GET "https://localhost:7001/api/WeatherForecast?days=10" -H "accept: application/json"
```

### Current weather
```bash
curl -X GET "https://localhost:7001/api/WeatherForecast/current" -H "accept: application/json"
```

### Specific date
```bash
curl -X GET "https://localhost:7001/api/WeatherForecast/date/2025-01-15" -H "accept: application/json"
```

### Date range
```bash
curl -X GET "https://localhost:7001/api/WeatherForecast/range?startDate=2025-01-10&endDate=2025-01-15" -H "accept: application/json"
```

### Statistics
```bash
curl -X GET "https://localhost:7001/api/WeatherForecast/statistics?days=14" -H "accept: application/json"
```

---

## Testing with Postman

1. **Import Collection**: Create a new collection named "Weather Forecast API"
2. **Set Variables**: 
   - `baseUrl`: https://localhost:7001
3. **Create Requests**: Add requests for each endpoint listed above
4. **Run Tests**: Execute individual requests or run the entire collection

### Sample Postman Tests

Add these to the "Tests" tab in Postman:

```javascript
// Test for successful response
pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});

pm.test("Response has success property", function () {
    var jsonData = pm.response.json();
    pm.expect(jsonData).to.have.property('success');
});

pm.test("Success is true", function () {
    var jsonData = pm.response.json();
    pm.expect(jsonData.success).to.be.true;
});

pm.test("Response has data property", function () {
    var jsonData = pm.response.json();
    pm.expect(jsonData).to.have.property('data');
});
```

---

## Testing with Swagger UI

1. Start the WebApi project
2. Navigate to: `https://localhost:{port}/swagger`
3. Locate the WeatherForecast section
4. Click "Try it out" on any endpoint
5. Enter parameters if required
6. Click "Execute"
7. Review the response

---

## Error Scenarios to Test

### Invalid Days Parameter
```http
GET /api/WeatherForecast?days=0
GET /api/WeatherForecast?days=31
GET /api/WeatherForecast?days=-5
```
**Expected:** 400 Bad Request with error message

### Past Date
```http
GET /api/WeatherForecast/date/2020-01-01
```
**Expected:** 404 Not Found

### Invalid Date Range
```http
GET /api/WeatherForecast/range?startDate=2025-01-15&endDate=2025-01-10
```
**Expected:** 400 Bad Request with error message

### Date Range Too Large
```http
GET /api/WeatherForecast/range?startDate=2025-01-01&endDate=2025-03-01
```
**Expected:** 400 Bad Request with error message

---

## Performance Testing

Test the API with various load scenarios:

1. **Single Request**: Test response time for individual requests
2. **Concurrent Requests**: Send multiple requests simultaneously
3. **Large Date Ranges**: Request maximum allowed date ranges (30 days)
4. **Statistics with Max Days**: Request statistics for 30 days

**Expected Performance:**
- Response time: < 200ms for standard requests
- Concurrent handling: Should handle at least 100 concurrent requests
- No memory leaks or performance degradation over time

---

## Integration Testing Checklist

- [ ] All endpoints return proper HTTP status codes
- [ ] ApiResponse wrapper is consistent across all endpoints
- [ ] Validation errors return 400 Bad Request
- [ ] Out of range dates return 404 Not Found
- [ ] Server errors return 500 Internal Server Error
- [ ] Logging works correctly for all scenarios
- [ ] CORS is configured properly for UI consumption
- [ ] Data annotations are validated
- [ ] Async/await patterns work correctly
- [ ] Service layer properly isolated from controller

---

## Notes

- All endpoints use async/await for better scalability
- Service layer separates business logic from controller
- Comprehensive logging for debugging and monitoring
- Consistent error handling across all endpoints
- ApiResponse wrapper provides uniform response structure
- Data validation using data annotations
- XML documentation for Swagger UI
- RESTful API design principles followed
