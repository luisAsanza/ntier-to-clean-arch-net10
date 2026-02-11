# 🏗️ Legacy to Clean Architecture Migration

![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white) ![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white) ![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white) ![Architecture](https://img.shields.io/badge/Clean%20Architecture-Blue?style=for-the-badge)

> **A modernization project transforming a tightly coupled N-Tier legacy application into a robust, testable solution using Clean Architecture and the Explicit Service Pattern.**

---

## 📖 The Story
This project demonstrates the migration of a legacy ASP.NET N-Tier application. The original system suffered from scattered business logic, exception-driven control flow, and difficult maintenance.

**The Goal:** Refactor the codebase to adhere to the **Dependency Inversion Principle** and **Separation of Concerns**, ensuring the core business logic remains independent of external frameworks.

**The Strategy:**
* **Layered Abstraction:** Moved from horizontal "Data Access Layers" to a domain-centric **Clean Architecture**.
* **Explicit Services:** Instead of indirect complexity (CQRS), I implemented strong, typed **Application Services** that orchestrate validation, mapping, and persistence.
* **Result Pattern:** Replaced try-catch control flow with a functional `Result<T>` pattern for predictable error handling.

---

## 🚀 Architecture Overview

The solution follows the **Clean Architecture** principles, divided into four distinct projects to enforce dependency rules:

1.  **Domain (Core):**
    * Contains Enterprise Entities (`Person`) and Domain primitives.
    * Pure C# classes with no dependencies.
    * *Role:* The "Heart" of the system.

2.  **Application (Core):**
    * Defines the Service Contracts (`IPersonService`) and Repositories (`IPersonsRepository`).
    * Implements Business Logic Services (`PersonService`).
    * Handles Validation (`FluentValidation`) and Result/Error definitions.
    * *Role:* The "Brain" of the system.

3.  **Infrastructure:**
    * Implements Repositories using **EF Core**.
    * Handles Database Migrations and Context configuration.
    * *Role:* The "Plumbing" and data access.

4.  **Web (Presentation):**
    * ASP.NET Core MVC.
    * Handles HTTP requests, AutoMapper (ViewModel mapping), and dependency injection.
    * Delegates logic to Application Services.
    * *Role:* The entry point.

---

## 🛠️ Key Technical Patterns

### 1. Explicit Service Pattern
Business logic is encapsulated in explicit services (e.g., `PersonService`). This ensures:
* **Readability:** dependencies like `IValidator` and `IRepository` are clearly injected via constructor.
* **Testability:** Services are easily mocked in unit tests.
* **High Cohesion:** Validation, Mapping, and Persistence orchestration happen in one clear place.

### 2. Result Pattern (Functional Error Handling)
Instead of throwing Exceptions for validation errors or "Not Found" scenarios, the application returns a `Result` object.

```csharp
// No try-catch needed in the controller!
public async Task<Result<PersonResponse>> GetPersonAsync(Guid id) {
    if (person is null) return Result.Failure(PersonErrors.NotFound(id));
    return Result.Success(person);
}
```

### 3. High-Performance Logging
Utilized .NET **Source Generators** for logging (`[LoggerMessage]`) within partial classes to reduce allocation overhead and improve performance compared to standard `_logger.LogInformation`.

### 4. Robust Validation
* **FluentValidation:** Validation rules are decoupled from the DTOs and injected into the Service.
* **Defensive Coding:** Inputs are validated *before* touching the domain or database.

---

## 💻 Tech Stack

* **Framework:** .NET 10 (Preview) / ASP.NET Core
* **Persistence:** SQL Server / Entity Framework Core
* **Logging:** Serilog (Structured Logging)
* **Mapping:** AutoMapper (Presentation Layer) & Custom Mappers (Application Layer)
* **Validation:** FluentValidation
* **Testing:** xUnit, Moq, FluentAssertions

---

## 🔧 Getting Started

### Prerequisites
* .NET SDK 9.0+
* SQL Server (LocalDB or Docker)

### Installation
1.  **Clone the repo**
    ```bash
    git clone [https://github.com/yourusername/ntier-to-clean-arch.git](https://github.com/yourusername/ntier-to-clean-arch.git)
    ```
2.  **Configure Database**
    Update `appsettings.json` in `CleanCRUDSolution.Web` with your connection string.
3.  **Apply Migrations**
    ```bash
    dotnet ef database update --project CleanCRUDSolution.Infrastructure --startup-project CleanCRUDSolution.Web
    ```
4.  **Run**
    ```bash
    dotnet run --project CleanCRUDSolution.Web
    ```

---

## 📝 Author
**Luis Asanza**
* **LinkedIn:** 
* **Portfolio:** 

> *This project showcases how to write clean, maintainable, and testable code without over-engineering.*
