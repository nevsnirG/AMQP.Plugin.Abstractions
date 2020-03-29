using Microsoft.Extensions.DependencyInjection;

namespace AMQP.Plugin.Builders
{
    /// <summary>
    /// Represents a fluid builder pattern for building and configuring AMQP implementations.
    /// </summary>
    public interface IAMQPBuilder
    {
        /// <summary>
        /// The connection string to use when connecting to the message broker.
        /// </summary>
        string ConnectionString { get; }
        /// <summary>
        /// The service collection to register the AMQP implementation and its dependencies to.
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// Set the connection string for the message broker.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>The <see cref="IAMQPBuilder"/> instance.</returns>
        IAMQPBuilder SetConnectionString(string connectionString);
    }
}