#!/usr/bin/env pwsh
# PowerShell script to run both WebApi and UI projects simultaneously

Write-Host "Starting WebApi and UI projects..." -ForegroundColor Green

# Start WebApi in background
Start-Process -FilePath "dotnet" -ArgumentList "run --project WebApi" -WorkingDirectory $PWD -WindowStyle Minimized

# Wait a moment for WebApi to start
Start-Sleep -Seconds 3

# Start UI project
Start-Process -FilePath "dotnet" -ArgumentList "run --project Ui" -WorkingDirectory $PWD

Write-Host "Both projects are starting..." -ForegroundColor Yellow
Write-Host "WebApi will be available at: https://localhost:7048" -ForegroundColor Cyan
Write-Host "UI will be available at: https://localhost:7279" -ForegroundColor Cyan
Write-Host "Press Ctrl+C in each terminal window to stop the projects" -ForegroundColor Red