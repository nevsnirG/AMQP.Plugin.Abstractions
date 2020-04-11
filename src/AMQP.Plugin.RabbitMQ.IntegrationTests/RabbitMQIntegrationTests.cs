using AMQP.Plugin.Abstractions;
using AMQP.Plugin.Abstractions.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Xunit;

namespace AMQP.Plugin.RabbitMQ.IntegrationTests
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "SkipWhenLiveUnitTesting")]
    public class RabbitMQIntegrationTests
    {
        private const string CONNECTION_STRING = "amqp://guest:guest@localhost:5672/";

        [Fact]
        public void Connect_Success()
        {
            //Arrange
            var connectionFactory = new RabbitMQConnectionFactory(CONNECTION_STRING);

            //Act
            using var connection = connectionFactory.CreateConnection(nameof(IntegrationTests));

            //Assert
            Assert.NotNull(connection);
        }

        [Fact]
        public void CanSendAndReceiveMessage()
        {
            //Arrange
            const string routingKey = nameof(routingKey);
            var connectionFactory = new RabbitMQConnectionFactory(CONNECTION_STRING);
            using var connection = connectionFactory.CreateConnection(nameof(IntegrationTests));
            using var consumer = connection.CreateConsumer(routingKey);
            using var publisher = connection.CreatePublisher(routingKey);
            var expected = new byte[3] { 0x01, 0x02, 0x03 };
            byte[] actual = null;
            var mre = new ManualResetEvent(false);
            void onMessageReceived(object sender, MessageReceivedEventArgs e)
            {
                actual = e?.Body;
                mre.Set();
            }

            //Act
            consumer.RegisterConsumer(onMessageReceived);
            publisher.SendMessage(expected);
            var flagged = mre.WaitOne(Timeout.Infinite);

            //Assert
            Assert.True(flagged);
            Assert.Equal(expected, actual);
        }
    }
}