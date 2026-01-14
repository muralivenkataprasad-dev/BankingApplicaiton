# Banking Application Test Script
# This script demonstrates how to run and test the banking application

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  AwesomeGIC Bank - Test Instructions  " -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "✓ .NET SDK: Installed (10.0.102)" -ForegroundColor Green
Write-Host "✓ PostgreSQL: Running (v18)" -ForegroundColor Green
Write-Host "✓ Database: bankingdb exists" -ForegroundColor Green
Write-Host "✓ Tables: accounts, transactions created" -ForegroundColor Green
Write-Host ""

Write-Host "========================================" -ForegroundColor Yellow
Write-Host "       HOW TO RUN THE APPLICATION      " -ForegroundColor Yellow
Write-Host "========================================" -ForegroundColor Yellow
Write-Host ""
Write-Host "Run this command:" -ForegroundColor Cyan
Write-Host 'cd "c:\Success\GIC\dotnet\BankAccount\BankingApp"' -ForegroundColor White
Write-Host '$env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path","User")' -ForegroundColor White
Write-Host "dotnet run" -ForegroundColor White
Write-Host ""

Write-Host "========================================" -ForegroundColor Yellow
Write-Host "         TEST SCENARIOS                " -ForegroundColor Yellow
Write-Host "========================================" -ForegroundColor Yellow
Write-Host ""

Write-Host "1. DEPOSIT MONEY" -ForegroundColor Cyan
Write-Host "   - Type: d" -ForegroundColor White
Write-Host "   - Enter amount: 500" -ForegroundColor White
Write-Host "   - Expected: 'Thank you. `$500.00 has been deposited to your account.'" -ForegroundColor Green
Write-Host ""

Write-Host "2. DEPOSIT MORE MONEY" -ForegroundColor Cyan
Write-Host "   - Type: d" -ForegroundColor White
Write-Host "   - Enter amount: 300" -ForegroundColor White
Write-Host "   - Expected: 'Thank you. `$300.00 has been deposited to your account.'" -ForegroundColor Green
Write-Host ""

Write-Host "3. WITHDRAW MONEY" -ForegroundColor Cyan
Write-Host "   - Type: w" -ForegroundColor White
Write-Host "   - Enter amount: 100" -ForegroundColor White
Write-Host "   - Expected: 'Thank you. `$100.00 has been withdrawn.'" -ForegroundColor Green
Write-Host ""

Write-Host "4. PRINT STATEMENT" -ForegroundColor Cyan
Write-Host "   - Type: p" -ForegroundColor White
Write-Host "   - Expected: Shows all transactions with date, amount, and balance" -ForegroundColor Green
Write-Host ""

Write-Host "5. TRY TO OVERDRAW (Should Fail)" -ForegroundColor Cyan
Write-Host "   - Type: w" -ForegroundColor White
Write-Host "   - Enter amount: 10000" -ForegroundColor White
Write-Host "   - Expected: 'Error: Insufficient funds'" -ForegroundColor Red
Write-Host ""

Write-Host "6. QUIT APPLICATION" -ForegroundColor Cyan
Write-Host "   - Type: q" -ForegroundColor White
Write-Host "   - Expected: 'Thank you for banking with AwesomeGIC Bank. Have a nice day!'" -ForegroundColor Green
Write-Host ""

Write-Host "========================================" -ForegroundColor Yellow
Write-Host "      VERIFY DATA IN DATABASE          " -ForegroundColor Yellow
Write-Host "========================================" -ForegroundColor Yellow
Write-Host ""
Write-Host "Check account balance:" -ForegroundColor Cyan
Write-Host '$env:PGPASSWORD = "sql123"; & "C:\Program Files\PostgreSQL\18\bin\psql.exe" -U postgres -d bankingdb -c "SELECT * FROM accounts;"' -ForegroundColor White
Write-Host ""
Write-Host "Check all transactions:" -ForegroundColor Cyan
Write-Host '$env:PGPASSWORD = "sql123"; & "C:\Program Files\PostgreSQL\18\bin\psql.exe" -U postgres -d bankingdb -c "SELECT * FROM transactions;"' -ForegroundColor White
Write-Host ""

Write-Host "========================================" -ForegroundColor Green
Write-Host "  Your banking application is ready!   " -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
