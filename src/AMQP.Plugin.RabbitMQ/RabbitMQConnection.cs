using AMQP.Plugin.Abstractions;
using Rabbit = RabbitMQ.Client;
using System;

namespace AMQP.Plugin.RabbitMQ
{
    internal sealed class RabbitMQConnection : IConnection
    {
        private readonly string _exchange;

        private Rabbit.IConnection _connection;

        public RabbitMQConnection(string exchange, Rabbit.IConnection connection)
        {
            if (string.IsNullOrWhiteSpace(exchange))
                throw new ArgumentNullException(nameof(exchange));

            _exchange = exchange;
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public IConsumer CreateConsumer(string routingKey)
        {
            return CreateClient(routingKey);
        }

        public IPublisher CreatePublisher(string routingKey)
        {
            return CreateClient(routingKey);
        }

        private RabbitMQClient CreateClient(string routingKey)
        {
            if (_connection is null)
                throw new ObjectDisposedException(nameof(_connection));
            //TODO - Handle possible CreateModel exceptions.
            var model = _connection.CreateModel();
            model.ExchangeDeclare(_exchange, Rabbit.ExchangeType.Topic, false, false, null); //TODO - Handle possible exceptions.
            return new RabbitMQClient(_exchange, routingKey, model);
        }

        public void Dispose()
        {
            if (!(_connection is null))
            {
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}