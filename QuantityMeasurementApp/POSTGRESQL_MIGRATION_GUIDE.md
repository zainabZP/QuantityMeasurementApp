# PostgreSQL Migration Guide

This guide shows the exact code changes needed to migrate from SQL Server to PostgreSQL.

## 1. Update QM.Repository.csproj

**File**: `QM.Repository/QM.Repository.csproj`

**Find this:**
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.0" />
```

**Replace with:**
```xml
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="10.0.0" />
```

---

## 2. Update QuantityMeasurementApi.csproj (if it has SqlServer reference)

**File**: `QuantityMeasurementApi/QuantityMeasurementApi.csproj`

Remove or replace any SQL Server references with:
```xml
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="10.0.0" />
```

---

## 3. Update Program.cs

**File**: `QuantityMeasurementApi/Program.cs`

**Find this block (around line 28-31):**
```csharp
// ── Database ──────────────────────────────────────────────────────────
builder.Services.AddDbContext<QuantityMeasurementDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("QM.Repository")));
```

**Replace with:**
```csharp
// ── Database ──────────────────────────────────────────────────────────
builder.Services.AddDbContext<QuantityMeasurementDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("QM.Repository")));
```

---

## 4. Update appsettings.json (Local Testing)

**File**: `QuantityMeasurementApi/appsettings.json`

**Find:**
```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=WASEEM\\SQLEXPRESS;Initial Catalog=QuantityMeasurementDb2;Integrated Security=true;TrustServerCertificate=true;",
  "Redis": "localhost:6379"
}
```

**Replace with:**
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=QuantityMeasurementDb;Username=postgres;Password=postgres;SSL Mode=Disable;",
  "Redis": "localhost:6379"
}
```

---

## 5. Regenerate Migrations

After making the code changes, regenerate the migrations for PostgreSQL:

```bash
# Navigate to project directory
cd c:\QuanttityApp\QuantityMeasurementApp

# Remove old SQL Server migrations
Remove-Item QM.Repository\Migrations -Recurse -Force

# Create new PostgreSQL migration
dotnet ef migrations add InitialPostgresMigration --project QM.Repository

# Test locally (requires local PostgreSQL)
dotnet ef database update --project QM.Repository
```

---

## 6. Update appsettings.Production.json

**File**: `QuantityMeasurementApi/appsettings.Production.json`

Keep it empty - environment variables will provide the connection string:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore.Database.Command": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Swagger": {
    "Title": "Quantity Measurement API",
    "Version": "v1",
    "Enabled": false
  },
  "ConnectionStrings": {
    "DefaultConnection": "",
    "Redis": ""
  },
  "Jwt": {
    "Key": "",
    "Issuer": "QuantityMeasurementApi",
    "Audience": "QuantityMeasurementApiUsers"
  },
  "Crypto": {
    "Key": "",
    "IV": ""
  }
}
```

---

## 7. Add Automatic Migrations on Startup (Optional)

**File**: `QuantityMeasurementApi/Program.cs`

Add this before `app.Run()` to automatically apply migrations:

```csharp
// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<QuantityMeasurementDbContext>();
    try
    {
        db.Database.Migrate();
        Log.Information("Migrations applied successfully");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Failed to apply migrations");
    }
}

app.MapControllers();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// Swagger
var swaggerEnabled = app.Configuration.GetValue<bool>("Swagger:Enabled");
if (swaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quantity API v1"));
}

await app.RunAsync();
```

---

## Summary of Changes

| File | Change |
|------|--------|
| `QM.Repository/QM.Repository.csproj` | Replace SqlServer NuGet with Npgsql.EntityFrameworkCore.PostgreSQL |
| `QuantityMeasurementApi/Program.cs` | Change `UseSqlServer()` to `UseNpgsql()` |
| `QuantityMeasurementApi/appsettings.json` | Update connection string for PostgreSQL format |
| `QM.Repository/Migrations/` | Delete all migrations and regenerate for PostgreSQL |
| `QuantityMeasurementApi/appsettings.Production.json` | Set empty values (use environment variables) |

---

## Testing Locally with PostgreSQL

### Install PostgreSQL
- Download from https://www.postgresql.org/download/
- Or use Docker: `docker run --name postgres -e POSTGRES_PASSWORD=postgres -p 5432:5432 -d postgres`

### Create Database Locally
```sql
CREATE DATABASE "QuantityMeasurementDb";
```

### Run Migrations
```bash
dotnet ef database update --project QM.Repository
```

### Run the API
```bash
dotnet run --project QuantityMeasurementApi
```

---

## Deploy to Render

After making these changes:

1. Commit and push:
   ```bash
   git add -A
   git commit -m "Migrate from SQL Server to PostgreSQL"
   git push origin main
   ```

2. Follow the main deployment guide in `RENDER_DEPLOYMENT.md`

3. The Dockerfile will handle installing PostgreSQL driver via NuGet

---

## Common Issues

### "The configuration for entity type 'X' cannot be used because the CLR type does not match"
- This happens when migrations don't match the database provider
- Solution: Delete migrations folder and regenerate: `dotnet ef migrations add InitialPostgresMigration --project QM.Repository`

### Connection refused
- Verify connection string format: `Host=;Port=5432;Database=;Username=;Password=;SSL Mode=`
- Check PostgreSQL is running
- Verify username/password in connection string

### SSL/TLS errors
- Local: Use `SSL Mode=Disable`
- Production: Use `SSL Mode=Require`

### Host not found errors on Render
- Wait 2-3 minutes after creating PostgreSQL database before deploying API
- The database is still initializing
