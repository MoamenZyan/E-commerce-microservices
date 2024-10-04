# E-commerce Microservices System

This project is a microservices-based e-commerce application that includes user, product, order, cart, payment, review, and notification services. Each service is independent and interacts with other services through APIs or messages using RabbitMQ.

## Table of Contents

- [Services](#services)
- [Technologies](#technologies)
- [Architecture](#architecture)
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Payment Integration](#payment-integration)
- [Logging](#logging)
- [Message Broker](#message-broker)
- [Consistency with Outbox Pattern](#consistency-with-outbox-pattern)

## Services

- **User Service**: Manages user registration, authentication, and profile.
- **Product Service**: Handles product catalog and inventory.
- **Order Service**: Processes orders, manages status, and integrates with payment.
- **Cart Service**: Manages user shopping carts.
- **Review Service**: Allows users to leave reviews and ratings for products.
- **Notification Service**: Sends notifications for orders and payment status.
- **Payment Service**: Integrates PayPal and Stripe for handling payments.
- **Shared Project**: Contains common libraries and utilities used across services.

Each service has its own database and runs independently.

## Technologies

- **.NET 8 (ASP.NET Core)** for building microservices.
- **RabbitMQ** as a message broker for inter-service communication.
- **Kibana & Elasticsearch** for centralized logging.
- **PayPal & Stripe** for payment processing.
- **SQL Server** for relational databases.
- **Outbox Pattern** to ensure message consistency across services.
- **Docker** for containerization and deployment.

## Architecture

The application follows a microservices architecture with each service handling its own domain logic and data. Communication between services happens either through direct API calls or using RabbitMQ for asynchronous messaging.

- **CQRS** is used in the services for separating read and write operations.
- Each service follows **Clean Architecture** principles, ensuring maintainable and scalable code.

## Features

- User authentication and authorization.
- Product management.
- Shopping cart functionality.
- Order processing with payment integration.
- Review and rating system.
- Notification system.
- Centralized logging using Kibana & Elasticsearch.
- Consistency ensured using the Outbox Pattern and RabbitMQ for reliable message delivery.
