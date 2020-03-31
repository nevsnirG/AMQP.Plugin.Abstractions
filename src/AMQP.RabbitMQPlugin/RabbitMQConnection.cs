using AMQP.Plugin.Abstractions;
using System;

namespace AMQP.RabbitMQPlugin
{
    internal sealed class RabbitMQConnection : IConnection
    {
        private readonly string _exchange;

        private RabbitMQ.Client.IConnection _connection;

        public RabbitMQConnection(string exchange, RabbitMQ.Client.IConnection connection)
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
            model.ExchangeDeclare(_exchange, RabbitMQ.Client.ExchangeType.Topic, false, false, null); //TODO - Handle possible exceptions.
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