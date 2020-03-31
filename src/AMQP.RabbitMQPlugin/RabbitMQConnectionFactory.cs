using AMQP.Plugin.Abstractions;
using AMQP.Plugin.Abstractions.Exceptions;
using System;

namespace AMQP.RabbitMQPlugin
{
    internal sealed class RabbitMQConnectionFactory : IConnectionFactory
    {
        private readonly RabbitMQ.Client.IConnectionFactory _connectionFactory;

        public RabbitMQConnectionFactory(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            if (Uri.TryCreate(connectionString, UriKind.Absolute, out var connectionUri))
            {
                if (!connectionUri.Scheme.Equals("amqp"))
                    throw new ArgumentException("The specified connection URI has the wrong scheme.", nameof(connectionString));
            }
            else
                throw new ArgumentException("The specified connection string is not a valid URI.", nameof(connectionString));

            _connectionFactory = new RabbitMQ.Client.ConnectionFactory
            {
                Uri = connectionUri
            };
        }

        public RabbitMQConnectionFactory(RabbitMQ.Client.IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public IConnection CreateConnection(string exchange)
        {
            if (string.IsNullOrWhiteSpace(exchange))
                throw new ArgumentNullException(nameof(exchange));

            try
            {
                var connection = _connectionFactory.CreateConnection();
                return new RabbitMQConnection(exchange, connection);
            }
            catch (RabbitMQ.Client.Exceptions.BrokerUnreachableException ex)
            {
                throw new BrokerUnreachableException(ex.Message, ex);
            }
        }
    }
}