# ⚡ Quick Render Setup - PostgreSQL Username Fix

## ⚠️ Important: Render PostgreSQL Username Restriction

**Render does NOT allow `postgres` as the database username.**

Use one of these instead:
- ✅ `quantityapp`
- ✅ `appuser`  
- ✅ `quantityuser`
- ✅ `pguser`

---

## 🚀 Quick Setup Steps

### Step 1: Create PostgreSQL Database

1. Go to: https://dashboard.render.com
2. Click **New +** → **PostgreSQL**
3. Fill in form:
   - **Name**: `quantity-measurement-db`
   - **Database**: `quantity_measurement_app`
   - **User**: `quantityapp` ⭐ (NOT postgres)
   - **Password**: Create a strong password (copy it!)
   - **Region**: Choose your region
   - **Plan**: Free or Standard
4. Click **Create Database**
5. **WAIT 5-10 MINUTES** for database to initialize
6. Once created, copy the **Internal Database URL**

### Step 2: Create Web Service

1. Click **New +** → **Web Service**
2. Select **Build and deploy from Git repository**
3. Authorize GitHub and select **QuantityMeasurementApp**
4. Configure:
   - **Name**: `quantity-measurement-api`
   - **Environment**: Docker
   - **Region**: Same as PostgreSQL ⭐
   - **Plan**: Standard
   - **Auto-deploy**: Enable
5. Click **Create Web Service**

### Step 3: Add Environment Variables

In Web Service → **Settings** → **Environment**, add:

```
DATABASE_URL = postgresql://quantityapp:YOUR_PASSWORD@your-db-url.render.internal:5432/quantity_measurement_app

ASPNETCORE_ENVIRONMENT = Production

Jwt__Key = YourSuperSecretKeyAtLeast32CharsLong!YourSuperSecretKeyAtLeast32CharsLong!

Jwt__Issuer = QuantityMeasurementApi

Jwt__Audience = QuantityMeasurementApiUsers

Crypto__Key = MyAESEncryptionKey32BytesLongHere

Crypto__IV = MyIVBlock16Bytes!
```

### Step 4: Monitor Logs

1. Go to **Logs** tab
2. Watch until status changes to "Live" ✅

### Step 5: Test

Once Live:
- Swagger: `https://your-service-name.onrender.com/swagger`
- API: `https://your-service-name.onrender.com`

---

## 🔗 Reference

- Render Dashboard: https://dashboard.render.com
- GitHub Repo: https://github.com/zainabZP/QuantityMeasurementApp
- Full Guide: See `COMPLETE_SETUP_GUIDE.md`

---

## ✅ You're All Set!

Your .NET API is now deployed on Render with PostgreSQL! 🎉
