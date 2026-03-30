using Microsoft.EntityFrameworkCore;
using Serilog;
using QM.BusinessLogic.Interface;
using QM.BusinessLogic.Service;
using QM.Repository.Data;
using QM.Repository.Interface;
using QM.Repository.Repository;
using QuantityMeasurementApi.Exceptions;

namespace QuantityMeasurementApi
{
    public class Program
    {
        public static WebApplication BuildApp(string[] args)
        {
            // ── Serilog ───────────────────────────────────────────────────────
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog();

            builder.Services.AddDbContext<QuantityMeasurementDbContext>(options =>
            options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly("QuantityMeasurementApi")));

            // ── Repository & Service ──────────────────────────────────────────
            builder.Services.AddScoped<IQuantityMeasurementRepository,
                                       QuantityMeasurementDatabaseRepository>();
            builder.Services.AddScoped<IQuantityMeasurementService,
                                       QuantityMeasurementServiceImpl>();

            // ── Controllers ───────────────────────────────────────────────────
            builder.Services.AddControllers();

            // ── Swagger / OpenAPI ─────────────────────────────────────────────
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() 
                { 
                    Title = "Quantity Measurement API", 
                    Version = "v1" 
                });
                                c.EnableAnnotations(); 
                // Include XML comments if generated
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                    c.IncludeXmlComments(xmlPath);
            });

            // ── Global Exception Handler ──────────────────────────────────────
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            var app = builder.Build();

            // ── Middleware ────────────────────────────────────────────────────
            app.UseExceptionHandler();

            // Enable Swagger in all environments (restrict to dev-only in prod)
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quantity Measurement API v1");
                c.RoutePrefix = "swagger";   // → http://localhost:5000/swagger
            });

            app.MapGet("/", () => Results.Redirect("/swagger"));
            app.MapControllers();

            // Auto-apply migrations on startup
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<QuantityMeasurementDbContext>();
                db.Database.Migrate();
            }

            return app;
        }

        public static void Main(string[] args)
        {
            var app = BuildApp(args);
            app.Run();
        }
    }
}