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

            // DB
            var connectionString = "Data Source=WASEEM\\SQLEXPRESS;Initial Catalog=QuantityMeasurementDb;Integrated Security=true;TrustServerCertificate=true;";
            
            services.AddDbContext<QuantityMeasurementDbContext>(options =>
                options.UseSqlServer(connectionString));

            //  ALL AddScoped MOVED HERE
            services.AddScoped<IQuantityMeasurementRepository, QuantityMeasurementDatabaseRepository>();
            services.AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();
            services.AddScoped<QuantityMeasurementController>();

            // Logging
            services.AddLogging(config => config.AddSerilog());

            return services.BuildServiceProvider();
        }
    }
}