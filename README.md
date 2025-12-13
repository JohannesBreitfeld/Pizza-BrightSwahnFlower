# BrightSwahnFlower - Pizza Ordering System

A microservices-based pizza ordering system built with .NET and Docker.

## Architecture Overview

The system consists of multiple microservices that communicate through an API Gateway and use message-based communication via RabbitMQ.

```mermaid
graph TB
    subgraph "Client Layer"
        Frontend[PizzaFrontend<br/>:5000]
    end

    subgraph "API Gateway"
        Gateway[PizzaGateway<br/>Ocelot<br/>:8080]
    end

    subgraph "Microservices"
        OrderService[OrderService<br/>:8080]
        PizzaInfo[PizzaInformationService<br/>:8080]
        Identity[Identity Service<br/>:8080]
        EmailService[EmailService<br/>PizzaMail<br/>]
    end

    subgraph "Databases"
        CosmosDB[(CosmosDB<br/>:8081)]
        SQLServer[(SQL Server<br/>:1433)]
    end

    subgraph "Cache"
        Redis[(Redis<br/>:6379)]
    end

    subgraph "Message Broker"
        RabbitMQ["RabbitMQ<br/>AMQP :5672<br/>Mgmt :15672"]
    end

    %% Frontend to Gateway
    Frontend -->|HTTP| Gateway

    %% Gateway Routes
    Gateway -->|/api/orders<br/>/api/admin/orders| OrderService
    Gateway -->|/api/identity| Identity
    Gateway -->|/api/pizzas<br/>/api/admin/pizzas| PizzaInfo

    %% Service to Database Connections
    OrderService -->|Read/Write| CosmosDB
    PizzaInfo -->|Read/Write| SQLServer
    Identity -->|Read/Write| SQLServer

    %% Cache Connections
    PizzaInfo -.->|Cache<br/>GetAllPizzas<br/>GetAllIngredients| Redis

    %% RabbitMQ Messaging
    OrderService -.->|Publish Events<br/>order.eventType| RabbitMQ
    RabbitMQ -.->|Subscribe to<br/>order events| EmailService

    %% Styling
    classDef frontend fill:#e1f5ff,stroke:#01579b,stroke-width:2px,color:#01579b
    classDef gateway fill:#fff9c4,stroke:#f57f17,stroke-width:2px,color:#f57f17
    classDef service fill:#c8e6c9,stroke:#2e7d32,stroke-width:2px,color:#2e7d32
    classDef database fill:#f8bbd0,stroke:#c2185b,stroke-width:2px,color:#c2185b
    classDef cache fill:#ffe0b2,stroke:#e65100,stroke-width:2px,color:#e65100
    classDef broker fill:#d1c4e9,stroke:#512da8,stroke-width:2px,color:#512da8

    class Frontend frontend
    class Gateway gateway
    class OrderService,PizzaInfo,Identity,EmailService service
    class CosmosDB,SQLServer database
    class Redis cache
    class RabbitMQ broker
```

### Services

- **PizzaFrontend** (Localhost:5000): Web frontend for the pizza ordering system
- **PizzaGateway** (Port 8080): API Gateway using Ocelot for routing requests to microservices
- **OrderService** (Port 8080): Handles order creation and management, stores data in CosmosDB
- **PizzaInformationService** (Port 8080): Manages pizza and ingredient information, uses SQL Server with Redis caching for GetAllPizzas and GetAllIngredients queries
- **Identity Service** (Port 8080): Handles authentication (login, refresh, logout), uses SQL Server
- **EmailService**: Sends email notifications via RabbitMQ event subscriptions

### Infrastructure

- **SQL Server**: Database for PizzaInformationService and Identity Service
- **CosmosDB Emulator**: NoSQL database for OrderService
- **Redis**: Cache for PizzaInformationService (caches GetAllPizzas and GetAllIngredients queries)
- **RabbitMQ**: Message broker for asynchronous communication between services

### Communication Patterns

**Synchronous (HTTP):**
- Frontend communicates with all services through the API Gateway
- Gateway routes:
  - `/api/orders`, `/api/admin/orders` → OrderService
  - `/api/identity` → Identity Service
  - `/api/pizzas`, `/api/admin/pizzas` → PizzaInformationService

**Asynchronous (RabbitMQ):**
- OrderService publishes order events (e.g., `order.created`) to RabbitMQ
- EmailService subscribes to these events and sends email notifications

## Getting Started

### Prerequisites

- Docker and Docker Compose
- .NET 10.0 SDK (for local development)

### Running with Docker Compose

```bash
docker-compose up --build
```

This will start all services and their dependencies.

### Access Points

- Frontend: http://localhost:5000
- CosmosDB Emulator: https://localhost:8081_explorer/index.html
- RabbitMQ Management UI: http://localhost:15672 (username: guest, password: guest)
