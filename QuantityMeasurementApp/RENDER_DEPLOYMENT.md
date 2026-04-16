# Render Deployment Guide for Quantity Measurement API

## Prerequisites
- GitHub account with your repository pushed
- Render account (https://render.com)
- Required configuration values (JWT keys, encryption keys, database URLs)

## Step-by-Step Deployment Instructions

### Step 1: Prepare Your PostgreSQL Database
You can use any PostgreSQL provider:
- **Render PostgreSQL** (easiest - managed by Render)
- **Azure Database for PostgreSQL**
- **AWS RDS PostgreSQL**
- **Railway PostgreSQL** (simple alternative)
- **Neon** (serverless PostgreSQL)

After creating your PostgreSQL database:
1. Run Entity Framework migrations in the cloud
2. Get your connection string in format:
   ```
   Host=<hostname>;Port=5432;Database=<database>;Username=<username>;Password=<password>;SSL Mode=Require;
   ```
   Or Render's format:
   ```
   postgresql://<username>:<password>@<hostname>:<port>/<database>
   ```

### Step 2: Prepare Redis (Optional)
If you need Redis caching:
- Use **Redis Cloud** (free tier available)
- Get your Redis URL in format: `redis://default:<password>@<host>:<port>`

### Step 3: Gather Required Environment Variables
Collect the following:
- `DATABASE_URL` - Your SQL Server connection string
- `REDIS_URL` - Your Redis URL (or leave empty to disable Redis)
- `JWT_KEY` - Must be at least 32 characters long
- `JWT_ISSUER` - e.g., "QuantityMeasurementApi"
- `JWT_AUDIENCE` - e.g., "QuantityMeasurementApiUsers"
- `CRYPTO_KEY` - 32 bytes base64 encoded encryption key
- `CRYPTO_IV` - 16 bytes base64 encoded IV

### Step 4: Commit and Push Files to GitHub
```bash
git add Dockerfile render.yaml .dockerignore QuantityMeasurementApi/appsettings.Production.json
git commit -m "Add Render deployment configuration"
git push origin main
```

### Step 5: Create PostgreSQL Database (Optional: Use Render's Managed Service)

1. Go to https://dashboard.render.com
2. Click **New +** → **PostgreSQL**
3. Configure:
   - **Name**: quantity-measurement-db
   - **Database**: quantity_measurement_app
   - **User**: postgres
   - **Region**: Same as your web service
   - **Plan**: Standard (or Free for testing)
4. Copy the internal database URL (you'll use this for DATABASE_URL)

### Step 6: Create Web Service on Render

1. Go to https://dashboard.render.com
2. Click **New +** → **Web Service**
3. Select **Build and deploy from a Git repository**
4. Connect your GitHub repository (authorize Render if needed)
5. Select the **QuantityMeasurementApp** repository
6. Configure the service:
   - **Name**: quantity-measurement-api
   - **Environment**: Docker
   - **Region**: Same as PostgreSQL database
   - **Plan**: Standard or higher (Starter plan has limitations)
   - **Auto-deploy**: Enable

### Step 7: Add Environment Variables

After creating the service, go to **Settings** → **Environment** and add:

| Key | Value |
|-----|-------|
| `DATABASE_URL` | Your PostgreSQL connection string (from Render or your provider) |
| `REDIS_URL` | Your Redis connection URL (optional) |
| `JWT_KEY` | Your JWT secret key (32+ chars) |
| `JWT_ISSUER` | QuantityMeasurementApi |
| `JWT_AUDIENCE` | QuantityMeasurementApiUsers |
| `CRYPTO_KEY` | Your encryption key |
| `CRYPTO_IV` | Your encryption IV |
| `ASPNETCORE_ENVIRONMENT` | Production |

### Step 8: Deploy

1. Click **Deploy** button (or it auto-deploys if enabled)
2. Monitor the build logs - it will:
   - Clone your repository
   - Build the Docker image
   - Run the container
3. Once "Live" status shows, your API is deployed!

## Required Code Changes: SQL Server → PostgreSQL

Your `Program.cs` currently uses SQL Server. Update it to use PostgreSQL:

### Step 1: Update QM.Repository.csproj
Replace:
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.0" />
```

With:
```xml
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="10.0.0" />
```

### Step 2: Update Program.cs

Change this line:
```csharp
options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("QM.Repository"));
```

To:
```csharp
options.UseNpgsql(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("QM.Repository"));
```

### Step 3: Update appsettings.json (for local testing)
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=QuantityMeasurementDb;Username=postgres;Password=postgres;SSL Mode=Disable;"
}
```

### Step 4: Regenerate Migrations

Since PostgreSQL has different SQL syntax, you need to recreate migrations:

```bash
# Remove old SQL Server migrations
rm -r QM.Repository/Migrations

# Create new PostgreSQL migrations
dotnet ef migrations add InitialMigration --project QM.Repository

# Test locally
dotnet ef database update --project QM.Repository
```

## Handling Database Migrations on Render

Enable automatic migrations in `Program.cs`:
```csharp
// In Program.cs, after building the app
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<QuantityMeasurementDbContext>();
    db.Database.Migrate(); // Runs migrations automatically
}
app.Run();
```

Or SSH into Render container and run manually:
```bash
dotnet ef database update --project QM.Repository
```

## Important Notes

1. **PostgreSQL**: Uses `UseNpgsql()` instead of `UseSqlServer()`
2. **Connection String Format**: PostgreSQL uses `Host=`, `Port=`, `Database=`, `Username=`, `Password=` parameters
3. **Migrations**: Must be regenerated for PostgreSQL - SQL Server migrations won't work
4. **Redis**: Optional - if not needed, remove Redis connection or use in-memory cache
5. **Startup Time**: First deployment takes 5-10 minutes. Subsequent deployments are faster
6. **Logs**: Monitor in Render dashboard under **Settings** → **Logs**
7. **Custom Domain**: Add domain in **Settings** → **Custom Domain**
8. **CORS**: Configure in your Program.cs for your Render domain
9. **Health Checks**: Render will check `http://localhost:8080` by default

## Testing Your Deployment

Once deployed, test with:
```bash
curl https://your-render-domain.onrender.com/api/health
curl https://your-render-domain.onrender.com/swagger/index.html  # (if Swagger enabled)
```

## Troubleshooting

**Build fails:**
- Check Logs in Render dashboard
- Ensure all project references are correct
- Verify Dockerfile path is correct

**App crashes after deploy:**
- Check environment variables are set correctly
- Verify database connection string is valid
- Check database migrations ran successfully

**Connection timeout:**
- Verify DATABASE_URL is correct
- Verify SSL Mode setting (usually `Require` for cloud, `Disable` for local)
- For Render PostgreSQL: connection happens automatically
- For external provider: check firewall allows Render's IP

**Performance issues:**
- Upgrade Render plan
- Optimize database queries
- Consider adding Render middleware for caching

**PostgreSQL-specific issues:**
- If app crashes, check migrations were created for PostgreSQL (not SQL Server)
- Verify Npgsql package is installed: `dotnet list package | grep Npgsql`
- Connection string format: `Host=<host>;Port=5432;Database=<db>;Username=<user>;Password=<pass>;SSL Mode=Require;`
