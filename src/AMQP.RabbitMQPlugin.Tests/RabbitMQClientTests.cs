using AMQP.Plugin;
using AMQP.Plugin.Extensions;
using Moq;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Xunit;

namespace AMQP.RabbitMQPlugin.Tests
{
    [ExcludeFromCodeCoverage]
    public class RabbitMQClientTests
    {
        private readonly Mock<IModel> _modelMock;
        private readonly Mock<RabbitMQ.Client.IConnection> _connectionMock;
        private readonly Expression<Action<IModel>> _disposeExpression;
        private readonly Expression<Func<IModel, bool>> _isClosedExpression;
        private readonly Expression<Func<IModel, QueueDeclareOk>> _queueDeclareExpression;
        private readonly Expression<Func<IModel, string>> _basicConsumeExpression;
        private readonly Expression<Func<RabbitMQ.Client.IConnection, IModel>> _createModelExpression;
        private readonly Expression<Action<IModel>> _basicPublishExpression;
        private readonly Expression<Action<IModel>> _queueBindExpression;
        private readonly Expression<Action<IModel>> _exchangeDeclareExpression;

        public RabbitMQClientTests()
        {
            _disposeExpression = (model) => model.Dispose();
            _isClosedExpression = (model) => model.IsClosed;
            _queueDeclareExpression = (model) => model.QueueDeclare(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IDictionary<string, object>>());
            _basicConsumeExpression = (model) => model.BasicConsume(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<IBasicConsumer>());
            _createModelExpression = (connection) => connection.CreateModel();
            _basicPublishExpression = (model) => model.BasicPublish(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<IBasicProperties>(), It.IsAny<byte[]>());
            _queueBindExpression = (model) => model.QueueBind(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>());
            _exchangeDeclareExpression = (model) => model.ExchangeDeclare(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IDictionary<string, object>>());

            _modelMock = new Mock<IModel>(MockBehavior.Strict);
            _modelMock.Setup(_disposeExpression)
                .Verifiable();
            _modelMock.Setup(_queueDeclareExpression)
                .Returns((string q, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments) =>
                {
                    if (string.IsNullOrEmpty(q))
                        q = $"queue-{Guid.NewGuid().ToString("N")}";

                    return new QueueDeclareOk(q, 0, 0);
                })
                .Verifiable();
            _modelMock.Setup(_basicConsumeExpression)
                .Returns("tag")
                .Verifiable();
            _modelMock.Setup(_basicPublishExpression)
                .Verifiable();
            _modelMock.Setup(_queueBindExpression)
                .Verifiable();
            _modelMock.Setup(_exchangeDeclareExpression)
                .Verifiable();

            _connectionMock = new Mock<RabbitMQ.Client.IConnection>(MockBehavior.Strict);
            _connectionMock.Setup(_createModelExpression)
                .Returns(_modelMock.Object)
                .Verifiable();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\n")]
        public void Constructor_InvalidExchange_ArgumentNullException(string exchange)
        {
            //Arrange
            string routingKey = nameof(routingKey);
            IModel model = null;

            //Act
            var action = new Action(() => new RabbitMQClient(exchange, routingKey, model));

            //Assert
            Assert.Throws<ArgumentNullException>(nameof(exchange), action);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\n")]
        public void Constructor_InvalidRoutingKey_ArgumentNullException(string routingKey)
        {
            //Arrange
            string exchange = nameof(exchange);
            IModel model = null;

            //Act
            var action = new Action(() => new RabbitMQClient(exchange, routingKey, model));

            //Assert
            Assert.Throws<ArgumentNullException>(nameof(routingKey), action);
        }

        [Fact]
        public void Constructor_ModelNullInput_ArgumentNullException()
        {
            //Arrange
            string exchange = nameof(exchange);
            string routingKey = nameof(routingKey);
            IModel model = null;

            //Act
            var action = new Action(() => new RabbitMQClient(exchange, routingKey, model));

            //Assert
            Assert.Throws<ArgumentNullException>(nameof(model), action);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(new byte[0])]
        public void SendMessage_InvalidBody_ArgumentNullException(byte[] body)
        {
            //Arrange
            string exchange = nameof(exchange);
            string routingKey = nameof(routingKey);
            var connection = new RabbitMQConnection(exchange, _connectionMock.Object);
            var publisher = connection.CreatePublisher(routingKey);

            //Act
            var action = new Action(() => publisher.SendMessage(body));

            //Assert
            Assert.Throws<ArgumentNullException>(nameof(body), action);
            _connectionMock.Verify(_createModelExpression);
            _modelMock.Verify(_exchangeDeclareExpression);
        }

        [Fact]
        public void SendMessage_ModelClosed_InvalidOperationException()
        {
            //Arrange
            string exchange = nameof(exchange);
            string routingKey = nameof(routingKey);
            _modelMock.Setup(_isClosedExpression)
                .Returns(true)
                .Verifiable();
            var connection = new RabbitMQConnection(exchange, _connectionMock.Object);
            var publisher = connection.CreatePublisher(routingKey);
            var body = new byte[1] { 0x01 };

            //Act
            var action = new Action(() => publisher.SendMessage(body));

            //Assert
            Assert.Throws<InvalidOperationException>(action);
            _connectionMock.Verify(_createModelExpression);
            _modelMock.Verify(_exchangeDeclareExpression);
            _modelMock.Verify((model) => model.IsClosed);
        }

        [Fact]
        public void SendMessage_PublisherDisposed_ObjectDisposedException()
        {
            //Arrange
            string exchange = nameof(exchange);
            string routingKey = nameof(routingKey);
            var connection = new RabbitMQConnection(exchange, _connectionMock.Object);
            var publisher = connection.CreatePublisher(routingKey);
            var body = new byte[1] { 0x01 };

            //Act
            publisher.Dispose();
            var action = new Action(() => publisher.SendMessage(body));

            //Assert
            Assert.Throws<ObjectDisposedException>(action);
            _connectionMock.Verify(_createModelExpression);
            _modelMock.Verify(_exchangeDeclareExpression);
            _modelMock.Verify(_disposeExpression, Times.Once);
        }

        [Fact]
        public void RegisterConsumer_MessageHandlerNullInput_ArgumentNullException()
        {
            //Arrange
            string exchange = nameof(exchange);
            string routingKey = nameof(routingKey);
            var connection = new RabbitMQConnection(exchange, _connectionMock.Object);
            var consumer = connection.CreateConsumer(routingKey);
            OnMessageReceived onMessageReceived = null;

            //Act
            var action = new Action(() => consumer.RegisterConsumer(onMessageReceived));

            //Assert
            Assert.Throws<ArgumentNullException>(nameof(onMessageReceived), action);
            _connectionMock.Verify(_createModelExpression);
            _modelMock.Verify(_exchangeDeclareExpression);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("a")]
        public void RegisterConsumer_EmptyQueueName_Success(string queueName)
        {
            //Arrange
            string exchange = nameof(exchange);
            string routingKey = nameof(routingKey);
            var connection = new RabbitMQConnection(exchange, _connectionMock.Object);
            var consumer = connection.CreateConsumer(routingKey);

            //Act
            consumer.RegisterConsumer(queueName, (s, e) => { });

            //Assert
            _connectionMock.Verify(_createModelExpression);
            _modelMock.Verify(_exchangeDeclareExpression);
            _modelMock.Verify(_queueDeclareExpression);
            _modelMock.Verify(_basicConsumeExpression);
        }

        [Fact]
        public void RegisterConsumer_QueueNameNullInput_Success()
        {
            //Arrange
            string exchange = nameof(exchange);
            string routingKey = nameof(routingKey);
            var connection = new RabbitMQConnection(exchange, _connectionMock.Object);
            var consumer = connection.CreateConsumer(routingKey);

            //Act
            consumer.RegisterConsumer((s, e) => { });

            //Assert
            _connectionMock.Verify(_createModelExpression);
            _modelMock.Verify(_exchangeDeclareExpression);
            _modelMock.Verify(_queueDeclareExpression);
            _modelMock.Verify(_basicConsumeExpression);
        }

        [Fact]
        public void SendMessage_Success()
        {
            //Arrange
            string exchange = nameof(exchange);
            string routingKey = nameof(routingKey);
            _modelMock.Setup(_isClosedExpression)
                .Returns(false)
                .Verifiable();
            var connection = new RabbitMQConnection(exchange, _connectionMock.Object);
            var publisher = connection.CreatePublisher(routingKey);
            var body = new byte[1] { 0x01 };

            //Act
            publisher.SendMessage(body);

            //Assert
            _connectionMock.Verify(_createModelExpression);
            _modelMock.Verify(_exchangeDeclareExpression);
            _modelMock.Verify(_isClosedExpression);
            _modelMock.Verify(_basicPublishExpression);
        }
    }
}