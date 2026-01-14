# Test PostgreSQL Connection
Write-Host "Testing PostgreSQL Connection..." -ForegroundColor Cyan

# Set environment
$env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path","User")

# Find PostgreSQL bin directory
$pgBinPath = "C:\Program Files\PostgreSQL\18\bin"
if (Test-Path $pgBinPath) {
    $env:Path += ";$pgBinPath"
    Write-Host "PostgreSQL bin added to PATH" -ForegroundColor Green
}

# Set PGPASSWORD environment variable for password
$env:PGPASSWORD = "sql123"

# Test connection
Write-Host "`nTesting connection with psql..." -ForegroundColor Yellow
psql -U postgres -h localhost -p 5432 -d postgres -c "SELECT version();" 2>&1

if ($LASTEXITCODE -eq 0) {
    Write-Host "`nConnection successful!" -ForegroundColor Green
    
    # Check if bankingdb exists
    Write-Host "`nChecking for bankingdb database..." -ForegroundColor Yellow
    $dbCheck = psql -U postgres -h localhost -p 5432 -d postgres -t -c "SELECT 1 FROM pg_database WHERE datname='bankingdb';" 2>&1
    
    if ($dbCheck -match "1") {
        Write-Host "Database 'bankingdb' exists!" -ForegroundColor Green
    } else {
        Write-Host "Database 'bankingdb' does NOT exist. Creating it..." -ForegroundColor Yellow
        psql -U postgres -h localhost -p 5432 -d postgres -c "CREATE DATABASE bankingdb;" 2>&1
        if ($LASTEXITCODE -eq 0) {
            Write-Host "Database created successfully!" -ForegroundColor Green
        }
    }
} else {
    Write-Host "`nConnection failed. Error details above." -ForegroundColor Red
    Write-Host "Common issues:" -ForegroundColor Yellow
    Write-Host "1. PostgreSQL service not running"
    Write-Host "2. Wrong password"
    Write-Host "3. pg_hba.conf not configured for local connections"
    Write-Host "`nCheck pg_hba.conf file and ensure it has this line:"
    Write-Host "host    all             all             127.0.0.1/32            scram-sha-256" -ForegroundColor Cyan
}

# Clean up
Remove-Item Env:\PGPASSWORD
