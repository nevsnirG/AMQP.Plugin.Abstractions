using AMQP.Plugin.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AMQP.Plugin.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddAMQP(this IServiceCollection services, Action<IAMQPBuilder> configure)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            if (!(configure is null))
                configure.Invoke(new AMQPBuilder(services));
        }
    }
}