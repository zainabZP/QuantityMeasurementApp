// // using System.Text;
// // using Microsoft.AspNetCore.Authentication.JwtBearer;
// // using Microsoft.AspNetCore.Identity;
// // using Microsoft.EntityFrameworkCore;
// // using Microsoft.IdentityModel.Tokens;
// // using Microsoft.OpenApi.Models;
// // using Serilog;
// // using QM.BusinessLogic.Interface;
// // using QM.BusinessLogic.Service;
// // using QM.Models.Entities;
// // using QM.Repository.Data;
// // using QM.Repository.Interface;
// // using QM.Repository.Repository;
// // using QuantityMeasurementApi.Middleware;
// // //using QuantityMeasurementApi.Services;
// // using QM.BusinessLogic.Service;
// // namespace QuantityMeasurementApi;

// // public class Program
// // {
// //     public static WebApplication BuildApp(string[] args)
// //     {
// //         Log.Logger = new LoggerConfiguration()
// //             .WriteTo.Console()
// //             .CreateLogger();

// //         var builder = WebApplication.CreateBuilder(args);
// //         builder.Host.UseSerilog();

// //         // ── Database ─────────────────────────────────────────────────────────
// //         builder.Services.AddDbContext<QuantityMeasurementDbContext>(options =>
// //             options.UseSqlServer(
// //                 builder.Configuration.GetConnectionString("DefaultConnection"),
// //                 b => b.MigrationsAssembly("QM.Repository")));

// //         // ── Identity ──────────────────────────────────────────────────────────
// //         builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
// //         {
// //             options.Password.RequireDigit           = true;
// //             options.Password.RequiredLength         = 6;
// //             options.Password.RequireNonAlphanumeric = false;
// //         })
// //         .AddEntityFrameworkStores<QuantityMeasurementDbContext>()
// //         .AddDefaultTokenProviders();

// //         // ── JWT Authentication ────────────────────────────────────────────────
// //         var jwtKey = builder.Configuration["Jwt:Key"]!;
// //         builder.Services.AddAuthentication(options =>
// //         {
// //             options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
// //             options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
// //         })
// //         .AddJwtBearer(options =>
// //         {
// //             options.TokenValidationParameters = new TokenValidationParameters
// //             {
// //                 ValidateIssuer           = true,
// //                 ValidateAudience         = true,
// //                 ValidateLifetime         = true,
// //                 ValidateIssuerSigningKey = true,
// //                 ValidIssuer              = builder.Configuration["Jwt:Issuer"],
// //                 ValidAudience            = builder.Configuration["Jwt:Audience"],
// //                 IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
// //             };
// //         })
// //         .AddGoogle(options =>
// //         {
// //             options.ClientId     = builder.Configuration["Authentication:Google:ClientId"]!;
// //             options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
// //         });

// //         builder.Services.AddAuthorization();

// //         // ── Repository & Service ──────────────────────────────────────────────
// //         builder.Services.AddScoped<IQuantityMeasurementRepository, QuantityMeasurementDatabaseRepository>();
// //         builder.Services.AddScoped<IQuantityMeasurementService,    QuantityMeasurementServiceImpl>();

// //         // ── UC18 Security Services ────────────────────────────────────────────
// //         builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
// //         builder.Services.AddScoped<ICryptoService,   CryptoService>();
// //         builder.Services.AddScoped<IHashService,     HashService>();
// //         builder.Services.AddSingleton<ITokenBlacklistService, TokenBlacklistService>();

// //         builder.Services.AddControllers();

// //         // ── Swagger with JWT Bearer button ────────────────────────────────────
// //         builder.Services.AddEndpointsApiExplorer();
// //         builder.Services.AddSwaggerGen(c =>
// //         {
// //             c.SwaggerDoc("v1", new() { Title = "Quantity Measurement API", Version = "v1" });
// //             c.EnableAnnotations();

// //             c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
// //             {
// //                 Name         = "Authorization",
// //                 Type         = SecuritySchemeType.Http,
// //                 Scheme       = "Bearer",
// //                 BearerFormat = "JWT",
// //                 In           = ParameterLocation.Header,
// //                 Description  = "Enter: Bearer {your token}"
// //             });

// //             c.AddSecurityRequirement(new OpenApiSecurityRequirement
// //             {
// //                 {
// //                     new OpenApiSecurityScheme
// //                     {
// //                         Reference = new OpenApiReference
// //                         {
// //                             Type = ReferenceType.SecurityScheme,
// //                             Id   = "Bearer"
// //                         }
// //                     },
// //                     Array.Empty<string>()
// //                 }
// //             });

// //             var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
// //             var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
// //             if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
// //         });

// //         builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
// //         builder.Services.AddProblemDetails();

