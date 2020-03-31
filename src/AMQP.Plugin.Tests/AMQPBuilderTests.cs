using AMQP.Plugin.Abstractions.Builders;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace AMQP.Plugin.Tests
{
    [ExcludeFromCodeCoverage]
    public class AMQPBuilderTests
    {
        [Fact]
        public void Constructor_ServicesNullInput_ArgumentNullException()
        {
            //Arrange
            IServiceCollection services = null;

            //Act
            var action = new Action(() => new AMQPBuilder(services));

            //Assert
            Assert.Throws<ArgumentNullException>(nameof(services), action);
        }

        [Fact]
        public void Constructor_Success()
        {
            //Arrange
            var servicesMock = new Mock<IServiceCollection>(MockBehavior.Strict);
            var services = servicesMock.Object;

            //Act
            var builder = new AMQPBuilder(services);

            //Assert
            Assert.NotNull(builder.Services);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData("\t")]
        public void SetConnectionString_WrongInput_ArgumentNullException(string connectionString)
        {
            //Arrange
            var servicesMock = new Mock<IServiceCollection>(MockBehavior.Strict);
            var services = servicesMock.Object;
            var builder = new AMQPBuilder(services);

            //Act
            var action = new Action(() => builder.SetConnectionString(connectionString));

            //Assert
            Assert.Throws<ArgumentNullException>(nameof(connectionString), action);
        }

        [Fact]
        public void SetConnectionString_Success()
        {
            //Arrange
            var servicesMock = new Mock<IServiceCollection>(MockBehavior.Strict);
            var services = servicesMock.Object;
            var builder = new AMQPBuilder(services);
            var connectionString = "non null and whitespace value";

            //Act
            builder.SetConnectionString(connectionString);

            //Assert
            Assert.Equal(connectionString, builder.ConnectionString);
        }
    }
}