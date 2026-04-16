using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using QM.BusinessLogic.Interface;
using QM.BusinessLogic.Service;
using QM.Repository.Data;
using QM.Repository.Interface;
using QM.Repository.Repository;
using QuantityMeasurementApi.Middleware;
using Npgsql;

namespace QuantityMeasurementApi;

public class Program
{
    public static WebApplication BuildApp(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseSerilog();

        // ── Database ──────────────────────────────────────────────────────────
        var connectionString = GetConnectionString(builder.Configuration);
        
        builder.Services.AddDbContext<QuantityMeasurementDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                b => b.MigrationsAssembly("QM.Repository")));

        // ── JWT Authentication ────────────────────────────────────────────────
        var jwtKey = builder.Configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("Jwt:Key is not configured in appsettings.");

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer           = true,
                ValidateAudience         = true,
                ValidateLifetime         = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer              = builder.Configuration["Jwt:Issuer"],
                ValidAudience            = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
        });

        builder.Services.AddAuthorization();
        builder.Services.AddHttpContextAccessor();

        // ── CORS ──────────────────────────────────────────────────────────────
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader());
        });

        // ── Distributed Cache (Redis for token blacklist) ─────────────────────
        builder.Services.AddDistributedMemoryCache();

        // ── Repository Layer ──────────────────────────────────────────────────
        builder.Services.AddScoped<IQuantityMeasurementRepository, QuantityMeasurementDatabaseRepository>();
        builder.Services.AddScoped<ITokenBlacklistService,         TokenBlacklistService>();

        // ── Business Logic Layer ──────────────────────────────────────────────
        builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();
        builder.Services.AddScoped<IJwtTokenService,            JwtTokenService>();
        builder.Services.AddScoped<ICryptoService,              CryptoService>();
        builder.Services.AddScoped<IHashService,                HashService>();

        builder.Services.AddControllers();

        // ── Swagger ───────────────────────────────────────────────────────────
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "Quantity Measurement API", Version = "v1" });
            c.EnableAnnotations();

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name         = "Authorization",
                Type         = SecuritySchemeType.Http,
                Scheme       = "Bearer",
                BearerFormat = "JWT",
                In           = ParameterLocation.Header,
                Description  = "Enter: Bearer {your token}"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id   = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
        });

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        var app = builder.Build();

        app.UseExceptionHandler();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quantity Measurement API v1");
            c.RoutePrefix = "swagger";
        });

        app.MapGet("/", () => Results.Redirect("/swagger"));

        app.UseCors("AllowAll");
        app.UseAuthentication();
        app.UseMiddleware<TokenRevocationMiddleware>();
        app.UseAuthorization();

        app.MapControllers();

        // ── Auto-migrate on startup ───────────────────────────────────────────
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<QuantityMeasurementDbContext>();
            db.Database.Migrate();
        }

        return app;
    }

    public static void Main(string[] args) => BuildApp(args).Run();

    private static string GetConnectionString(IConfiguration configuration)
    {
        // 1. Try to get FROM environment variable directly (highest priority)
        var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
        
        // 2. If not found, try from configuration (will pick up substituted ${DATABASE_URL} from render.yaml)
        if (string.IsNullOrEmpty(databaseUrl))
        {
            databaseUrl = configuration.GetConnectionString("DefaultConnection");
        }

        if (string.IsNullOrEmpty(databaseUrl))
        {
            throw new InvalidOperationException("Database connection string not found in DATABASE_URL or ConnectionStrings:DefaultConnection.");
        }

        // 3. Handle URI format (postgresql://...)
        if (databaseUrl.StartsWith("postgres://") || databaseUrl.StartsWith("postgresql://"))
        {
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host     = databaseUri.Host,
                Port     = databaseUri.Port == -1 ? 5432 : databaseUri.Port,
                Database = databaseUri.AbsolutePath.TrimStart('/'),
                Username = userInfo[0],
                Password = userInfo.Length > 1 ? userInfo[1] : string.Empty,
                SslMode  = SslMode.Require,
                TrustServerCertificate = true,
                Timeout = 15 // Connection timeout 15s
            };

            var cs = builder.ToString();
            
            // Log masked connection string for debugging
            Log.Information("Configuring database: Host={Host}, Port={Port}, Database={Database}, User={User}, SSL=Require", 
                builder.Host, builder.Port, builder.Database, builder.Username);
                
            return cs;
        }

        // 4. Return as is if already in standard format
        return databaseUrl;
    }
}