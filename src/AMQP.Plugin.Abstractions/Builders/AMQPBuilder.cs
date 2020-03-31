using Microsoft.Extensions.DependencyInjection;
using System;

namespace AMQP.Plugin.Builders
{
    internal sealed class AMQPBuilder : IAMQPBuilder
    {
        public string ConnectionString { get; private set; }
        public IServiceCollection Services { get; }

        public AMQPBuilder(IServiceCollection services)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            Services = services;
        }

        public IAMQPBuilder SetConnectionString(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            ConnectionString = connectionString;
            return this;
        }
    }
}