// //         var app = builder.Build();

// //         app.UseExceptionHandler();
// //         app.UseSwagger();
// //         app.UseSwaggerUI(c =>
// //         {
// //             c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quantity Measurement API v1");
// //             c.RoutePrefix = "swagger";
// //         });

// //         app.MapGet("/", () => Results.Redirect("/swagger"));

// //         // ── Middleware pipeline — ORDER MATTERS ───────────────────────────────
// //         app.UseAuthentication();
// //         app.UseMiddleware<TokenRevocationMiddleware>();
// //         app.UseAuthorization();

// //         app.MapControllers();

// //         using (var scope = app.Services.CreateScope())
// //         {
// //             var db = scope.ServiceProvider.GetRequiredService<QuantityMeasurementDbContext>();
// //             db.Database.Migrate();
// //         }

// //         return app;
// //     }

// //     public static void Main(string[] args) => BuildApp(args).Run();
// // }



// using System.Text;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.IdentityModel.Tokens;
// using Microsoft.OpenApi.Models;
// using Serilog;
// using QM.BusinessLogic.Interface;
// using QM.BusinessLogic.Service;
// using QM.Models.Entities;
// using QM.Repository.Data;
// using QM.Repository.Interface;
// using QM.Repository.Repository;
// using QuantityMeasurementApi.Middleware;

// namespace QuantityMeasurementApi;

// public class Program
// {
//     public static WebApplication BuildApp(string[] args)
//     {
//         Log.Logger = new LoggerConfiguration()
//             .WriteTo.Console()
//             .CreateLogger();

//         var builder = WebApplication.CreateBuilder(args);
//         builder.Host.UseSerilog();

//         // ── Database ──────────────────────────────────────────────────────────
//         builder.Services.AddDbContext<QuantityMeasurementDbContext>(options =>
//             options.UseSqlServer(
//                 builder.Configuration.GetConnectionString("DefaultConnection"),
//                 b => b.MigrationsAssembly("QM.Repository")));

//         // ── Identity ──────────────────────────────────────────────────────────
//         builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
//         {
//             options.Password.RequireDigit           = true;
//             options.Password.RequiredLength         = 6;
//             options.Password.RequireNonAlphanumeric = false;
//             options.User.RequireUniqueEmail         = true;
//         })
//         .AddEntityFrameworkStores<QuantityMeasurementDbContext>()
//         .AddDefaultTokenProviders();

//         // ── JWT Authentication ────────────────────────────────────────────────
//         var jwtKey = builder.Configuration["Jwt:Key"]
//             ?? throw new InvalidOperationException("Jwt:Key is not configured in appsettings.");

//         var authBuilder = builder.Services.AddAuthentication(options =>
//         {
//             options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//             options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
//         })
//         .AddJwtBearer(options =>
//         {
//             options.TokenValidationParameters = new TokenValidationParameters
//             {
//                 ValidateIssuer           = true,
//                 ValidateAudience         = true,
//                 ValidateLifetime         = true,
//                 ValidateIssuerSigningKey = true,
//                 ValidIssuer              = builder.Configuration["Jwt:Issuer"],
//                 ValidAudience            = builder.Configuration["Jwt:Audience"],
//                 IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
//             };
//         });

//         // ── Google OAuth (only registered if real credentials are configured) ─
//         var googleClientId     = builder.Configuration["Authentication:Google:ClientId"];
//         var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

//         if (!string.IsNullOrWhiteSpace(googleClientId)
//             && !googleClientId.Contains("YOUR_REAL")
//             && !string.IsNullOrWhiteSpace(googleClientSecret)
//             && !googleClientSecret.Contains("YOUR_REAL"))
//         {
//             authBuilder.AddGoogle(options =>
//             {
//                 options.ClientId     = googleClientId;
//                 options.ClientSecret = googleClientSecret;
//                 options.CallbackPath = "/api/v1/auth/google-callback";
//             });
//         }
//         else
//         {
//             Log.Warning("Google OAuth is not configured. " +
//                         "Set Authentication:Google:ClientId and ClientSecret in appsettings.json.");
//         }

//         builder.Services.AddAuthorization();

//         // ── Repository & Service ──────────────────────────────────────────────
//         builder.Services.AddScoped<IQuantityMeasurementRepository, QuantityMeasurementDatabaseRepository>();
//         builder.Services.AddScoped<IQuantityMeasurementService,    QuantityMeasurementServiceImpl>();

//         // ── UC18 Security Services ────────────────────────────────────────────
//         builder.Services.AddScoped<IJwtTokenService,          JwtTokenService>();
//         builder.Services.AddScoped<ICryptoService,            CryptoService>();
//         builder.Services.AddScoped<IHashService,              HashService>();
//         builder.Services.AddSingleton<ITokenBlacklistService, TokenBlacklistService>();

