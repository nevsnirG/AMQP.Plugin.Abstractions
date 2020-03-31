using AMQP.Plugin.Abstractions;
using AMQP.Plugin.Abstractions.Builders;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace AMQP.RabbitMQPlugin.Tests
{
    [ExcludeFromCodeCoverage]
    public class AMQPBuilderExtensionTests
    {
        [Fact]
        public void RegisterRabbitMQ_BuilderNullInput_ArgumentNullException()
        {
            //Arrange
            IAMQPBuilder builder = null;

            //Act
            var action = new Action(() => builder.Build());

            //Assert
            Assert.Throws<ArgumentNullException>(nameof(builder), action);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\n")]
        public void RegisterRabbitMQ_BuilderInvalidConnectionString_ArgumentException(string connectionString)
        {
            //Arrange
            var builderMock = new Mock<IAMQPBuilder>(MockBehavior.Strict);
            builderMock.Setup((builder) => builder.ConnectionString).Returns(connectionString);
            var builder = builderMock.Object;

            //Act
            var action = new Action(() => builder.Build());

            //Assert
            Assert.Throws<ArgumentException>(nameof(builder), action);
        }

        [Fact]
        public void RegisterRabbitMQ_Success()
        {
            //Arrange
            var connectionString = "amqp://localhost:1234";
            var builderMock = new Mock<IAMQPBuilder>(MockBehavior.Loose);
            builderMock.Setup((builder) => builder.ConnectionString).Returns(connectionString);
            builderMock.Setup((builder) => builder.Services).Returns(new ServiceCollection());
            var builder = builderMock.Object;

            //Act
            builder.Build();

            //Assert
            var provider = builder.Services.BuildServiceProvider();
            var connectionFactory = provider.GetService<IConnectionFactory>();
            Assert.NotNull(connectionFactory);
            Assert.IsType<RabbitMQConnectionFactory>(connectionFactory);
        }
    }
}