using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using QM.BusinessLogic.Interface;
using QM.BusinessLogic.Service;
using QM.Repository.Data;
using QM.Repository.Interface;
using QM.Repository.Repository;
using QuantityMeasurementApp.Controllers;

namespace QuantityMeasurementApp.Config
{
    public static class ServiceConfig
    {
        public static ServiceProvider Configure()
        {
            var services = new ServiceCollection();

            var connectionString =
                "Data Source=WASEEM\\SQLEXPRESS;" +
                "Initial Catalog=QuantityMeasurementDb;" +
                "Integrated Security=true;" +
                "TrustServerCertificate=true;";

            services.AddDbContext<QuantityMeasurementDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IQuantityMeasurementRepository, QuantityMeasurementDatabaseRepository>();
            services.AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();
            services.AddScoped<QuantityMeasurementController>();
            services.AddLogging(config => config.AddSerilog());

            var provider = services.BuildServiceProvider();

            // UC17: auto-create DB schema from EF Core entity model (pure ORM, no raw SQL)
            using var scope = provider.CreateScope();
            var ctx = scope.ServiceProvider
                           .GetRequiredService<QuantityMeasurementDbContext>();
            ctx.Database.EnsureCreated();

            return provider;
        }
    }
}