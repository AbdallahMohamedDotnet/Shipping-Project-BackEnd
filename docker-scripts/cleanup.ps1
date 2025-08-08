#!/usr/bin/env pwsh

param(
    [switch]$Volumes,
    [switch]$All,
    [switch]$Force
)

Write-Host "Cleaning up Docker resources for Shipping application..." -ForegroundColor Yellow

$ErrorActionPreference = "Stop"

try {
    # Stop and remove containers
    Write-Host "Stopping containers..." -ForegroundColor Cyan
    docker-compose down
    
    if ($Force) {
        Write-Host "Force removing containers..." -ForegroundColor Cyan
        docker-compose down -v --remove-orphans
    }

    # Remove volumes if requested
    if ($Volumes) {
        Write-Host "Removing volumes..." -ForegroundColor Cyan
        $volumeName = "shipping_sqlserver-data"
        $volume = docker volume ls -q | Where-Object { $_ -eq $volumeName }
        if ($volume) {
            docker volume rm $volumeName
            Write-Host "Removed volume: $volumeName" -ForegroundColor Green
        }
    }

    # Clean up images
    Write-Host "Cleaning up images..." -ForegroundColor Cyan
    
    # Remove project-specific images
    $images = docker images --format "table {{.Repository}}:{{.Tag}}" | Select-String "shipping-"
    if ($images) {
        Write-Host "Removing project images:" -ForegroundColor Yellow
        $images | ForEach-Object {
            Write-Host "  $_" -ForegroundColor White
            docker rmi $_ -f
        }
    }

    if ($All) {
        Write-Host "Running full Docker cleanup..." -ForegroundColor Cyan
        
        # Remove unused images
        docker image prune -f
        
        # Remove unused containers
        docker container prune -f
        
        # Remove unused networks
        docker network prune -f
        
        # Remove unused volumes (if not already done)
        if (-not $Volumes) {
            docker volume prune -f
        }
    }

    Write-Host "Cleanup completed successfully!" -ForegroundColor Green

} catch {
    Write-Error "Cleanup failed: $_"
    exit 1
}