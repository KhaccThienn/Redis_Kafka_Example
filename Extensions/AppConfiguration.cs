namespace Remake_Kafka_Example_01.Extensions
{
    public static class AppConfiguration
    {
        public static IConfiguration Configuration { get; private set; }

        public static AppSetting LoadAppSettings(IConfiguration configuration)
        {
            Configuration = configuration;
            return AppSetting.MapValues(configuration);
        }
    }
}
