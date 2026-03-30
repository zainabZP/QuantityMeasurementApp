UC17 — Quantity Measurement REST API with N-Tier Architecture & EF Core ORM

Overview
UC17 is a .NET 10 REST API that exposes Quantity Measurement operations (compare, convert, add, subtract, divide) over HTTP using a clean N-Tier architecture.
It uses Entity Framework Core with SQL Server for persistence, Serilog for structured logging, Swagger/OpenAPI for interactive documentation, and a global exception handler for consistent error responses. 
All operations are saved to the database and can be queried by type, measurement category, or error status.

CurrUC17/
│
├── QM.Models/                            # Shared models layer
│   ├── DTOs/
│   │   ├── AuthDTOs.cs
│   │   ├── QuantityDTO.cs
│   │   └── QuantityInputDTO.cs
│   ├── Entities/
│   │   ├── ApplicationUser.cs
│   │   └── QuantityMeasurementEntity.cs
│   ├── Enums/
│   │   ├── ArithmeticOperation.cs
│   │   ├── LengthUnit.cs
│   │   ├── TemperatureUnit.cs
│   │   ├── VolumeUnit.cs
│   │   └── WeightUnit.cs
│   ├── Exceptions/
│   │   ├── DatabaseException.cs
│   │   └── QuantityMeasurementException.cs
│   ├── Models/
│   │   ├── Feet.cs
│   │   ├── Inch.cs
│   │   ├── QuantityModel.cs
│   │   └── QuantityWeight.cs
│   └── QM.Models.csproj
│
├── QM.BusinessLogic/                    # Business logic layer
│   ├── Interface/
│   │   ├── IMeasurable.cs
│   │   └── IQuantityMeasurementService.cs
│   ├── Service/
│   │   ├── ArithmeticOperation.cs
│   │   ├── ConversionService.cs
│   │   ├── LengthService.cs
│   │   ├── LengthUnitExtensions.cs
│   │   ├── Quantity.cs
│   │   ├── QuantityLength.cs
│   │   ├── QuantityMeasurementServiceImpl.cs
│   │   ├── QuantityService.cs
│   │   ├── TemperatureUnitExtensions.cs
│   │   ├── VolumeUnitExtensions.cs
│   │   ├── WeightService.cs
│   │   └── WeightUnitExtensions.cs
│   └── QM.BusinessLogic.csproj
│
├── QM.Repository/                        # Data access layer
│   ├── Data/
│   │   └── QuantityMeasurementDbContext.cs
│   ├── Interface/
│   │   └── IQuantityMeasurementRepository.cs
│   ├── Repository/
│   │   ├── QuantityMeasurementCacheRepository.cs
│   │   └── QuantityMeasurementDatabaseRepository.cs
│   └── QM.Repository.csproj
│
├── QuantityMeasurementApi/               # Web API layer
│   ├── Config/
│   │   └── ApiHost.cs
│   ├── Controllers/
│   │   └── QuantityMeasurementApiController.cs
│   ├── Middleware/
│   │   └── GlobalExceptionHandler.cs
│   ├── Migrations/
│   │   ├── 20260328080738_InitialCreate.cs
│   │   ├── 20260328080738_InitialCreate.Designer.cs
│   │   └── QuantityMeasurementDbContextModelSnapshot.cs
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── appsettings.json
│   ├── Program.cs
│   └── QuantityMeasurementApi.csproj
│
├── QuantityMeasurementApp/               # Console app layer
│   ├── Config/
│   │   ├── loggerConfig.cs
│   │   └── serviceConfig.cs
│   ├── Controllers/
│   │   └── QuantityMeasurementController.cs
│   ├── Menus/
│   │   ├── LegacyMenu.cs
│   │   ├── MainMenu.cs
│   │   └── NTierMenu.cs
│   ├── CacheMeasurements.json
│   ├── Program.cs
│   └── QuantityMeasurementApp.csproj
│
├── QuantityMeasurementAppTests/          # Test project
│   ├── Integration/
│   │   └── QuantityMeasurementIntegrationTests.cs
│   ├── Repository/
│   │   └── QuantityMeasurementDatabaseRepositoryTests.cs
│   ├── Service/
│   │   └── QuantityMeasurementServiceTests.cs
│   ├── UC3_QuantityLengthTests.cs
│   ├── UC4Tests.cs
│   ├── UC6_Tests.cs
│   ├── UC7_Tests.cs
│   ├── UC8_Tests.cs
│   ├── UC9_WeightTests.cs
│   ├── UC10_QuantityTests.cs
│   ├── UC11_VolumeUnitTests.cs
│   ├── UC12_Tests.cs
│   ├── UC13_Tests.cs
│   ├── UC14_TemperatureTests.cs
│   ├── UC15_NTierArchitectureTests.cs
│   ├── UC16_BackwardCompatibility_CacheRepositoryTests.cs
│   └── QuantityMeasurementAppTests.csproj
│
└── QuantityMeasurementApp.slnx

Project Descriptions
QM.Models — Shared library containing all DTOs, entities, enums, exceptions, and domain models used across all layers.
QM.BusinessLogic — Core business logic for all quantity measurement operations including length, weight, volume, and temperature conversions. Contains service interfaces and implementations.
QM.Repository — Data access layer with EF Core DbContext, repository interface, and two implementations — database repository (SQL Server) and cache repository (JSON file).
QuantityMeasurementApi — ASP.NET Core Web API exposing all measurement operations as REST endpoints. Includes Swagger UI, global exception handling, Serilog logging, and auto-migration on startup.
QuantityMeasurementApp — Console application providing a menu-driven interface to interact with the measurement service directly (legacy and N-Tier modes).
QuantityMeasurementAppTests — NUnit test project covering all use cases from UC3 to UC17 including unit tests, repository tests, service tests, and integration tests.

QuantityMeasurementApi  (HTTP Layer)
        │
        ▼
QM.BusinessLogic  (Service Layer)
        │
        ▼
QM.Repository  (Data Access Layer)
        │
        ▼
SQL Server — QuantityMeasurementDb2

The API layer depends on BusinessLogic and Repository via interfaces, keeping all layers loosely coupled. The console app (QuantityMeasurementApp) also depends on BusinessLogic and Repository directly.
