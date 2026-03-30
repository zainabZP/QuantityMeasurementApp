using Serilog;

namespace QuantityMeasurementApp.Config
{
    public static class LoggingConfig
    {
        public static void Configure()
        {
            Log.Logger = new LoggerConfiguration()  // serilog class constructor , sets up logger configurations i.e rules for logger
                .MinimumLevel.Information()  // serilog extension method that says logger will ignore everything except error and warning
                .WriteTo.Console()  // serilog extension method that says logs will be shown on console i.e terminal
                .CreateLogger(); // serilog method that creates and return logger obj
        }
    }
}