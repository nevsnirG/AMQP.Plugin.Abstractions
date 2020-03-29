using AMQP.Plugin;
using AMQP.Plugin.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AMQP.RabbitMQPlugin
{
    /// <summary>
    /// Contains extension methods for the <see cref="IAMQPBuilder"/>.
    /// </summary>
    public static class IAMQPBuilderExtension
    {
        /// <summary>
        /// Register a RabbitMQ message broker implementation. Make sure this is called last.
        /// </summary>
        /// <param name="builder">The <see cref="IAMQPBuilder"/> containing the <see cref="IServiceCollection"/> to register to.</param>
        public static void Build(this IAMQPBuilder builder)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(builder.ConnectionString))
                throw new ArgumentException("The AMQPBuilder does not have a valid connection string.", nameof(builder));

            builder.Services.AddTransient<IConnectionFactory>((sp) => new RabbitMQConnectionFactory(builder.ConnectionString));
        }
    }
}