# NTier Project

## Overview
NTier is a .NET Core-based, multi-layered architecture project that integrates powerful Dependency Injection (DI) adaptation using Infrastructure Factories. It includes an advanced logging system, default entity mapping, multi-language SEO support, and many other features to enhance scalability and maintainability.

## Features
- **NTier Architecture** - Ensures modularity and separation of concerns.
- **Dependency Injection via Infrastructure Factories** - Enhances flexibility and maintainability.
- **Built-in Logging System** - Enables efficient tracking and debugging.
- **Base Entity & Default Entity Mapping** - Provides consistency across database models.
- **SEO Helper with Multi-Language Support** - Supports Azerbaijani (az), English (en), Russian (ru), and Turkish (tr).
- **JWT Authentication** - Secures APIs with token-based authentication.
- **Hangfire Background Job Management** - Allows scheduling and managing background tasks.
- **Swagger API Documentation** - Generates interactive API documentation.
- **Localization & Request Culture Management** - Adapts responses based on user language preferences.
- **CORS Support** - Enables secure cross-origin requests.

## Installation & Setup

### 1. Clone the Repository
```sh
 git clone https://github.com/namigabilov/NTier.git
```

### 2. Open the Project in Your IDE
Navigate to the project directory and open it in your preferred IDE (Visual Studio, Rider, VS Code, etc.).

### 3. Update Your ConnectionStrings
Postgresql is enabled in the project. You need to configure the `appsettings.json` file with your "Default" key:
```json
"ConnectionStrings": {
    "Default": "Host=;Port=5432;Username=;Password=;Database=;"
}
```

### 4. Navigate to WebApi Directory
```sh
 cd WebApi
```

### 5. Restore Dependencies
```sh
 dotnet restore
```

### 6. Run Database Migrations
If the project uses Entity Framework Core, apply pending migrations:
```sh
 cd ../DataAccess/
 dotnet ef migrations add Initialcreate
 dotnet ef database update
```

### 7. Run the Application
```sh
 cd ../WebApi/
 dotnet run
```

## Configuration

### Authentication (JWT)
JWT authentication is enabled in the project. You need to configure the `appsettings.json` file with your secret key:
```json
"Token": {
    "SecurityKey": "your-secure-key"
}
```

### Swagger API Documentation
The project includes **Swagger** support with multiple API groups and security configurations:
- Visit `http://localhost:<port>/swagger` in your browser to access the API documentation.
- The public API documentation is available at:
  ```
  /swagger/public/swagger.json
  ```
- Security setup requires passing the JWT token in the **Authorization** header.

### Localization Support
The application supports multiple languages and dynamically adapts the request culture based on user preferences. The supported languages are:
- **Azerbaijani (az)**
- **English (en)**
- **Russian (ru)**
- **Turkish (tr)**

Localization is managed using a custom middleware and integrated into the request pipeline.

### Background Jobs (Hangfire)
- **Hangfire** is configured for scheduling background jobs.
- The dashboard is accessible at:
  ```
  http://localhost:<port>/hangfire
  ```
- Jobs can be scheduled, monitored, and executed directly via the Hangfire dashboard.

## Technologies Used
- **.NET Core** - Backend API development.
- **Entity Framework Core** - ORM for database operations.
- **Hangfire** - Background job processing.
- **JWT Authentication** - Secure API authentication.
- **AutoMapper** - Automatic object mapping.
- **Newtonsoft.Json** - JSON serialization and deserialization.
- **Swagger** - API documentation and testing tool.
- **CORS** - Secure cross-origin resource sharing.

## Contribution
We welcome contributions from the community. To contribute:
1. Fork the repository.
2. Create a new branch (`feature/your-feature`).
3. Commit your changes.
4. Push the branch and create a pull request.

## Troubleshooting & FAQs
### How do I resolve database-related issues?
Ensure that your database is correctly configured in `appsettings.json` and that you have run the migrations:
```sh
 dotnet ef database update
```

### How do I generate a new JWT token?
Use any API testing tool (like Postman) to call the authentication endpoint and retrieve a new token.

### The Swagger UI does not load properly, what should I do?
- Ensure the application is running.
- Navigate to `http://localhost:<port>/swagger`.
- Check logs for any errors.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contact
For any inquiries, feel free to reach out to the repository owner or create an issue on GitHub.

