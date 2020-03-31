using System;

namespace AMQP.Plugin.Abstractions.Exceptions
{
    /// <summary>
    /// The exception that is thrown when a message broker could not be reached.
    /// </summary>
    [Serializable]
    public sealed class BrokerUnreachableException : Exception
    {
        /// <summary>
        /// Initialize the <see cref="BrokerUnreachableException"/>.
        /// </summary>
        public BrokerUnreachableException() : this("The message broker could not be reached.")
        {
        }

        /// <summary>
        /// Initialize the <see cref="BrokerUnreachableException"/>.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public BrokerUnreachableException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initialize the <see cref="BrokerUnreachableException"/>.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception that occured when establishing a connection.</param>
        public BrokerUnreachableException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}