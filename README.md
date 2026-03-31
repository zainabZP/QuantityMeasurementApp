# UC18 - Quantity Measurement Application with JWT Authentication & Security

A multi-layered .NET application for quantity measurement operations (Length, Weight, Temperature, Volume) with a secured REST API powered by **ASP.NET Core Identity**, **JWT Bearer Authentication**, **AES-256 Encryption**, and **BCrypt/SHA Hashing**.

---

## 📁 Project Structure

```
CurrUc18/
│
├── QM.BusinessLogic/                  # Business Logic Layer
│   ├── Interface/
│   │   ├── IMeasurable.cs             # Generic measurable interface with arithmetic support
│   │   ├── IQuantityMeasurementService.cs  # Service contract (Compare, Convert, Add, Subtract, Divide)
│   │   └── ITokenBlacklistService.cs  # JWT token revocation interface
│   └── Service/
│       ├── ArithmeticOperation.cs     # Core arithmetic operations
│       ├── ConversionService.cs       # Unit conversion logic
│       ├── CryptoService.cs           # AES-256 Encryption/Decryption
│       ├── HashService.cs             # SHA-256, SHA-512, BCrypt hashing
│       ├── JwtTokenService.cs         # JWT token generation
│       ├── LengthService.cs           # Length measurement service
│       ├── LengthUnitExtensions.cs    # Length unit helper extensions
│       ├── Quantity.cs                # Core quantity model
│       ├── QuantityLength.cs          # Length-specific quantity
│       ├── QuantityMeasurementServiceImpl.cs  # Main service implementation
│       ├── QuantityService.cs         # Quantity utility service
│       ├── TemperatureUnitExtensions.cs
│       ├── TokenBlacklistService.cs   # In-memory token revocation
│       ├── VolumeUnitExtensions.cs
│       ├── WeightService.cs
│       └── WeightUnitExtensions.cs
│
├── QM.Models/                         # Models Layer
│   ├── DTOs/
│   │   ├── AuthDTOs.cs                # Register, Login, AuthResponse, Encrypt/Hash DTOs
│   │   ├── QuantityDTO.cs             # Quantity data transfer object
│   │   ├── QuantityInputDTO.cs        # Input wrapper for two quantities
│   │   └── UserProfileDTO.cs         # User profile response DTO
│   ├── Entities/
│   │   ├── ApplicationUser.cs         # ASP.NET Identity user entity
│   │   └── QuantityMeasurementEntity.cs  # Database entity for measurements
│   ├── Enums/
│   │   ├── ArithmeticOperation.cs
│   │   ├── LengthUnit.cs
│   │   ├── TemperatureUnit.cs
│   │   ├── VolumeUnit.cs
│   │   └── WeightUnit.cs
│   ├── Exceptions/
│   │   ├── DatabaseException.cs
│   │   └── QuantityMeasurementException.cs
│   └── Models/
│       ├── Feet.cs
│       ├── Inch.cs
│       ├── QuantityModel.cs
│       └── QuantityWeight.cs
│
├── QM.Repository/                     # Data Access Layer
│   ├── Context/
│   │   └── QuantityMeasurementDbContext.cs   # EF Core + Identity DbContext
│   ├── Interface/
│   │   └── IQuantityMeasurementRepository.cs
│   ├── Migrations/                    # EF Core Migrations
│   └── Repository/
│       ├── QuantityMeasurementCacheRepository.cs   # In-memory/JSON cache repo
│       └── QuantityMeasurementDatabaseRepository.cs # SQL Server repo
│
├── QuantityMeasurementApi/            # ASP.NET Core Web API
│   ├── Config/
│   │   └── ApiHost.cs
│   ├── Controllers/
│   │   ├── AuthController.cs          # Register & Login endpoints
│   │   ├── QuantityMeasurementApiController.cs  # Core measurement endpoints
│   │   ├── SecurityController.cs      # Encrypt/Decrypt/Hash endpoints
│   │   └── UserController.cs          # User profile management
│   ├── Middleware/
│   │   ├── GlobalExceptionHandler.cs  # Centralized error handling
│   │   └── TokenRevocationMiddleware.cs  # Checks revoked JWT tokens
│   ├── Program.cs                     # App entry point & DI setup
│   ├── appsettings.json               # Configuration (JWT, DB, etc.)
│   └── Properties/
│       └── launchSettings.json
│
├── QuantityMeasurementApp/            # Console Application
│   ├── Config/
│   │   ├── loggerConfig.cs
│   │   └── serviceConfig.cs
│   ├── Controllers/
│   │   └── QuantityMeasurementController.cs
│   ├── Menus/
│   │   ├── LegacyMenu.cs              # Old console UI
│   │   ├── MainMenu.cs                # Main menu router
│   │   └── NTierMenu.cs               # N-Tier architecture menu
│   └── Program.cs
│
└── QuantityMeasurementAppTests/       # Test Project
    ├── Integration/
    │   └── QuantityMeasurementIntegrationTests.cs
    ├── Repository/
    │   └── QuantityMeasurementDatabaseRepositoryTests.cs
    ├── Service/
    │   └── QuantityMeasurementServiceTests.cs
    └── UC3 - UC16 test files
```

