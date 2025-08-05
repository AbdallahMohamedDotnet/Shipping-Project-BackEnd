#!/usr/bin/env pwsh
# Shipping Project Manager Script

param(
    [Parameter(Position=0)]
    [ValidateSet("start", "watch", "build", "clean", "stop", "help")]
    [string]$Command = "help"
)

function Show-Help {
    Write-Host "Shipping Project Manager" -ForegroundColor Green
    Write-Host "======================" -ForegroundColor Green
    Write-Host ""
    Write-Host "Usage: .\project-manager.ps1 [command]" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Commands:" -ForegroundColor Cyan
    Write-Host "  start   - Start both WebApi and UI projects" -ForegroundColor White
    Write-Host "  watch   - Start both projects with hot reload" -ForegroundColor White
    Write-Host "  build   - Build the entire solution" -ForegroundColor White
    Write-Host "  clean   - Clean the solution" -ForegroundColor White
    Write-Host "  stop    - Stop all dotnet processes" -ForegroundColor White
    Write-Host "  help    - Show this help message" -ForegroundColor White
    Write-Host ""
    Write-Host "URLs:" -ForegroundColor Magenta
    Write-Host "  WebApi: https://localhost:7048" -ForegroundColor Gray
    Write-Host "  UI:     https://localhost:7279" -ForegroundColor Gray
    Write-Host "  Swagger: https://localhost:7048/swagger" -ForegroundColor Gray
}

function Start-Projects {
    Write-Host "Starting WebApi and UI projects..." -ForegroundColor Green
    Start-Process -FilePath "dotnet" -ArgumentList "run --project WebApi" -WorkingDirectory $PWD
    Start-Sleep -Seconds 3
    Start-Process -FilePath "dotnet" -ArgumentList "run --project Ui" -WorkingDirectory $PWD
    Write-Host "Projects started!" -ForegroundColor Green
}

function Start-ProjectsWithWatch {
    Write-Host "Starting WebApi and UI projects with hot reload..." -ForegroundColor Green
    Start-Process -FilePath "dotnet" -ArgumentList "watch run --project WebApi" -WorkingDirectory $PWD
    Start-Sleep -Seconds 3
    Start-Process -FilePath "dotnet" -ArgumentList "watch run --project Ui" -WorkingDirectory $PWD
    Write-Host "Projects started with hot reload!" -ForegroundColor Green
}

function Build-Solution {
    Write-Host "Building solution..." -ForegroundColor Yellow
    dotnet build
}

function Clean-Solution {
    Write-Host "Cleaning solution..." -ForegroundColor Yellow
    dotnet clean
}

function Stop-Projects {
    Write-Host "Stopping all dotnet processes..." -ForegroundColor Red
    Get-Process dotnet -ErrorAction SilentlyContinue | Stop-Process -Force
    Write-Host "All dotnet processes stopped!" -ForegroundColor Green
}

switch ($Command) {
    "start" { Start-Projects }
    "watch" { Start-ProjectsWithWatch }
    "build" { Build-Solution }
    "clean" { Clean-Solution }
    "stop" { Stop-Projects }
    "help" { Show-Help }
    default { Show-Help }
}