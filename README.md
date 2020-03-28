# AMQP.Plugin
Plugin framework for AMQP implementations.

## Introduction
There are many different message queue/event bus implementations for all .NET flavours, but none implement from a generic set of interfaces/classes. This plugin framework provides a set of interfaces defining the AMQP protocol. The plugins act as an abstraction over (existing) AMQP implementations, such as the provided RabbitMQ implementation, making them fit the plugin specifications.