---

## 🚀 Features

### Core Measurement Operations
- **Compare** two quantities across unit types
- **Convert** a quantity to a target unit
- **Add / Subtract / Divide** quantities with unit awareness

### Supported Measurement Types
| Type | Units |
|---|---|
| Length | Feet, Inch, Yard, Centimeter |
| Weight | Gram, Kilogram, Pound |
| Temperature | Celsius, Fahrenheit, Kelvin |
| Volume | Litre, Millilitre, Gallon |

### UC18 - Security Features (New)
- **JWT Bearer Authentication** — register, login, receive token
- **Token Revocation** — logout invalidates token via in-memory blacklist
- **AES-256 Encryption/Decryption** — encrypt and decrypt any string
- **Hashing** — SHA-256, SHA-512, and BCrypt with verify support
- **ASP.NET Core Identity** — full user management (register, login, profile update, delete)
- **Protected Endpoints** — all measurement and security APIs require `[Authorize]`

---

## 🔌 API Endpoints

### Authentication — `/api/v1/auth`
| Method | Endpoint | Description |
|---|---|---|
| POST | `/register` | Register a new user |
| POST | `/login` | Login and receive JWT token |
| POST | `/logout` | Revoke current token |

### Quantity Measurements — `/api/v1/quantities` *(Requires JWT)*
| Method | Endpoint | Description |
|---|---|---|
| POST | `/compare` | Compare two quantities |
| POST | `/convert` | Convert to target unit |
| POST | `/add` | Add two quantities |
| POST | `/subtract` | Subtract second from first |
| POST | `/divide` | Divide first by second |
| GET | `/all` | Get all measurement records |
| GET | `/history/operation/{type}` | Filter history by operation type |
| GET | `/history/type/{type}` | Filter history by measurement type |
| GET | `/history/errored` | Get all errored records |
| GET | `/count/{operationType}` | Count operations by type |

### Security — `/api/v1/security` *(Requires JWT)*
| Method | Endpoint | Description |
|---|---|---|
| POST | `/encrypt` | AES-256 encrypt plaintext |
| POST | `/decrypt` | AES-256 decrypt ciphertext |
| POST | `/hash/sha256` | Hash with SHA-256 |
| POST | `/hash/sha512` | Hash with SHA-512 |
| POST | `/hash/bcrypt` | Hash password with BCrypt |
| POST | `/hash/verify/bcrypt` | Verify BCrypt hash |
| POST | `/hash/verify/sha256` | Verify SHA-256 hash |

### User Management — `/api/v1/users` *(Requires JWT)*
| Method | Endpoint | Description |
|---|---|---|
| GET | `/me` | Get current user profile |
| PUT | `/me` | Update username or password |
| DELETE | `/me` | Delete current user account |

---

## ⚙️ Setup & Configuration

### Prerequisites
- .NET 8 SDK
- SQL Server (or update connection string for another provider)

### Configuration — `appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=QuantityMeasurementDb;Trusted_Connection=True;"
  },
  "Jwt": {
    "Key": "your-secret-key-here",
    "Issuer": "QuantityMeasurementApi",
    "Audience": "QuantityMeasurementClient"
  }
}
```

### Run Migrations
```bash
dotnet ef database update --project QM.Repository --startup-project QuantityMeasurementApi
```

### Run the API
```bash
cd QuantityMeasurementApi
dotnet run
```

Swagger UI will be available at: `https://localhost:{port}/swagger`

---

## 🔐 Authentication Flow

1. **Register** → `POST /api/v1/auth/register` with `{ username, email, password }`
2. **Login** → `POST /api/v1/auth/login` → receives `{ token, tokenType: "Bearer" }`
3. **Use Token** → Add to request header: `Authorization: Bearer {token}`
4. **Logout** → `POST /api/v1/auth/logout` → token is blacklisted server-side

---

## 🧪 Running Tests

```bash
cd QuantityMeasurementAppTests
dotnet test
```

Test coverage includes:
- Unit tests for UC3 through UC16
- Service layer tests
- Repository tests (Database & Cache)
- Integration tests

---

## 🏗️ Architecture

This project follows **N-Tier Architecture** with clear separation of concerns:

```
QuantityMeasurementApi  (Presentation)
        ↓
QM.BusinessLogic        (Business Logic)
        ↓
QM.Repository           (Data Access)
        ↓
QM.Models               (Shared Models/DTOs/Entities)
```
