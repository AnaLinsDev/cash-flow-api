# Cash Flow API 

A RESTful Web API built with **C# and ASP.NET Core** for managing expenses.

This project was developed **primarily to understand how to manage database and entities using dependency injection extensions**, while also exploring concepts such as business rules, input validation, exception handling, and global exception filter.

## Technologies

- C#
- .NET
- ASP.NET Core Web API
- Swagger / OpenAPI
- Visual Studio
- PostgreSQL

## Features

- List all expenses
- Get a expense by ID
- Create expense
- Update a expense
- Delete a expense
- Business rule validation
- Status and Priority validation
- HTTP status code handling
- Exception filter

## Layered Architecture

For this project, the following layered architecture was adopted:

1. CashFlow.API — The entry point of the application, responsible for handling HTTP requests through the controllers.
2. CashFlow.Application — The application/service layer, responsible for implementing the application's use cases and business rules.
3. CashFlow.Communication — The DTO layer, responsible for defining and organizing the request and response objects used to communicate between the API and the application layer.
4. CashFlow.Exception — The Exception layer, responsible for defining and organizing the aplication errors and saving the resourse message errors.
5. CashFlow.Infrastructure — The Infrastructure layer, responsible for defining and organizing the database communication.
6. CashFlow.Domain — The Domain layer, responsible for defining the entities and interfaces used in the Infrastructure layer.

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/expenses` | Get all expenses |
| GET | `/api/expenses/{id}` | Get a expense by ID |
| POST | `/api/expenses` | Create a expense |
| PUT | `/api/expenses/{id}` | Update a expense |
| DELETE | `/api/expenses/{id}` | Delete a expense |


## What I Practiced

Through this project, I practiced:

- Layered architecture in .NET
- ASP.NET Core Web API
- RESTful API design
- HTTP methods and status codes
- Business rule implementation
- Exception handling
- Debugging with Visual Studio
- API documentation with Swagger
- Implement dependency injection

## Next Steps

- Add reports (Excel and PDF)
- Implement unit tests
- Add authentication
- Add authorization for specific endpoints
