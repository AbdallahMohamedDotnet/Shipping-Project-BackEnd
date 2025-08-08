#!/usr/bin/env pwsh

param(
    [ValidateSet("dev", "prod")]
    [string]$Environment = "dev",
    [switch]$Build,
    [switch]$Detached = $true
)

Write-Host "Starting Shipping application in $Environment environment..." -ForegroundColor Green

$ErrorActionPreference = "Stop"

try {
    # Build if requested
    if ($Build) {
        Write-Host "Building images first..." -ForegroundColor Yellow
        docker-compose build
        if ($LASTEXITCODE -ne 0) {
            throw "Build failed"
        }
    }

    # Prepare compose arguments
    $composeArgs = @("up")
    
    if ($Detached) {
        $composeArgs += "-d"
    }

    # Run based on environment
    if ($Environment -eq "dev") {
        Write-Host "Starting in development mode with overrides..." -ForegroundColor Yellow
        docker-compose -f docker-compose.yml -f docker-compose.override.yml @composeArgs
    } else {
        Write-Host "Starting in production mode..." -ForegroundColor Yellow
        docker-compose -f docker-compose.yml @composeArgs
    }

    if ($LASTEXITCODE -ne 0) {
        throw "Failed to start services"
    }

    # Show running containers
    Write-Host "`nRunning containers:" -ForegroundColor Cyan
    docker-compose ps

    # Show service URLs
    Write-Host "`nService URLs:" -ForegroundColor Green
    if ($Environment -eq "dev") {
        Write-Host "WebApi: https://localhost:5001, http://localhost:5000" -ForegroundColor White
        Write-Host "UI: https://localhost:5003, http://localhost:5002" -ForegroundColor White
    } else {
        Write-Host "WebApi: http://localhost:5000" -ForegroundColor White
        Write-Host "UI: http://localhost:5001" -ForegroundColor White
    }
    Write-Host "SQL Server: localhost:1433 (sa/YourStrong@Password123)" -ForegroundColor White

} catch {
    Write-Error "Failed to start application: $_"
    exit 1
}