# Shipping Application - Docker Setup

This document provides instructions for running the Shipping application using Docker containers.

## Prerequisites

- Docker Desktop installed and running
- Docker Compose (included with Docker Desktop)

## Architecture

The application consists of three main services:

1. **SQL Server** - Database service (port 1433)
2. **WebAPI** - RESTful API service (port 5000)
3. **UI** - Razor Pages web application (port 5001)

## Quick Start

### 1. Build and Run All Services

```bash
# Build and start all services
docker-compose up --build

# Or run in detached mode (background)
docker-compose up -d --build
```

### 2. Access the Applications

- **UI Application**: http://localhost:5001
- **Web API**: http://localhost:5000
- **Swagger Documentation**: http://localhost:5000/swagger

### 3. Database Connection

The SQL Server instance will be available at:
- **Server**: localhost,1433
- **Username**: sa
- **Password**: YourStrong@Password123
- **Database**: Shipping

## Development Commands

```bash
# View logs
docker-compose logs

# View logs for specific service
docker-compose logs webapi
docker-compose logs ui
docker-compose logs sqlserver

# Stop all services
docker-compose down

# Stop and remove volumes (clears database)
docker-compose down -v

# Rebuild specific service
docker-compose build webapi
docker-compose build ui

# Scale services (if needed)
docker-compose up --scale webapi=2
```

## Database Migrations

The application will automatically create the database and tables on first run through Entity Framework migrations.

## Environment Variables

### Production Environment
- Set `ASPNETCORE_ENVIRONMENT=Production`
- Uses `appsettings.Production.json` configuration

### Development Environment
- Set `ASPNETCORE_ENVIRONMENT=Development`
- Uses `appsettings.Development.json` configuration

## Troubleshooting

### Common Issues

1. **Port conflicts**: Ensure ports 5000, 5001, and 1433 are not in use
2. **SQL Server startup**: Wait for SQL Server health check to pass before applications start
3. **Connection issues**: Verify the connection string in appsettings.Production.json

### Health Checks

Check if services are running:
```bash
docker-compose ps
```

### Container Logs

If services fail to start:
```bash
docker-compose logs --tail=50 [service-name]
```

## Security Notes

- Change the default SQL Server password in production
- Consider using Docker secrets for sensitive configuration
- Review firewall rules for exposed ports

## File Structure

```
??? docker-compose.yml           # Main composition file
??? docker-compose.override.yml  # Development overrides
??? .dockerignore               # Files to exclude from build
??? WebApi/
?   ??? Dockerfile             # WebAPI container definition
?   ??? appsettings.Production.json
??? Ui/
    ??? Dockerfile             # UI container definition
    ??? appsettings.Production.json
```