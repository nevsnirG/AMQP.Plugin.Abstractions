using Microsoft.Extensions.DependencyInjection;

namespace AMQP.Plugin.Builders
{
    public interface IAMQPBuilder
    {
        string ConnectionString { get; }
        IServiceCollection Services { get; }

        IAMQPBuilder SetConnectionString(string connectionString);
    }
}