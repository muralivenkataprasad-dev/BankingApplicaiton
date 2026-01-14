@echo off
REM Banking Application Launcher
REM This script runs the AwesomeGIC Bank application

echo Starting AwesomeGIC Bank Application...
echo.

REM Set the .NET path
set PATH=%PATH%;C:\Program Files\dotnet

REM Navigate to application directory
cd /d "%~dp0"

REM Run the application
dotnet run

REM Keep window open after application exits
cmd /k
