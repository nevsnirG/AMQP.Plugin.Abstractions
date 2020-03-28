using System;

namespace AMQP.Plugin
{
    public interface IPublisher : IDisposable
    {
        void SendMessage(byte[] body);
    }
}