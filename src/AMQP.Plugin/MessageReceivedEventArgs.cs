using System;

namespace AMQP.Plugin
{
    /// <summary>
    /// Provides data for the <see cref="OnMessageReceived"/> callback.
    /// </summary>
    public sealed class MessageReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// The body of the message.
        /// </summary>
        public byte[] Body { get; set; }

        /// <summary>
        /// Initialized <see cref="MessageReceivedEventArgs"/> with the specified message data.
        /// </summary>
        /// <param name="body">The body of the message.</param>
        public MessageReceivedEventArgs(byte[] body)
        {
            if (body is null)
                throw new ArgumentNullException(nameof(body));
            else
                Body = body;
        }
    }
}