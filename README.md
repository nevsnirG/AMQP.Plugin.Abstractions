# AMQP.Plugin.Abstractions
Plugin framework for AMQP implementations.

Available on NuGet:

<a href="https://www.nuget.org/packages/nevsnirG.AMQP.Plugin/">
   <img src="https://img.shields.io/badge/nuget-v1.0.2-green" />
</a>

### Implementations
<a href="https://github.com/nevsnirG/AMQP.Plugin.Abstractions/tree/master/src/AMQP.Plugin.RabbitMQ">RabbitMQ</a>

## Introduction
There are many different message queue/event bus implementations for all .NET flavours, but none implement from a generic set of interfaces/classes. This plugin framework provides a set of interfaces defining the AMQP protocol. The plugins act as an abstraction over (existing) AMQP implementations, such as the provided RabbitMQ implementation, making them fit the plugin specifications.
