using System;

namespace AMQP.Plugin
{
    /// <summary>
    /// Responsible for handling incoming messages.
    /// </summary>
    /// <param name="sender">The object that invoked the delegate.</param>
    /// <param name="e">The <see cref="MessageReceivedEventArgs"/> containing message data.</param>
    public delegate void OnMessageReceived(object sender, MessageReceivedEventArgs e);

    /// <summary>
    /// Responsible for consuming messages from a message queue.
    /// </summary>
    public interface IConsumer : IDisposable
    {
        /// <summary>
        /// Register a consumer to the message queue.
        /// </summary>
        /// <param name="queue">The queue to consume messages from.</param>
        /// <param name="onMessageReceived">The callback to invoke when a message is consumed.</param>
        void RegisterConsumer(string queue, OnMessageReceived onMessageReceived);
    }
}