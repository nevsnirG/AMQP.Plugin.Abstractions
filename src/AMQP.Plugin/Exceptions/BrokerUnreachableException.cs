using System;

namespace AMQP.Plugin.Exceptions
{
    [Serializable]
    public sealed class BrokerUnreachableException : Exception
    {
        public BrokerUnreachableException() : this("The message broker could not be reached.")
        {
        }

        public BrokerUnreachableException(string message) : base(message)
        {
        }

        public BrokerUnreachableException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}