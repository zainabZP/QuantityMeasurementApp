# Complete PostgreSQL Migration & Deployment Guide

## 🎯 What We've Done So Far ✅
1. ✅ Updated QM.Repository.csproj - Replaced SqlServer NuGet with Npgsql
2. ✅ Updated Program.cs - Changed UseSqlServer() to UseNpgsql()
3. ✅ Updated QuantityMeasurementApi.csproj - Removed SQLite and SqlServer
4. ✅ Updated appsettings.json - PostgreSQL connection string for local testing
5. ✅ Updated appsettings.Production.json - Ready for environment variables

---

## 📋 STEP 5: Setup PostgreSQL Locally (Optional, for testing)

### Option A: Install PostgreSQL Directly
1. Download from: https://www.postgresql.org/download/
2. Install with default settings
3. Username: `postgres` (local) or `quantityapp` (for Render)
4. Password: `postgres` (or what you choose)
5. Port: `5432`
6. Create database: Open pgAdmin and create a database named `QuantityMeasurementDb`

**Note for Render users**: When creating a database on Render, use a different username like `quantityapp` instead of `postgres`

### Option B: Use Docker (Easier)
```powershell
# In PowerShell, run:
docker run --name postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=QuantityMeasurementDb -p 5432:5432 -d postgres

# Verify it's running:
docker ps
```

---

## 📋 STEP 6: Regenerate Migrations

**IMPORTANT**: This deletes old SQL Server migrations and creates new PostgreSQL ones.

### Option A: Use the Setup Script (Easiest)
```powershell
# Open PowerShell in your project directory
cd c:\QuanttityApp\QuantityMeasurementApp

# Run the setup script
.\setup-migrations.bat
```

### Option B: Manual Commands
```powershell
cd c:\QuanttityApp\QuantityMeasurementApp

# 1. Restore NuGet packages
dotnet restore

# 2. Remove old SQL Server migrations
Remove-Item -Path "QM.Repository\Migrations" -Recurse -Force

# 3. Create new PostgreSQL migrations
dotnet ef migrations add InitialPostgresMigration --project QM.Repository --startup-project QuantityMeasurementApi

# 4. Update database (requires PostgreSQL running)
dotnet ef database update --project QM.Repository --startup-project QuantityMeasurementApi
```

**Expected Output:**
```
Build started...
Build succeeded.
Done. To undo this action, use 'ef migrations remove'
Successfully created migration 'InitialPostgresMigration'
Database updated successfully
```

---

## 📋 STEP 7: Test Locally (Optional, Recommended)

Once migrations are applied, test your API:

```powershell
cd c:\QuanttityApp\QuantityMeasurementApp

# Run the API
dotnet run --project QuantityMeasurementApi

# You should see:
# info: Microsoft.Hosting.Lifetime[14]
#       Now listening on: https://localhost:7291
# info: Microsoft.Hosting.Lifetime[0]
#       Application started. Press Ctrl+C to exit.
```

**Test the API:**
- Swagger UI: https://localhost:7291/swagger
- Health Check: https://localhost:7291

If you see any errors, check:
- PostgreSQL is running: `docker ps` (if using Docker)
- Connection string in appsettings.json matches your database
- Database is created

---

## 📋 STEP 8: Commit and Push to GitHub

```powershell
cd c:\QuanttityApp\QuantityMeasurementApp

# Check what files changed
git status

# Add all changes
git add -A

# Commit the changes
git commit -m "Migrate from SQL Server to PostgreSQL

- Replace SqlServer NuGet with Npgsql.EntityFrameworkCore.PostgreSQL
- Update Program.cs to use UseNpgsql()
- Update connection strings for PostgreSQL
- Regenerate migrations for PostgreSQL
- Add Render deployment files
- GitHub Issue: #1"

# Push to GitHub
git push origin main
```

**Expected Output:**
```
Enumerating objects: 15, done.
Counting objects: 100% (15/15), done.
...
To https://github.com/zainabZP/QuantityMeasurementApp.git
   a1b2c3d..e4f5g6h  main -> main
```

---

## 📋 STEP 9: Deploy on Render

### 9.1: Create PostgreSQL Database on Render

