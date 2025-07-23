#!/bin/bash
# Docker Management Script for Shipping Application

case "$1" in
    "start")
        echo "Starting Shipping Application..."
        docker-compose up -d --build
        echo "Application started! Access URLs:"
        echo "  UI: http://localhost:5001"
        echo "  API: http://localhost:5000"
        echo "  Swagger: http://localhost:5000/swagger"
        ;;
    "stop")
        echo "Stopping Shipping Application..."
        docker-compose down
        ;;
    "restart")
        echo "Restarting Shipping Application..."
        docker-compose down
        docker-compose up -d --build
        ;;
    "logs")
        if [ -z "$2" ]; then
            docker-compose logs -f
        else
            docker-compose logs -f "$2"
        fi
        ;;
    "status")
        docker-compose ps
        ;;
    "clean")
        echo "Cleaning up containers and volumes..."
        docker-compose down -v
        docker system prune -f
        ;;
    "build")
        echo "Building containers..."
        docker-compose build
        ;;
    *)
        echo "Usage: $0 {start|stop|restart|logs [service]|status|clean|build}"
        echo ""
        echo "Commands:"
        echo "  start    - Start all services"
        echo "  stop     - Stop all services"
        echo "  restart  - Restart all services"
        echo "  logs     - View logs (optionally specify service: webapi, ui, sqlserver)"
        echo "  status   - Show container status"
        echo "  clean    - Stop containers and remove volumes"
        echo "  build    - Build containers without starting"
        exit 1
        ;;
esac