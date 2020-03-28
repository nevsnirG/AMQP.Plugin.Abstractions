using System;
using Xunit;

namespace AMQP.Plugin.Tests
{
    public class MessageReceivedEventArgsTests
    {
        [Fact]
        public void Constructor_BodyInput_Success()
        {
            //Arrange
            var body = new byte[0];

            //Act
            var eventArgs = new MessageReceivedEventArgs(body);

            //Assert
            Assert.Equal(body, eventArgs.Body);
        }

        [Fact]
        public void Constructor_NullBodyInput_ArgumentNullException()
        {
            //Arrange
            byte[] body = null;

            try
            {
                //Act
                var eventArgs = new MessageReceivedEventArgs(body);
            }
            catch (ArgumentNullException ex)
            {
                //Assert
                Assert.Equal(nameof(body), ex.ParamName);
            }
        }
    }
}