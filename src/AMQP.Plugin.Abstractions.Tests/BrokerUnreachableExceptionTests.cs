using AMQP.Plugin.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace AMQP.Plugin.Tests
{
    [ExcludeFromCodeCoverage]
    public class BrokerUnreachableExceptionTests
    {
        [Fact]
        public void Constructor_Empty()
        {
            //Arrange
            var expected = "The message broker could not be reached.";

            //Act
            var exception = new BrokerUnreachableException();

            //Assert
            Assert.Equal(expected, exception.Message);
        }

        [Fact]
        public void Constructor_MessageInput()
        {
            //Arrange
            var expected = "test message";

            //Act
            var exception = new BrokerUnreachableException(expected);

            //Assert
            Assert.Equal(expected, exception.Message);
        }

        [Fact]
        public void Constructor_MessageAndExceptionInput()
        {
            //Arrange
            var expectedMessage = "test message";
            var expectedException = new Exception();

            //Act
            var exception = new BrokerUnreachableException(expectedMessage, expectedException);

            //Assert
            Assert.Equal(expectedMessage, exception.Message);
            Assert.Same(expectedException, exception.InnerException);
        }
    }
}