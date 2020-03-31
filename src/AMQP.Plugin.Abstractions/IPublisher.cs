using System;

namespace AMQP.Plugin
{
    /// <summary>
    /// Responsible for publishing messages to the message broker.
    /// </summary>
    public interface IPublisher : IDisposable
    {
        /// <summary>
        /// Publish a message to the message broker.
        /// </summary>
        /// <param name="body">The body of the message.</param>
        void SendMessage(byte[] body);
    }
}