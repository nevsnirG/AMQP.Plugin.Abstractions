using AMQP.Plugin.Abstractions.Builders;
using AMQP.Plugin.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace AMQP.Plugin.Tests
{
    [ExcludeFromCodeCoverage]
    public class IServiceCollectionExtensionTests
    {
        [Fact]
        public void AddAMQP_ServicesNullInput_ArgumentNullException()
        {
            //Arrange
            IServiceCollection services = null;

            //Act
            var action = new Action(() => services.AddAMQP(null));

            //Assert
            Assert.Throws<ArgumentNullException>(nameof(services), action);
        }

        [Fact]
        public void AddAMQP_ActionInput_Executed()
        {
            //Arrange
            var servicesMock = new Mock<IServiceCollection>(MockBehavior.Strict);
            var services = servicesMock.Object;
            var action = new Action<IAMQPBuilder>((builder) =>
            {
                //Assert
                Assert.NotNull(builder);
            });

            //Act
            services.AddAMQP(action);
        }
    }
}