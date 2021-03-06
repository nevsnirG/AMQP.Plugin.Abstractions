﻿using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Rabbit = RabbitMQ.Client;

namespace AMQP.Plugin.RabbitMQ.Tests
{
    [ExcludeFromCodeCoverage]
    public class RabbitMQConnectionTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\n")]
        public void Constructor_InvalidExchange_ArgumentNullException(string exchange)
        {
            //Act
            var action = new Action(() => new RabbitMQConnection(exchange, null));

            //Assert
            Assert.Throws<ArgumentNullException>(nameof(exchange), action);
        }

        [Fact]
        public void Constructor_NullConnection_ArgumentNullException()
        {
            //Arrange
            string exchange = nameof(exchange);
            Rabbit.IConnection connection = null;

            //Act
            var action = new Action(() => new RabbitMQConnection(exchange, connection));

            //Assert
            Assert.Throws<ArgumentNullException>(nameof(connection), action);
        }

        [Fact]
        public void CreateClient_DisposedConnection_ObjectDisposedException()
        {
            //Arrange
            string exchange = nameof(exchange);
            var connectionMock = new Mock<Rabbit.IConnection>(MockBehavior.Loose);
            var connection = new RabbitMQConnection(exchange, connectionMock.Object);
            string routingKey = nameof(routingKey);

            //Act
            connection.Dispose();
            var action = new Action(() => connection.CreatePublisher(routingKey));

            //Assert
            Assert.Throws<ObjectDisposedException>(action);
        }
    }
}