//         builder.Services.AddControllers();

//         // ── Swagger with JWT Bearer button ────────────────────────────────────
//         builder.Services.AddEndpointsApiExplorer();
//         builder.Services.AddSwaggerGen(c =>
//         {
//             c.SwaggerDoc("v1", new() { Title = "Quantity Measurement API", Version = "v1" });
//             c.EnableAnnotations();

//             c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//             {
//                 Name         = "Authorization",
//                 Type         = SecuritySchemeType.Http,
//                 Scheme       = "Bearer",
//                 BearerFormat = "JWT",
//                 In           = ParameterLocation.Header,
//                 Description  = "Enter: Bearer {your token}"
//             });

//             c.AddSecurityRequirement(new OpenApiSecurityRequirement
//             {
//                 {
//                     new OpenApiSecurityScheme
//                     {
//                         Reference = new OpenApiReference
//                         {
//                             Type = ReferenceType.SecurityScheme,
//                             Id   = "Bearer"
//                         }
//                     },
//                     Array.Empty<string>()
//                 }
//             });

//             var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
//             var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
//             if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
//         });

//         builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
//         builder.Services.AddProblemDetails();

//         var app = builder.Build();

//         app.UseExceptionHandler();
//         app.UseSwagger();
//         app.UseSwaggerUI(c =>
//         {
//             c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quantity Measurement API v1");
//             c.RoutePrefix = "swagger";
//         });

//         app.MapGet("/", () => Results.Redirect("/swagger"));

//         // ── Middleware pipeline — ORDER MATTERS ───────────────────────────────
//         app.UseAuthentication();
//         app.UseMiddleware<TokenRevocationMiddleware>();
//         app.UseAuthorization();

//         app.MapControllers();

//         // ── Auto-migrate on startup ───────────────────────────────────────────
//         using (var scope = app.Services.CreateScope())
//         {
//             var db = scope.ServiceProvider.GetRequiredService<QuantityMeasurementDbContext>();
//             db.Database.Migrate();
//         }

//         return app;
//     }

//     public static void Main(string[] args) => BuildApp(args).Run();
// }




using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using QM.BusinessLogic.Interface;
using QM.BusinessLogic.Service;
using QM.Models.Entities;
using QM.Repository.Data;
using QM.Repository.Interface;
using QM.Repository.Repository;
using QuantityMeasurementApi.Middleware;

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
        builder.Services.AddDbContext<QuantityMeasurementDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("QM.Repository")));

        // ── Identity ──────────────────────────────────────────────────────────
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit           = true;
            options.Password.RequiredLength         = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.User.RequireUniqueEmail         = true;
        })
        .AddEntityFrameworkStores<QuantityMeasurementDbContext>()
        .AddDefaultTokenProviders();

        // ── JWT Authentication ────────────────────────────────────────────────
        var jwtKey = builder.Configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("Jwt:Key is not configured in appsettings.");

        var authBuilder = builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
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

        // ── Google OAuth (only registered if real credentials are configured) ─
        var googleClientId     = builder.Configuration["Authentication:Google:ClientId"];
        var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

        if (!string.IsNullOrWhiteSpace(googleClientId)
            && !googleClientId.Contains("YOUR_REAL")
            && !string.IsNullOrWhiteSpace(googleClientSecret)
            && !googleClientSecret.Contains("YOUR_REAL"))
        {
            authBuilder.AddGoogle(options =>
            {
                options.ClientId     = googleClientId;
                options.ClientSecret = googleClientSecret;
                options.CallbackPath = "/api/v1/auth/google-callback";
            });
        }
        else
        {
            Log.Warning("Google OAuth is not configured. " +
                        "Set Authentication:Google:ClientId and ClientSecret in appsettings.json.");
        }

        builder.Services.AddAuthorization();

        // ── Distributed Cache (in-memory — swap for Redis in production) ──────
        builder.Services.AddDistributedMemoryCache();

        // ── Repository Layer registrations ────────────────────────────────────
        builder.Services.AddScoped<IQuantityMeasurementRepository, QuantityMeasurementDatabaseRepository>();
        builder.Services.AddScoped<ITokenBlacklistService,         TokenBlacklistService>();

        // ── Business Logic Layer registrations ────────────────────────────────
        builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();
        builder.Services.AddScoped<IJwtTokenService,            JwtTokenService>();
        builder.Services.AddScoped<ICryptoService,              CryptoService>();
        builder.Services.AddScoped<IHashService,                HashService>();

        builder.Services.AddControllers();

        // ── Swagger with JWT Bearer button ────────────────────────────────────
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

        // ── Middleware pipeline — ORDER MATTERS ───────────────────────────────
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
}