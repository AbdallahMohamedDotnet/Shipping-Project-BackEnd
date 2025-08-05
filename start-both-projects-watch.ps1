#!/usr/bin/env pwsh
# PowerShell script to run both projects with hot reload (watch mode)

Write-Host "Starting WebApi and UI projects with hot reload..." -ForegroundColor Green

# Start WebApi with watch in background
Start-Process -FilePath "dotnet" -ArgumentList "watch run --project WebApi" -WorkingDirectory $PWD -WindowStyle Normal

# Wait a moment for WebApi to start
Start-Sleep -Seconds 3

# Start UI project with watch
Start-Process -FilePath "dotnet" -ArgumentList "watch run --project Ui" -WorkingDirectory $PWD -WindowStyle Normal

Write-Host "Both projects are starting with hot reload..." -ForegroundColor Yellow
Write-Host "WebApi will be available at: https://localhost:7048" -ForegroundColor Cyan
Write-Host "UI will be available at: https://localhost:7279" -ForegroundColor Cyan
Write-Host "Changes to code will automatically restart the applications" -ForegroundColor Magenta
Write-Host "Press Ctrl+C in each terminal window to stop the projects" -ForegroundColor Red