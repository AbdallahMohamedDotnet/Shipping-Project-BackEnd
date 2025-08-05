@echo off
echo Starting WebApi and UI projects...

echo Starting WebApi...
start "WebApi" cmd /k "cd /d %~dp0 && dotnet run --project WebApi"

echo Waiting for WebApi to start...
timeout /t 3 /nobreak > nul

echo Starting UI...
start "UI" cmd /k "cd /d %~dp0 && dotnet run --project Ui"

echo.
echo Both projects are starting...
echo WebApi will be available at: https://localhost:7048
echo UI will be available at: https://localhost:7279
echo.
echo Press any key in each terminal window to stop the projects
pause