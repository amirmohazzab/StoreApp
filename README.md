# StoreApp

StoreApp is a full-stack e-commerce application built with ASP.NET Core 8, Angular, Entity Framework Core, and SQL Server.

The application allows customers to browse products, manage shopping carts, place orders, manage wishlists, and maintain user profiles.
Administrators can manage products, categories, orders, users, and other store-related data through an administration panel.

## Features

### Customer Features

- User Registration and Login
- Authentication using ASP.NET Identity
- Role-based Authorization
- Product Browsing
- Product Search and Filtering
- Product Reviews
- Shopping Cart Management
- Wishlist Management
- Order Placement
- Order History
- User Profile Management
- Address Management

### Administration Features

- Product Management
- Category Management
- Brand Management
- Product Type Management
- Order Management
- User Management
- Permission Management
- Role Management
- Contact Message Management


## Technology Stack

### Backend

- ASP.NET Core 8
- Entity Framework Core
- SQL Server
- ASP.NET Identity
- MediatR
- AutoMapper

### Frontend

- Angular
- TypeScript
- Bootstrap

## Architecture & Design Patterns

- Clean Architecture
- CQRS
- MediatR
- Repository Pattern
- Unit of Work

## Solution Structure

StoreApp

├── Angular (Frontend)
├── Api 
├── Infrastructure (Data) 
├── Application 
└── Domain

### StoreApp.Web

Presentation layer responsible for:

- API Controllers
- Dependency Injection Configuration
- Middleware Configuration
- Authentication & Authorization
- Application Startup Configuration

### StoreApp.Application

Application layer responsible for:

- CQRS Commands and Queries
- MediatR Handlers
- DTOs
- Business Use Cases
- Validation Logic
- Application Interfaces

### StoreApp.Domain

Core domain layer responsible for:

- Entities
- Domain Models
- Business Rules
- Shared Base Entities

### StoreApp.Data

Infrastructure layer responsible for:

- Entity Framework Core
- SQL Server Integration
- ASP.NET Identity
- Repositories
- Unit of Work
- Database Migrations
- Data Access Logic

## Database

The application uses SQL Server and contains approximately 30 database tables covering:

Products
 ├── ProductImages
 ├── ProductReviews
 ├── ProductSizes
 ├── ProductColors
 ├── ProductBrand
 ├── ProductcType
 └── ProductCategory

Users
 ├── User
 ├── UserRole
 ├── UserPermission
 ├── UserLike
 ├── UserWishlist
 ├── Role
 ├── Permission
 ├── RolePermission
 └── Address

Orders
 ├── Order
 ├── OrderItem
 ├── DeliveryMethod
 ├── OrderStatusHistory
 ├── ProductItemOrdered
 ├── Portal
 └── ShipToAddress

 Contacts
 ├── Contact
 ├── ContactConversation
 ├── ContactMessage
 └── ContactAttachment

Costomer
 ├── CostomereBasket
 └── CostomerBasketItem

 Base
 ├── BaseEntity
 └── BaseAuditableEntity

## Key Implementations

- JWT/Identity-based Authentication
- Role-based Authorization
- File Upload Functionality
- Product Pagination
- Product Search and Filtering
- CQRS with MediatR
- Clean Architecture
- Repository Pattern
- Unit of Work

## Deployment

The application is deployed on shared hosting with SQL Server as the primary database.

## Future Improvements

- Redis Caching
- Docker Containerization
- Automated Testing
- CI/CD Pipeline
- Cloud Deployment
