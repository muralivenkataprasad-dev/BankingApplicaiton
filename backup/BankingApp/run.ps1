# Banking Application Launcher (PowerShell)
# This script runs the AwesomeGIC Bank application

Write-Host "Starting AwesomeGIC Bank Application..." -ForegroundColor Cyan
Write-Host ""

# Set the .NET path
$env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path","User")

# Get the script directory
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $scriptDir

# Run the application
try {
    dotnet run
} catch {
    Write-Host "An error occurred: $_" -ForegroundColor Red
    Write-Host ""
    Write-Host "Press any key to exit..." -ForegroundColor Yellow
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
}