1. Go to: https://dashboard.render.com
2. Sign in (or create account)
3. Click **New +** → **PostgreSQL**
4. Configure:
   - **Name**: `quantity-measurement-db`
   - **Database**: `quantity_measurement_app`
   - **User**: `quantityapp` (⭐ NOT 'postgres' - Render doesn't allow it)
   - **Region**: Choose closest to your users
   - **Plan**: Standard (or Free for testing)
5. Click **Create Database**
6. Wait 5-10 minutes for database to initialize
7. Copy the **Internal Database URL** (looks like: `postgresql://quantityapp:xxxxx@dpg-xxxxx.render.internal:5432/quantity_measurement_app`)

### 9.2: Create Web Service (API)

1. Go to: https://dashboard.render.com
2. Click **New +** → **Web Service**
3. Select **Build and deploy from a Git repository**
4. Click **Connect your account** → Authorize Render on GitHub
5. Select repository: **QuantityMeasurementApp**
6. Configure:
   - **Name**: `quantity-measurement-api`
   - **Environment**: Docker
   - **Region**: Same as PostgreSQL database
   - **Plan**: Standard (minimum recommended)
   - **Branch**: `main`
   - **Auto-deploy**: Enable (checkbox)
7. Click **Create Web Service**

### 9.3: Add Environment Variables

1. After web service is created, click on it
2. Go to **Settings** → **Environment**
3. Add these variables:

```
DATABASE_URL = postgresql://quantityapp:PASSWORD@HOST:5432/quantity_measurement_app
ASPNETCORE_ENVIRONMENT = Production
Jwt__Key = YourSuperSecretKeyAtLeast32CharsLong!YourSuperSecretKeyAtLeast32CharsLong!
Jwt__Issuer = QuantityMeasurementApi
Jwt__Audience = QuantityMeasurementApiUsers
Crypto__Key = MyAESEncryptionKey32BytesLongHere
Crypto__IV = MyIVBlock16Bytes!
```

**For DATABASE_URL**: Use the Internal Database URL from PostgreSQL database (it's safer than External URL)

4. Click **Save Changes**

### 9.4: Monitor Deployment

1. Go to **Logs** tab
2. Watch for:
   - Build process (2-3 minutes)
   - Docker image creation (1-2 minutes)
   - Application startup
3. Status will change from "Deploying" to "Live"

### 9.5: Test Deployment

Once "Live", test your API:
- Swagger: `https://your-service-name.onrender.com/swagger`
- Health: `https://your-service-name.onrender.com`

---

## ✅ You're Done!

Your API is now:
- ✅ Using PostgreSQL
- ✅ Deployed on Render
- ✅ Auto-deploying from GitHub
- ✅ Has Swagger documentation
- ✅ Ready for production

---

## 🔧 Troubleshooting

### Build fails on Render

**Issue**: Build fails during Docker build  
**Fix**: Check logs for exact error, common issues:
- Missing NuGet packages - run `dotnet restore` locally
- Project file syntax errors - validate XML
- Missing migrations - ensure migrations folder is in git

```bash
# Check locally
dotnet build
```

### Database connection timeout

**Issue**: Application crashes with "Unable to connect to database"  
**Fix**:
1. Verify DATABASE_URL environment variable is set
2. Wait 5 minutes after creating PostgreSQL database
3. Check database is accepting connections: `SELECT 1;`
4. Ensure migrations ran automatically or manually

### Migrations not found

**Issue**: "Unable to find migration 'InitialPostgresMigration'"  
**Fix**: Ensure migrations folder is committed to git:

```bash
git add QM.Repository/Migrations
git commit -m "Add PostgreSQL migrations"
git push
```

### Swagger not accessible

**Issue**: 404 when accessing `/swagger`  
**Fix**: Check `appsettings.Production.json` has `"Swagger": { "Enabled": false }`  
This is expected in production - Swagger is disabled. To enable:

```json
"Swagger": {
  "Enabled": true
}
```

Then redeploy.

---

## 📊 Monitoring

After deployment, monitor your app:

1. **Logs**: Render dashboard → Logs tab
2. **Performance**: Render dashboard → Metrics tab
3. **Errors**: Check logs for exceptions
4. **Database**: Monitor PostgreSQL usage on Render

---

## 🚀 Next Steps (Optional)

1. Configure custom domain
2. Setup SSL/TLS
3. Configure CORS for frontend
4. Setup continuous integration (GitHub Actions)
5. Add monitoring/alerting
6. Configure Redis for production
