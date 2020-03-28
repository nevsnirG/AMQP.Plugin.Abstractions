using AMQP.Plugin;
using AMQP.Plugin.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AMQP.RabbitMQPlugin
{
    public static class IAMQPBuilderExtension
    {
        public static void RegisterRabbitMQ(this IAMQPBuilder builder)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(builder.ConnectionString))
                throw new ArgumentException("The AMQPBuilder does not have a valid connection string.", nameof(builder));

            builder.Services.AddTransient<IConnectionFactory>((sp) => new RabbitMQConnectionFactory(builder.ConnectionString));
        }
    }
}