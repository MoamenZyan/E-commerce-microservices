# E-commerce Microservices System

This project is a microservices-based e-commerce application that includes user, product, order, cart, payment, review, and notification services. Each service is independent and interacts with other services through APIs or messages using RabbitMQ.

## Table of Contents

- [Services](#services)
- [Technologies](#technologies)
- [Architecture](#architecture)
- [Features](#features)
- [Usage](#usage)
- [Payment Integration](#payment-integration)
- [Logging](#logging)
- [Message Broker](#message-broker)
- [Consistency with Outbox Pattern](#consistency-with-outbox-pattern)
- [Microservice Architecture Diagram](#microservice-architecture-diagram)
- [Screenshots](#screenshots)
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

## Usage
- Start all services by running them through Visual Studio or using Docker.
- Use Postman or any API testing tool to interact with the services.
- Payment services (PayPal, Stripe) are configured with sandbox credentials for testing purposes.
- Access Kibana logs at http://localhost:5601.

## Payment Integration
- PayPal: Payments are processed via PayPal API. The system supports capturing and authorizing payments.
- Stripe: Stripe is used for processing payments with various cards. The system uses the PaymentIntent API for secure and reliable payments.

## Logging
- RabbitMQ: Used for asynchronous communication between services. For example, when an order is placed, the Payment Service is notified via RabbitMQ to process payment.

## Consistency with Outbox Pattern
- The Outbox Pattern is implemented to ensure consistency between the local database changes and RabbitMQ messages. This ensures no data loss or message duplication during service crashes or network issues.

## Microservice Architecture Diagram
![E-Commerce Microservices Architecture Diagram](https://github.com/user-attachments/assets/a6452794-c5d4-438e-b75b-979f8798612d)

## Screenshots
![Kibana Elastic Search](https://github.com/user-attachments/assets/ad0f3604-e16d-4b07-b4ec-a740fe764155)
![orderConfirmedEmail](https://github.com/user-attachments/assets/8b7cda00-9ecd-43c0-b78a-9e31eae1dd66)
![Stripe Checkout](https://github.com/user-attachments/assets/2d7fdfcc-93ff-4eec-9826-0a3909d86f44)
![productsAddedToCartEmail](https://github.com/user-attachments/assets/6e8e6716-5d5a-4d93-991a-8351d51435ef)
