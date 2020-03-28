using System;

namespace AMQP.Plugin
{
    public sealed class MessageReceivedEventArgs : EventArgs
    {
        public byte[] Body { get; set; }

        public MessageReceivedEventArgs(byte[] body)
        {
            if (body is null)
                throw new ArgumentNullException(nameof(body));
            else
                Body = body;
        }
    }
}