namespace AMQP.Plugin.Extensions
{
    public static class IConsumerExtension
    {
        public static void RegisterConsumer(this IConsumer consumer, OnMessageReceivedHandler onMessageReceivedHandler)
        {
            consumer.RegisterConsumer(null, onMessageReceivedHandler);
        }
    }
}