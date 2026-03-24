using Serilog;

namespace QuantityMeasurementApp.Config
{
    public static class LoggingConfig
    {
        public static void Configure()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}