@echo off
REM PostgreSQL Migration Setup Script
REM This script will regenerate migrations for PostgreSQL

echo.
echo ========================================
echo PostgreSQL Migration Setup
echo ========================================
echo.

REM Navigate to the project root
cd /d c:\QuanttityApp\QuantityMeasurementApp

echo [1/4] Restoring NuGet packages...
dotnet restore
if errorlevel 1 (
    echo ERROR: Failed to restore packages
    pause
    exit /b 1
)

echo.
echo [2/4] Removing old SQL Server migrations...
REM Check if Migrations folder exists
if exist "QM.Repository\Migrations" (
    echo Deleting QM.Repository\Migrations folder...
    rmdir /s /q "QM.Repository\Migrations"
    echo Migrations deleted successfully
) else (
    echo No migrations folder found
)

echo.
echo [3/4] Creating new PostgreSQL migrations...
dotnet ef migrations add InitialPostgresMigration --project QM.Repository --startup-project QuantityMeasurementApi
if errorlevel 1 (
    echo ERROR: Failed to create migrations
    echo Make sure PostgreSQL is running and accessible
    pause
    exit /b 1
)

echo.
echo [4/4] Updating database...
echo.
echo NOTE: This requires PostgreSQL to be running locally
echo Connection string: Host=localhost;Port=5432;Database=QuantityMeasurementDb;Username=postgres;Password=postgres;SSL Mode=Disable;
echo.
pause /p "Press any key to continue with database update (or Ctrl+C to skip)..."

dotnet ef database update --project QM.Repository --startup-project QuantityMeasurementApi
if errorlevel 1 (
    echo ERROR: Failed to update database
    echo Make sure PostgreSQL is running with the correct credentials
    pause
    exit /b 1
)

echo.
echo ========================================
echo SUCCESS! Migrations completed
echo ========================================
echo.
pause
