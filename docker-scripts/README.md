# Docker Scripts for Shipping Application

This directory contains PowerShell scripts to help manage Docker operations for the Shipping application.

## Scripts

### build-images.ps1
Builds Docker images for the WebApi and UI services.

**Usage:**
```powershell
# Build with latest tag
.\docker-scripts\build-images.ps1

# Build with specific version
.\docker-scripts\build-images.ps1 -Version "v1.2.0"

# Build without cache
.\docker-scripts\build-images.ps1 -NoCache

# Build and push to registry
.\docker-scripts\build-images.ps1 -Version "v1.2.0" -Push
```

### start-app.ps1
Starts the application with environment-specific configurations.

**Usage:**
```powershell
# Start in development mode (default)
.\docker-scripts\start-app.ps1

# Start in production mode
.\docker-scripts\start-app.ps1 -Environment prod

# Build and start
.\docker-scripts\start-app.ps1 -Build

# Start in foreground (see logs)
.\docker-scripts\start-app.ps1 -Detached:$false
```

### cleanup.ps1
Cleans up Docker resources.

**Usage:**
```powershell
# Basic cleanup (stop containers, remove images)
.\docker-scripts\cleanup.ps1

# Include volumes cleanup
.\docker-scripts\cleanup.ps1 -Volumes

# Full cleanup (all unused resources)
.\docker-scripts\cleanup.ps1 -All

# Force cleanup (remove orphans)
.\docker-scripts\cleanup.ps1 -Force
```

## Quick Start

1. **First time setup:**
   ```powershell
   .\docker-scripts\build-images.ps1
   .\docker-scripts\start-app.ps1 -Environment dev
   ```

2. **Daily development:**
   ```powershell
   .\docker-scripts\start-app.ps1 -Build
   ```

3. **Production deployment:**
   ```powershell
   .\docker-scripts\build-images.ps1 -Version "v1.0.0"
   .\docker-scripts\start-app.ps1 -Environment prod
   ```

4. **Cleanup:**
   ```powershell
   .\docker-scripts\cleanup.ps1 -All -Volumes
   ```

## Service URLs

### Development Mode
- **WebApi:** https://localhost:5001 (HTTPS), http://localhost:5000 (HTTP)
- **UI:** https://localhost:5003 (HTTPS), http://localhost:5002 (HTTP)
- **SQL Server:** localhost:1433

### Production Mode  
- **WebApi:** http://localhost:5000
- **UI:** http://localhost:5001
- **SQL Server:** localhost:1433

## Notes

- Make sure Docker Desktop is running before executing any scripts
- The SQL Server password is set to `YourStrong@Password123` (change in production)
- All scripts include error handling and colored output for better user experience
- Use `-Detached:$false` to see real-time logs during startup