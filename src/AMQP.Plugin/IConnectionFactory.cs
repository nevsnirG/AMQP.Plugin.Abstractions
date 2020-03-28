namespace AMQP.Plugin
{
    public interface IConnectionFactory
    {
        IConnection CreateConnection(string exchange);
    }
}