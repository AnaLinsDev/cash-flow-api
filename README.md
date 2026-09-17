# Cash Flow API 

A RESTful Web API built with **C# and ASP.NET Core** for managing expenses.

This project was developed **primarily to understand how to manage database and entities using dependency injection extensions and also generating EXCEL and PDF reports**, while also exploring concepts such as business rules, input validation, exception handling, and global exception filter.

![][main-image]

## Technologies

- C#
- .NET
- ASP.NET Core Web API
- Swagger / OpenAPI
- Visual Studio
- PostgreSQL

## Features

- CRUD expenses
- Download Excel report by month
- Download PDF report by month
- Business rule validation
- Status and Priority validation
- Exception filter
- Swagger documentation (HTTP status code handling)

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
| GET | `/api/reports/excel?month=2026-09` | Get expenses by the year-month and download the Excel report  |
| GET | `/api/reports/pdf?month=2026-09` | Get expenses by the year-month and download the PDF report  |
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
- How to use the MigraDoc to create and style a PDF report
- How to use the ClosedXML to create and style an Excel report

## Next Steps

- Create the Migrations
- Implement unit tests
- Add authentication
- Add authorization for specific endpoints

## How to run

### Requirements
* Visual Studio version 2022+ or Visual Studio Code
* Windows 10+ or ​​Linux/MacOS with .NET SDK 8.0 or 9.0 installed
* PostgreSQL

### Installation

1. Clone the repository:

```bash
git clone https://github.com/AnaLinsDev/cash-flow-api.git
```

### 2. Navigate to the project

```bash
cd cash-flow-api
```

### 3. Add your DB_PATH

Fill in the database information in the `appsettings.Development.json` file inside the CashFlow.API.

### 4. Restore dependencies

```bash
dotnet restore
```

### 5. Build the project

```bash
dotnet build
```

### 6. Creating database tables

// Here will be added the steps to run the migrations, but it will be added in the next steps

### 7. Run the API

```bash
dotnet run --project src/CashFlow.API
```

The terminal will display the URL where the API is running.

### 8. Open Swagger

Open the Swagger URL displayed by the application in your browser.

Swagger can be used to test the available API endpoints without requiring Postman or another API client.

<!-- Images -->
[main-image]: images/readme_image.png
