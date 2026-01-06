# Weather Forecast API - Quick Start Guide

## 🚀 Getting Started in 5 Minutes

### Step 1: Start the API
```bash
# Navigate to WebApi project
cd WebApi

# Run the project
dotnet run
```

### Step 2: Open Swagger UI
Navigate to: `https://localhost:{your-port}/swagger`

### Step 3: Test Your First Endpoint

#### Option A: Using Swagger UI
1. Find the **WeatherForecast** section
2. Click on `GET /api/WeatherForecast`
3. Click **"Try it out"**
4. Click **"Execute"**
5. See the response below! 🎉

#### Option B: Using Browser
Simply open: `https://localhost:{your-port}/api/WeatherForecast`

#### Option C: Using cURL
```bash
curl -X GET "https://localhost:7001/api/WeatherForecast" -k
```

---

## 📋 Available Endpoints

| Endpoint | Description | Example |
|----------|-------------|---------|
| `GET /api/WeatherForecast` | Get 5-day forecast | `/api/WeatherForecast?days=7` |
| `GET /api/WeatherForecast/current` | Get current weather | `/api/WeatherForecast/current` |
| `GET /api/WeatherForecast/date/{date}` | Get forecast for date | `/api/WeatherForecast/date/2025-01-15` |
| `GET /api/WeatherForecast/range` | Get forecast range | `/api/WeatherForecast/range?startDate=2025-01-10&endDate=2025-01-15` |
| `GET /api/WeatherForecast/statistics` | Get weather stats | `/api/WeatherForecast/statistics?days=14` |

---

## 🎯 Quick Examples

### Get 10-Day Forecast
```http
GET https://localhost:7001/api/WeatherForecast?days=10
```

### Get Current Weather
```http
GET https://localhost:7001/api/WeatherForecast/current
```

### Get Forecast for Specific Date
```http
GET https://localhost:7001/api/WeatherForecast/date/2025-01-20
```

### Get Weekly Statistics
```http
GET https://localhost:7001/api/WeatherForecast/statistics?days=7
```

---

## 📊 Sample Response

```json
{
  "success": true,
  "message": "Successfully retrieved 5 day forecast",
  "data": [
    {
      "date": "2025-01-08",
      "temperatureC": 22,
      "temperatureF": 71,
      "summary": "Mild",
      "condition": 3,
      "humidity": 65,
      "windSpeed": 12.5
    },
    {
      "date": "2025-01-09",
      "temperatureC": 25,
      "temperatureF": 77,
      "summary": "Warm",
      "condition": 4,
      "humidity": 60,
      "windSpeed": 15.2
    }
  ],
  "errors": null
}
```

---

## 🔧 Common Parameters

| Parameter | Type | Range | Default | Description |
|-----------|------|-------|---------|-------------|
| `days` | int | 1-30 | 5 | Number of forecast days |
| `date` | DateOnly | Next 30 days | - | Specific date (YYYY-MM-DD) |
| `startDate` | DateOnly | Next 30 days | - | Range start date |
| `endDate` | DateOnly | Next 30 days | - | Range end date |

---

## 🌡️ Weather Conditions

| Value | Condition | Temperature (°C) |
|-------|-----------|------------------|
| 0 | Freezing | < 0 |
| 1 | Cold | 0-9 |
| 2 | Cool | 10-14 |
| 3 | Mild | 15-19 |
| 4 | Warm | 20-24 |
| 5 | Hot | 25-34 |
| 6 | VeryHot | ≥ 35 |

---

## ❌ Common Errors

### Invalid Days Parameter
```http
GET /api/WeatherForecast?days=50
```
**Response:** `400 Bad Request`
```json
{
  "success": false,
  "message": "Invalid days parameter",
  "errors": ["Days must be between 1 and 30"]
}
```

### Date Out of Range
```http
GET /api/WeatherForecast/date/2020-01-01
```
**Response:** `404 Not Found`
```json
{
  "success": false,
  "message": "Date not found",
  "errors": ["The requested date is out of forecast range (next 30 days only)"]
}
```

---

## 🧪 Testing with Postman

### Quick Import (Manual Setup):

1. **Create New Request**
   - Method: GET
   - URL: `https://localhost:7001/api/WeatherForecast`
   
2. **Add Tests**
   ```javascript
   pm.test("Status is 200", () => {
       pm.response.to.have.status(200);
   });
   
   pm.test("Success is true", () => {
       pm.expect(pm.response.json().success).to.be.true;
   });
   ```

3. **Send & Verify** ✅

---

## 🐛 Troubleshooting

### Port Already in Use
```bash
# Stop other instances
dotnet build
dotnet run --urls "https://localhost:7002"
```

### SSL Certificate Issues
```bash
# Trust the development certificate
dotnet dev-certs https --trust
```

### 404 Not Found
- ✅ Check the URL path: `/api/WeatherForecast` (case-sensitive)
- ✅ Ensure WebApi project is running
- ✅ Check the port number

### CORS Errors
- ✅ CORS is configured for `https://localhost:7279`
- ✅ Update in `Program.cs` if using different port

---

## 📚 Next Steps

1. ✅ Read the full [Testing Guide](WEATHER_API_TESTING_GUIDE.md)
2. ✅ Review [Refactoring Summary](REFACTORING_SUMMARY.md)
3. ✅ Explore Swagger UI for interactive testing
4. ✅ Try all available endpoints
5. ✅ Test error scenarios
6. ✅ Integrate with UI project

---

## 💡 Pro Tips

- 🔥 Use Swagger UI for quick testing during development
- 🔥 Check logs for detailed debugging information
- 🔥 Use `statistics` endpoint for analytics and reporting
- 🔥 The `current` endpoint is perfect for dashboard displays
- 🔥 Date range queries are great for calendar integrations

---

## 🎓 Learning Resources

**Project Files:**
- `WeatherForecast.cs` - Data model
- `IWeatherForecastService.cs` - Service interface
- `WeatherForecastService.cs` - Business logic
- `WeatherForecastController.cs` - API endpoints

**Architecture:**
```
Controller → Service → Business Logic → Response
     ↓          ↓            ↓              ↓
  Routing   DI/IoC      Processing      ApiResponse<T>
```

---

## 🤝 Need Help?

- Check the [WEATHER_API_TESTING_GUIDE.md](WEATHER_API_TESTING_GUIDE.md) for comprehensive documentation
- Review the [REFACTORING_SUMMARY.md](REFACTORING_SUMMARY.md) for architecture details
- Look at code comments and XML documentation
- Test with Swagger UI for interactive exploration

---

## ✨ Features at a Glance

- ✅ 5 RESTful endpoints
- ✅ Async/await for performance
- ✅ Comprehensive error handling
- ✅ Input validation
- ✅ Structured logging
- ✅ Swagger/OpenAPI documentation
- ✅ Service layer architecture
- ✅ Dependency injection
- ✅ Consistent API responses
- ✅ Production-ready code

---

**Happy Testing! 🎉**
