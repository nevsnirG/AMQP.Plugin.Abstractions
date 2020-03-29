using System;

namespace AMQP.Plugin
{
    /// <summary>
    /// A connection to a message broker.
    /// </summary>
    public interface IConnection : IDisposable
    {
        /// <summary>
        /// Register a publisher to the message broker.
        /// </summary>
        /// <param name="routingKey">The routing key for the publisher.</param>
        /// <returns>The registered <see cref="IPublisher"/> instance.</returns>
        IPublisher CreatePublisher(string routingKey);

        /// <summary>
        /// Register a consumer to the message broker.
        /// </summary>
        /// <param name="routingKey">The routing key for the consumer.</param>
        /// <returns>The registered <see cref="IConsumer"/> instance.</returns>
        IConsumer CreateConsumer(string routingKey);
    }
}