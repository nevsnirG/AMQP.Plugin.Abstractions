using AMQP.Plugin.Abstractions;
using AMQP.Plugin.Abstractions.Exceptions;
using Rabbit = RabbitMQ.Client;
using System;

namespace AMQP.Plugin.RabbitMQ
{
    internal sealed class RabbitMQConnectionFactory : IConnectionFactory
    {
        private readonly Rabbit.IConnectionFactory _connectionFactory;

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

            _connectionFactory = new Rabbit.ConnectionFactory
            {
                Uri = connectionUri
            };
        }

        public RabbitMQConnectionFactory(Rabbit.IConnectionFactory connectionFactory)
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
            catch (Rabbit.Exceptions.BrokerUnreachableException ex)
            {
                throw new BrokerUnreachableException(ex.Message, ex);
            }
        }
    }
}