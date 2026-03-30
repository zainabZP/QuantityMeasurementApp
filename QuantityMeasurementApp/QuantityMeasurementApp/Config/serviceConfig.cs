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
            var services = new ServiceCollection(); // returns a collection that configure service rules, currently only has blueprints i.e classes. in simple words it is A collection where you register services (blueprints)

            // DB
            var connectionString = "Data Source=WASEEM\\SQLEXPRESS;Initial Catalog=QuantityMeasurementDb;Integrated Security=true;TrustServerCertificate=true;";
            
            //AddDbContext Register my QuantityMeasurementDbContext in the service collection.
            // options is a parameter of type: DbContextOptionsBuilder.
            // DbContextOptionsBuilder, here options is an obj of DbContextOptionsBuilder class.
            // DbContextOptionsBuilder is a built-in class from Entity Framework Core. It lets you set things like: Database type (SQL Server, SQLite, etc.), Connection string, Logging, etc.
            services.AddDbContext<QuantityMeasurementDbContext>(options =>      
                options.UseSqlServer(connectionString));

            // AddScoped is an extension method for IServiceCollection (which ServiceCollection implements)
            // AddScoped register service blueprint to service collection with scoped life time.
            // In scoped lifetime a new obj of sevice gets created for every new req and used repeatedly for that req.
            services.AddScoped<IQuantityMeasurementRepository, QuantityMeasurementDatabaseRepository>(); // this means whenever IQuantityMeasurementRepository interface is requested anywherem, then give obj of QuantityMeasurementDatabaseRepository class.
            services.AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();
            services.AddScoped<QuantityMeasurementController>(); // this means whenever QuantityMeasurementController class is requested anywhere, then give obj of QuantityMeasurementController.

            // Logging
            // AddLogging is an extension method for IServiceCollection
            // AddLogging used to register logging service to ServiceCollection
            // config is type of ILoggingBuilder. ILoggingBuilder used to set up logging rules
            services.AddLogging(config => config.AddSerilog()); // in Serilog log is of json type.
            //Serilog does structured logging, meaning:
            // It stores data as named properties (fields)
            // Not just as plain text

            // BuildServiceProvider() is an extension method of IServiceCollection. services.BuildServiceProvider() connverts ServiceCollection to DI containtainer (ServiceProvider) that creates and inject object whenever necessary
            return services.BuildServiceProvider();
        }
    }
}