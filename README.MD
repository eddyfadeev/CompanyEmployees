# Company Employees Application Architecture Overview

Company Employees is an ASP.NET Core Web API application designed to manage companies and their employees. The project is organized into several projects, each with specific responsibilities, ensuring modularity and maintainability. Below is an overview of the key components and functionalities within this project.

## Project Structure Overview

The project follows a layered architecture, which separates concerns into distinct modules:
1. **Presentation Layer (`CompanyEmployees.Presentation`)**: Contains controllers and action filters for handling HTTP requests and responses.
2. **Service Layer (`Service`)**: Implements business logic and interacts with the repository layer.
3. **Repository Layer (`Repository`)**: Manages data access and persistence using Entity Framework Core.
4. **Entities Layer (`Entities`)**: Defines domain models and custom exceptions.
5. **Shared Layer (`Shared`)**: Contains shared DTOs, request features, and utility extensions.
6. **Contracts Layer (`Contracts`)**: Defines interfaces for services and repositories to ensure loose coupling.
7. **LoggerService Layer (`LoggerService`)**: Provides logging functionality using NLog.

## Key Features
- **API Endpoints**: The application exposes RESTful endpoints for managing companies, employees, authentication, and token operations.
- **CRUD Operations**: The API supports creating, reading, updating, and deleting companies and employees.
- **Authentication and Authorization**: Implements authentication using JWT for securing endpoints.
- **Data Shaping**: Allows clients to request only the specific fields they need from API responses using the `DataShaper` class.
- **Pagination and Filtering**: Supports paginated and filtered results through the use of `RequestParameters` and custom repository extensions.
- **Error Handling**: Custom exceptions are defined in the `Entities` layer to handle various business logic errors, and a global exception handler (`GlobalExceptionHandler`) is implemented to return consistent error responses.
- **Logging**: Uses NLog for logging application events and errors, with configuration managed via `nlog.config`.
- **Testing**: Comprehensive unit tests are provided in separate test projects for services, entities, and presentation layers.

## Configuration
- **Configuration Files**:
  - `appsettings.json` and `appsettings.Development.json`: Contain application settings, including connection strings and JWT configuration.
  - `nlog.config`: Configures NLog logging.
  - `launchSettings.json`: Defines profiles for running the application in different environments.