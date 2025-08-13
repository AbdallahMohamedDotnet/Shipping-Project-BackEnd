# Running WebAPI and UI Projects Together

## Overview
This guide explains how to run both the WebAPI and UI (Razor Pages) projects together in the same solution for development.

## Prerequisites
- Visual Studio 2022 or VS Code
- .NET 9 SDK
- SQL Server (for the database connection)

## Project Structure
```
D:\Shipping\
??? WebApi\          # WebAPI project (https://localhost:7048)
??? Ui\              # Razor Pages UI project (https://localhost:7279)
??? BL\              # Business Logic layer
??? DAL\             # Data Access Layer
??? Domains\         # Domain models
??? AppResources\    # Shared resources
??? Shipping.sln     # Solution file
```

## Step 1: Open the Solution
1. Open **Shipping.sln** in Visual Studio 2022
2. All projects should be loaded automatically

## Step 2: Set Multiple Startup Projects
1. Right-click the solution in Solution Explorer ? **"Set Startup Projects"**
2. Select **"Multiple startup projects"**
3. Set both projects to **"Start"**:
   - WebApi: **Start**
   - Ui: **Start**
4. Click **OK**

## Step 3: Verify Port Configuration

### WebAPI Project (Already Configured)
- **HTTPS**: https://localhost:7048
- **HTTP**: http://localhost:5232
- **Swagger**: https://localhost:7048/swagger

### UI Project (Already Configured)
- **HTTPS**: https://localhost:7279
- **HTTP**: http://localhost:5022

## Step 4: Configuration Files

### appsettings.json (UI Project) - Already Updated
```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:7048/"
  }
}
```

### HttpClient Configuration (Already Added)
The UI project now includes:
- **HttpClient** configured to communicate with WebAPI
- **ApiService** class for making HTTP calls
- **Example controller** demonstrating API integration

## Step 5: Run the Projects
1. Press **F5** or click **Start** in Visual Studio
2. Two browser windows should open:
   - **https://localhost:7048/swagger** - WebAPI Swagger documentation
   - **https://localhost:7279/** - UI application

## Step 6: Test the Integration
1. Navigate to: **https://localhost:7279/Shipping**
2. This page demonstrates calling the WebAPI from the UI
3. Click **"Test API Connection"** to verify connectivity

## Key Files Added/Modified

### New Files:
- **Ui/Services/ApiService.cs** - HTTP client service for API calls
- **Ui/Controllers/ShippingController.cs** - Example controller using API
- **Ui/Views/Shipping/Index.cshtml** - Example view showing API integration
- **Shipping.sln** - Solution file containing all projects

### Modified Files:
- **Ui/Services/RegisterServicesHelper.cs** - Added HttpClient and ApiService registration
- **Ui/appsettings.json** - Added API base URL configuration

## CORS Configuration (Already Set)
The WebAPI project already has CORS configured to allow requests from the UI:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:7279", "http://localhost:5022")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});
```

## Using the ApiService

### In Controllers:
```csharp
public class YourController : Controller
{
    private readonly ApiService _apiService;

    public YourController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> GetData()
    {
        var data = await _apiService.GetAsync<List<YourModel>>("api/yourcontroller");
        return View(data);
    }
}
```

### Available ApiService Methods:
- `GetAsync<T>(string endpoint)` - GET requests
- `PostAsync<T>(string endpoint, object data)` - POST requests
- `PutAsync<T>(string endpoint, object data)` - PUT requests
- `DeleteAsync(string endpoint)` - DELETE requests

## Troubleshooting

### Common Issues:
1. **Port conflicts**: Ensure no other applications are using ports 7048 or 7279
2. **CORS errors**: Verify the UI project URL is included in WebAPI CORS policy
3. **SSL certificate issues**: Accept development certificates when prompted
4. **Database connection**: Ensure SQL Server is running and connection string is correct

### Checking Logs:
- **UI logs**: Check the console output in Visual Studio
- **API logs**: Check Swagger UI and console output
- **Browser logs**: Open browser Developer Tools (F12) ? Console

## Next Steps
1. **Authentication**: Implement JWT token handling between projects
2. **Error handling**: Add comprehensive error handling for API calls
3. **Caching**: Implement response caching for better performance
4. **Production deployment**: Configure for production environment

## Support
If you encounter issues:
1. Check that both projects are running
2. Verify the URLs in browser address bar match the configuration
3. Check the console logs for detailed error messages
4. Ensure database connection is working