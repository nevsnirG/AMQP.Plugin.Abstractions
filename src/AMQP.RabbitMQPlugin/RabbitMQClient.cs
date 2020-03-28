﻿using AMQP.Plugin;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace AMQP.RabbitMQPlugin
{
    internal sealed class RabbitMQClient : IConsumer, IPublisher
    {
        private readonly string _exchange;
        private readonly string _routingKey; //routing key
        private readonly object _lock;

        private IModel _model;

        public RabbitMQClient(string exchange, string routingKey, IModel model)
        {
            if (string.IsNullOrWhiteSpace(exchange))
                throw new ArgumentNullException(nameof(exchange));
            if (string.IsNullOrWhiteSpace(routingKey))
                throw new ArgumentNullException(nameof(routingKey));

            _exchange = exchange;
            _routingKey = routingKey;
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _lock = new object();
        }

        public void SendMessage(byte[] body)
        {
            if (body is null || body.Length == 0)
                throw new ArgumentNullException(nameof(body));
            if (_model is null)
                throw new ObjectDisposedException(nameof(_model));
            if (_model.IsClosed)
                throw new InvalidOperationException("The underlying IModel is closed.");

            //TODO - Handle possible exceptions thrown by BasicPublish method.
            _model.BasicPublish(_exchange, _routingKey, false, null, body);
        }

        public void RegisterConsumer(string queue, OnMessageReceivedHandler onMessageReceivedHandler)
        {
            if (onMessageReceivedHandler is null)
                throw new ArgumentNullException(nameof(onMessageReceivedHandler));

            lock (_lock)
            {
                queue = _model.QueueDeclare(queue ?? string.Empty, false, true, true, null).QueueName;
            }

            var consumer = new EventingBasicConsumer(_model);
            consumer.Received += (sender, e) =>
            {
                //TODO - Add other properties from original eventargs.
                var eventArgs = new MessageReceivedEventArgs(e.Body);
                onMessageReceivedHandler.Invoke(this, eventArgs);
            };
            //TODO - Handle possible exceptions thrown by BasicConsume method.
            _model.BasicConsume(queue, false, consumer);
        }

        public void RegisterConsumer(OnMessageReceivedHandler onMessageReceivedEventHandler)
        {
            RegisterConsumer(null, onMessageReceivedEventHandler);
        }

        public void Dispose()
        {
            if (!(_model is null))
            {
                _model.Dispose();
                _model = null;
            }
        }
    }
}