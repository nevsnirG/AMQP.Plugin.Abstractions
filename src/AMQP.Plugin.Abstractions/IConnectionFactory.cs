namespace AMQP.Plugin
{
    /// <summary>
    /// Responsible for establishing connections to the message broker.
    /// </summary>
    public interface IConnectionFactory
    {
        /// <summary>
        /// Create a connection to the message broker.
        /// </summary>
        /// <param name="exchange">The exchange to connect to.</param>
        /// <returns>An <see cref="IConnection"/> instance connected to the message broker.</returns>
        IConnection CreateConnection(string exchange);
    }
}