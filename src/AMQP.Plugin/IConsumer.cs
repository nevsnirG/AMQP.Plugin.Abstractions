using System;

namespace AMQP.Plugin
{
    public delegate void OnMessageReceived(object sender, MessageReceivedEventArgs e);

    public interface IConsumer : IDisposable
    {
        void RegisterConsumer(string queue, OnMessageReceived onMessageReceived);
    }
}