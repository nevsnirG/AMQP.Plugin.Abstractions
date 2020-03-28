using System;

namespace AMQP.Plugin
{
    public interface IConnection : IDisposable
    {
        IPublisher CreatePublisher(string routingKey);

        IConsumer CreateConsumer(string routingKey);
    }
}