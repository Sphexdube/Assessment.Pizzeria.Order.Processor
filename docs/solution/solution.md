# Solution

## Introduction
The Pizza Order Processing System is a console application responsible for processing order files, validating order data, calculating prices, determining ingredient requirements, and preparing orders for fulfillment. The system reads orders from input files (JSON or CSV), processes them according to business rules, and pushes valid orders to a message queue for further processing.

### Processing an Order
When an order file is received, the system processes each order entry, validates it against business rules, calculates pricing, and determines ingredient requirements. Valid orders are then pushed to a message queue for kitchen preparation, while a summary report is displayed in the console.

![Order Processing Flow](/docs/solution/diagrams/flow/order-processing-flow.png)

## Business Rules
The system enforces several business rules during order validation:

1. **Order Completeness** - Orders must contain all required fields (OrderId, ProductId, Quantity, DeliverAt, CreatedAt, CustomerAddress)
2. **Delivery Time** - DeliverAt must be in the future
3. **Product Existence** - ProductId must exist in the product catalog
4. **Quantity Validation** - Quantity must be positive and reasonable (e.g., < 100 per product)
5. **Address Validation** - Customer address must be non-empty and in a reasonable format

## Solution Design
### High-level Architecture
#### Technologies
1. C# / .NET Core
2. Microsoft SQL Server (optional for persistent storage)
3. RabbitMQ (message queue)
4. JSON/CSV File Processing
5. Unit Testing Framework
6. Structured Logging

#### Guiding Principles
1. [SOLID Principles](https://www.geeksforgeeks.org/ood-principles-solid/)
   * [Single Responsibility](https://www.geeksforgeeks.org/single-responsibility-in-solid-design-principle/)
   * [Open/Closed principle](https://www.geeksforgeeks.org/open-closed-design-principle-in-java/)
   * [Liskov Substitution](https://www.javacodegeeks.com/2018/02/solid-principles-liskov-substitution-principle.html)
   * [Interface Segregation](https://www.javacodegeeks.com/2018/02/solid-principles-interface-segregation-principle.html)
   * [Dependency Inversion](https://www.geeksforgeeks.org/dependecy-inversion-principle-solid/)
2. [Repository Pattern](https://www.geeksforgeeks.org/repository-design-pattern/) (for data access)
3. [Strategy Pattern](https://refactoring.guru/design-patterns/strategy) (for file parsing)
4. [Factory Pattern](https://www.geeksforgeeks.org/factory-method-design-pattern-in-java/) (for creating validators)
5. [Command Pattern](https://refactoring.guru/design-patterns/command) (for order processing)

These patterns were selected to enable:
* Modularity and testability
* Easy extension for new file formats or validation rules
* Clear separation of concerns
* Simplified maintenance and future enhancements

#### Diagrams

### Solution Architecture
![Solution Architecture](/docs/solution/diagrams/flow/solution-architecture.png)