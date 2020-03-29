using AMQP.Plugin.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AMQP.Plugin.Extensions
{
    /// <summary>
    /// Contains extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Add an AMQP implementation via the builder pattern.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to register the AMQP implementation to.</param>
        /// <param name="configure">The builder action for the AMQP implementation.</param>
        public static void AddAMQP(this IServiceCollection services, Action<IAMQPBuilder> configure)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            if (!(configure is null))
                configure.Invoke(new AMQPBuilder(services));
        }
    }
}