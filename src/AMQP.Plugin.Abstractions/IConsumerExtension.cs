namespace AMQP.Plugin.Abstractions
{
    /// <summary>
    /// Contains overloads for methods inside <see cref="IConsumer"/>.
    /// </summary>
    public static class IConsumerExtension
    {
        /// <summary>
        /// Register a consumer on random generated queue.
        /// </summary>
        /// <param name="consumer">The <see cref="IConsumer"/> instance to register with.</param>
        /// <param name="onMessageReceived">The <see cref="OnMessageReceived"/> callback to invoke when receiving messages.</param>
        public static void RegisterConsumer(this IConsumer consumer, OnMessageReceived onMessageReceived)
        {
            consumer.RegisterConsumer(null, onMessageReceived);
        }
    }
}