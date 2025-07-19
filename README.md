# DNA Testing System - PRN232 Assignment

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core-orange.svg)](https://docs.microsoft.com/en-us/ef/)
[![API](https://img.shields.io/badge/API-REST-green.svg)](https://restfulapi.net/)

## System Description

This system manages DNA testing services for a medical facility, providing both civil and administrative DNA testing capabilities.

### Key Features

#### 🏠 **Public Portal**

- Medical facility introduction and service overview
- DNA testing service information (civil and administrative)
- Knowledge sharing blog with DNA testing guides and experiences
- Educational content and testing procedures

#### 📅 **Appointment Management**

- Online service booking system
- Self-collection or facility-collection options
- Flexible scheduling based on service type

#### 🧬 **Testing Process Workflows**

**Self-Collection Process (Civil DNA Testing):**

```
Registration → Kit Delivery → Sample Collection → Sample Shipping →
Laboratory Testing → Result Recording → Result Delivery
```

**Facility Collection Process:**

```
Registration → Staff Sample Collection → Order Update →
Laboratory Testing → Result Recording → Result Delivery
```

#### 👤 **User Features**

- Online result viewing and access
- User profile management
- Testing history tracking
- Rating and feedback system

#### ⚙️ **Administrative Features**

- Service and pricing configuration
- Order and sample management
- User account administration
- Comprehensive dashboard and reporting

## Technical Architecture

### Project Structure

```
DNATestingSystem/
├── DNATestingSystem.APIServices.BE.TienDM/     # API Layer (.NET 8.0)
├── DNATestingSystem.Services.TienDM/           # Business Logic Layer
└── DNATestingSystem.Repository.TienDM/         # Data Access Layer
```

### Technology Stack

- **Framework:** .NET 8.0
- **Database:** SQL Server with Entity Framework Core
- **Authentication:** JWT Bearer Token
- **API Documentation:** Swagger/OpenAPI
- **Architecture Pattern:** Repository Pattern with Dependency Injection

### Key Components

#### API Layer

- [`AppointmentsTienDMController`](ASSIGNMENT/SU25_PRN232_SE1730_ASM1_TienDM_00035845/DNATestingSystem.APIServices.BE.TienDM/Controllers/AppointmentsTienDMController.cs) - Appointment endpoints
- [`WeatherForecastController`](ASSIGNMENT/SU25_PRN232_SE1730_ASM1_TienDM_00035845/DNATestingSystem.APIServices.BE.TienDM/Controllers/WeatherForecastController.cs) - Sample controller
- [`Program.cs`](ASSIGNMENT/SU25_PRN232_SE1730_ASM1_TienDM_00035845/DNATestingSystem.APIServices.BE.TienDM/Program.cs) - Application startup and configuration

#### Data Layer

- [`SE18_PRN232_SE1730_G3_DNATestingSystemContext`](ASSIGNMENT/SU25_PRN232_SE1730_ASM1_TienDM_00035845/DNATestingSystem.Repository.TienDM/DBContext/SE18_PRN232_SE1730_G3_DNATestingSystemContext.cs) - Entity Framework DbContext
- [`GenericRepository<T>`](ASSIGNMENT/SU25_PRN232_SE1730_ASM1_TienDM_00035845/DNATestingSystem.Repository.TienDM/Basic/GenericRepository.cs) - Generic repository pattern implementation
- [`UserAccountRepository`](ASSIGNMENT/SU25_PRN232_SE1730_ASM1_TienDM_00035845/DNATestingSystem.Repository.TienDM/UserAccountRepository.cs) - User account data access

#### Service Layer Dependencies

- `IAppointmentsTienDmService` & `AppointmentsTienDmService`
- `ISystemUserAccountService` & `SystemUserAccountService`

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository

```bash
git clone <repository-url>
cd PRN232_TienHover_Assignement
```

2. Navigate to the API project

```bash
cd ASSIGNMENT/SU25_PRN232_SE1730_ASM1_TienDM_00035845/DNATestingSystem.APIServices.BE.TienDM
```

3. Restore dependencies

```bash
dotnet restore
```

4. Update database connection string in `appsettings.json`

5. Run the application

```bash
dotnet run
```

The API will be available at:

- HTTP: `http://localhost:8080`
- HTTPS: `http://localhost:5268`

### API Documentation

Access the Swagger documentation at: `http://localhost:8080/swagger` (Development mode)

## Security Features

- JWT-based authentication and authorization with Bearer token
- Secure API endpoints with configurable JWT validation
- Role-based access control
- OpenAPI security scheme integration

## Development Status

- ✅ **Backend API**: Completed (.NET 8.0)
- 🔄 **Frontend Client**: Coming Soon
- ✅ **Database Layer**: Entity Framework Core implementation
- ✅ **Authentication**: JWT Bearer implementation

## Course Information

- **Course:** PRN232 - Advanced Cross-Platform Development with .NET API
- **Semester:** Summer 2025 (SU25)
- **Student ID:** SE1730
- **Assignment:** ASM1_TienDM_00035845

## License

This project is developed for educational purposes as part of university coursework at FPT University.
