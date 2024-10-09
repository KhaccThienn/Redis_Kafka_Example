namespace Remake_Kafka_Example_01.Settings
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseCustomKafkaMessageBus(this WebApplication app)
        {
            app.UseKafkaMessageBus(mess =>
            {
                mess.RunConsumerAsync("0");
                mess.RunConsumerAsync("1");
            });
        }
    }
}
