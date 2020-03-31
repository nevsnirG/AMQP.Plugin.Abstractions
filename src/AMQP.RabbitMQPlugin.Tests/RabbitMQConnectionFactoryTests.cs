using AMQP.Plugin.Abstractions.Exceptions;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace AMQP.RabbitMQPlugin.Tests
{
    [ExcludeFromCodeCoverage]
    public class RabbitMQConnectionFactoryTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\n")]
        public void Constructor_ConnectionStringNullInput_ArgumentNullException(string connectionString)
        {
            //Act
            var action = new Action(() => new RabbitMQConnectionFactory(connectionString));

            //Assert
            Assert.Throws<ArgumentNullException>(nameof(connectionString), action);
        }

        [Theory]
        [InlineData("invalid string")]
        [InlineData("http://validUriWithInvalidScheme/")]
        public void Constructor_InvalidConnectionString_ArgumentException(string connectionString)
        {
            //Act
            var action = new Action(() => new RabbitMQConnectionFactory(connectionString));

            //Assert
            Assert.Throws<ArgumentException>(nameof(connectionString), action);
        }

        [Fact]
        public void Constructor_IConnectionFactoryNullInput_ArgumentNullException()
        {
            //Arrange
            RabbitMQ.Client.IConnectionFactory connectionFactory = null;

            //Act
            var action = new Action(() => new RabbitMQConnectionFactory(connectionFactory));

            //Assert
            Assert.Throws<ArgumentNullException>(nameof(connectionFactory), action);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\n")]
        public void CreateConnection_InvalidExchange_ArgumentNullException(string exchange)
        {
            //Arrange
            var connectionString = "amqp://localhost:1234/";
            var connectionFactory = new RabbitMQConnectionFactory(connectionString);

            //Act
            var action = new Action(() => connectionFactory.CreateConnection(exchange));

            //Assert
            Assert.Throws<ArgumentNullException>(nameof(exchange), action);
        }

        [Fact]
        public void CreateConnection_BrokerUnreachableException()
        {
            //Arrange
            var connectionMock = new Mock<RabbitMQ.Client.IConnection>(MockBehavior.Strict);
            var rabbitMqConnectionFactory = new Mock<RabbitMQ.Client.IConnectionFactory>(MockBehavior.Strict);
            rabbitMqConnectionFactory.Setup((factory) => factory.CreateConnection()).Throws(new RabbitMQ.Client.Exceptions.BrokerUnreachableException(null));
            var connectionFactory = new RabbitMQConnectionFactory(rabbitMqConnectionFactory.Object);

            //Act
            var action = new Action(() => connectionFactory.CreateConnection("test"));

            //Assert
            Assert.Throws<BrokerUnreachableException>(action);
        }

        [Fact]
        public void CreateConnection_Succes()
        {
            //Arrange
            var connectionMock = new Mock<RabbitMQ.Client.IConnection>(MockBehavior.Strict);
            var rabbitMqConnectionFactory = new Mock<RabbitMQ.Client.IConnectionFactory>(MockBehavior.Strict);
            rabbitMqConnectionFactory.Setup((factory) => factory.CreateConnection()).Returns(connectionMock.Object);
            var connectionFactory = new RabbitMQConnectionFactory(rabbitMqConnectionFactory.Object);

            //Act
            var connection = connectionFactory.CreateConnection("test");

            //Assert
            rabbitMqConnectionFactory.Verify((factory) => factory.CreateConnection(), Times.Once);
        }
    }
}