#!/usr/bin/env pwsh

param(
    [Parameter(Mandatory=$true)]
    [string]$Registry,
    [Parameter(Mandatory=$true)]
    [string]$Version,
    [string]$Username,
    [string]$Password,
    [switch]$Login,
    [switch]$Push,
    [switch]$Pull
)

Write-Host "Container Registry Operations for Shipping Application" -ForegroundColor Green

$ErrorActionPreference = "Stop"

# Set image names with registry prefix
$WebApiImage = "$Registry/shipping-webapi:$Version"
$UiImage = "$Registry/shipping-ui:$Version"

try {
    # Login to registry if requested
    if ($Login) {
        Write-Host "Logging into registry: $Registry" -ForegroundColor Yellow
        if ($Username -and $Password) {
            echo $Password | docker login $Registry --username $Username --password-stdin
        } else {
            docker login $Registry
        }
        
        if ($LASTEXITCODE -ne 0) {
            throw "Registry login failed"
        }
        Write-Host "Successfully logged into registry" -ForegroundColor Green
    }

    if ($Push) {
        Write-Host "Tagging and pushing images..." -ForegroundColor Yellow
        
        # Tag local images with registry prefix
        docker tag "shipping-webapi:latest" $WebApiImage
        docker tag "shipping-ui:latest" $UiImage
        
        # Push to registry
        Write-Host "Pushing WebApi image: $WebApiImage" -ForegroundColor Cyan
        docker push $WebApiImage
        
        Write-Host "Pushing UI image: $UiImage" -ForegroundColor Cyan  
        docker push $UiImage
        
        Write-Host "All images pushed successfully!" -ForegroundColor Green
    }

    if ($Pull) {
        Write-Host "Pulling images from registry..." -ForegroundColor Yellow
        
        Write-Host "Pulling WebApi image: $WebApiImage" -ForegroundColor Cyan
        docker pull $WebApiImage
        
        Write-Host "Pulling UI image: $UiImage" -ForegroundColor Cyan
        docker pull $UiImage
        
        # Tag with local names for easier use
        docker tag $WebApiImage "shipping-webapi:$Version"
        docker tag $UiImage "shipping-ui:$Version"
        
        Write-Host "All images pulled successfully!" -ForegroundColor Green
    }

} catch {
    Write-Error "Registry operation failed: $_"
    exit 1
}

Write-Host "`nRegistry images:" -ForegroundColor Cyan
Write-Host "WebApi: $WebApiImage" -ForegroundColor White
Write-Host "UI: $UiImage" -ForegroundColor White