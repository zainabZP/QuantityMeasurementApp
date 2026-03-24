using QuantityMeasurementApp.Config;

namespace QuantityMeasurementApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            LoggingConfig.Configure();
            AppInitializer.Initialize();
        }
    }
}