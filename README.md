# Pizza Order Processing System

## Introduction

The Pizza Order Processing System is a robust console application designed to streamline order management for pizzerias. It automates the process of reading order files, validating order data, calculating prices, tracking ingredient requirements, and preparing orders for fulfillment.

The system addresses common challenges in food service order processing by:
- Ensuring accurate order validation before processing
- Automating price calculations including VAT
- Tracking ingredient inventory requirements
- Facilitating efficient order fulfillment through queue integration
- Providing comprehensive reporting for business oversight

This application serves as a central component in the pizza order fulfillment pipeline, connecting customer orders with kitchen preparation and delivery operations.

## Features
- Parses order files (JSON/CSV)
- Validates orders with business rules
- Calculates total prices including VAT
- Determines required ingredients for all orders
- Pushes validated orders to a processing queue
- Generates order summaries and validation reports

## Getting Started

### Prerequisites
- .NET Core 6.0 or later
- Access to file system for reading order files and product data

### Installation
```bash
git clone https://github.com/Sphexdube/Assessment.Pizzeria.Order.Processor.git
cd Assessment.Pizzeria.Order.Processor

dotnet build

**<p style="text-align: center;">Copyright © 2025 Siphesihle Dube</p>**