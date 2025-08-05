# Running WebApi and UI Together - Development Guide

## Quick Start

### Method 1: Visual Studio (Recommended)
1. Open `ShippingProject.sln` in Visual Studio
2. Right-click solution ? "Set Startup Projects..."
3. Select "Multiple startup projects"
4. Set both **WebApi** and **Ui** to "Start"
5. Press F5 or click Start

### Method 2: Using Scripts
Run one of these scripts from the solution root directory:

**For development with hot reload:**
```powershell
.\start-both-projects-watch.ps1
```

**For regular startup:**
```powershell
.\start-both-projects.ps1
```
or
```cmd
start-both-projects.bat
```

### Method 3: Manual Terminal Commands
Open two terminal windows in the solution directory:

**Terminal 1 (WebApi):**
```bash
dotnet watch run --project WebApi
```

**Terminal 2 (UI):**
```bash
dotnet watch run --project Ui
```

## URLs
- **WebApi**: https://localhost:7048
- **UI**: https://localhost:7279
- **WebApi Swagger**: https://localhost:7048/swagger

## Project Communication
The projects are configured to work together:
- CORS is enabled in WebApi for UI URLs
- Authentication uses JWT tokens with refresh token cookies
- Shared database context and business logic

## Important Notes
1. **Start WebApi first** - UI depends on WebApi services
2. **HTTPS is configured** - Both projects use SSL certificates
3. **Hot reload enabled** - Code changes automatically restart applications
4. **Shared database** - Both projects use the same connection string

## Troubleshooting
- If CORS errors occur, ensure WebApi is running first
- Check that both projects use the same database connection string
- Verify SSL certificates are trusted (run `dotnet dev-certs https --trust`)

## Development Workflow
1. Make changes to shared projects (BL, DAL, Domains)
2. Both applications will automatically restart
3. Test API endpoints via Swagger at https://localhost:7048/swagger
4. Test UI functionality at https://localhost:7279