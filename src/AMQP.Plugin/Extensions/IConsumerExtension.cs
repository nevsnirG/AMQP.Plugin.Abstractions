namespace AMQP.Plugin.Extensions
{
    public static class IConsumerExtension
    {
        public static void RegisterConsumer(this IConsumer consumer, OnMessageReceived onMessageReceived)
        {
            consumer.RegisterConsumer(null, onMessageReceived);
        }
    }
}