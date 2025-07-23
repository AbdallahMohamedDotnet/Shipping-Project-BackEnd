@echo off
REM Docker Management Script for Shipping Application (Windows)

if "%1"=="start" (
    echo Starting Shipping Application...
    docker-compose up -d --build
    echo Application started! Access URLs:
    echo   UI: http://localhost:5001
    echo   API: http://localhost:5000
    echo   Swagger: http://localhost:5000/swagger
    goto :eof
)

if "%1"=="stop" (
    echo Stopping Shipping Application...
    docker-compose down
    goto :eof
)

if "%1"=="restart" (
    echo Restarting Shipping Application...
    docker-compose down
    docker-compose up -d --build
    goto :eof
)

if "%1"=="logs" (
    if "%2"=="" (
        docker-compose logs -f
    ) else (
        docker-compose logs -f %2
    )
    goto :eof
)

if "%1"=="status" (
    docker-compose ps
    goto :eof
)

if "%1"=="clean" (
    echo Cleaning up containers and volumes...
    docker-compose down -v
    docker system prune -f
    goto :eof
)

if "%1"=="build" (
    echo Building containers...
    docker-compose build
    goto :eof
)

echo Usage: %0 {start^|stop^|restart^|logs [service]^|status^|clean^|build}
echo.
echo Commands:
echo   start    - Start all services
echo   stop     - Stop all services
echo   restart  - Restart all services
echo   logs     - View logs (optionally specify service: webapi, ui, sqlserver)
echo   status   - Show container status
echo   clean    - Stop containers and remove volumes
echo   build    - Build containers without starting