using System;

namespace AMQP.Plugin
{
    public delegate void OnMessageReceivedHandler(object sender, MessageReceivedEventArgs e);

    public interface IConsumer : IDisposable
    {
        void RegisterConsumer(string queue, OnMessageReceivedHandler onMessageReceivedHandler);
        void RegisterConsumer(OnMessageReceivedHandler onMessageReceivedHandler);
    }
}