#!/usr/bin/env pwsh

param(
    [string]$Version = "latest",
    [switch]$NoCache,
    [switch]$Push
)

Write-Host "Building Docker images for Shipping application..." -ForegroundColor Green

$ErrorActionPreference = "Stop"

# Set image names
$WebApiImage = "shipping-webapi:$Version"
$UiImage = "shipping-ui:$Version"

try {
    # Build arguments
    $buildArgs = @()
    if ($NoCache) {
        $buildArgs += "--no-cache"
    }

    Write-Host "Building WebApi image: $WebApiImage" -ForegroundColor Yellow
    docker build -f WebApi/Dockerfile -t $WebApiImage @buildArgs .
    
    if ($LASTEXITCODE -ne 0) {
        throw "WebApi build failed"
    }

    Write-Host "Building UI image: $UiImage" -ForegroundColor Yellow
    docker build -f Ui/Dockerfile -t $UiImage @buildArgs .
    
    if ($LASTEXITCODE -ne 0) {
        throw "UI build failed"
    }

    Write-Host "All images built successfully!" -ForegroundColor Green
    
    # Display built images
    Write-Host "`nBuilt images:" -ForegroundColor Cyan
    docker images | Select-String "shipping-"

    # Optional push to registry
    if ($Push) {
        Write-Host "`nPushing images to registry..." -ForegroundColor Yellow
        docker push $WebApiImage
        docker push $UiImage
        Write-Host "Images pushed successfully!" -ForegroundColor Green
    }

} catch {
    Write-Error "Build failed: $_"
    exit 1
}