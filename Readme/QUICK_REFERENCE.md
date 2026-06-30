# Quick Reference Guide

## ?? Solution Location
```
C:\Users\WaseemJaffar\Desktop\DeliveryTracking
```

## ?? Common Commands

### Build & Restore
```bash
# Restore NuGet packages
dotnet restore

# Build the solution
dotnet build

# Clean the solution
dotnet clean
```

### Testing
```bash
# Run all tests
dotnet test

# Run tests with verbose output
dotnet test --verbosity detailed

# Run specific test project
dotnet test DeliveryTracking.UnitTests/DeliveryTracking.UnitTests.csproj
```

### Running the API
```bash
# Run the API project
dotnet run --project DeliveryTracking.API

# Run with launch settings
dotnet run --project DeliveryTracking.API --launch-profile https
```

### Adding Packages
```bash
# Add package to a specific project
dotnet add DeliveryTracking.Application package MediatR

# Add package to API project
dotnet add DeliveryTracking.API package Microsoft.EntityFrameworkCore
```

### Project References
```bash
# Add project reference
dotnet add DeliveryTracking.API reference DeliveryTracking.Domain

# Remove project reference
dotnet remove DeliveryTracking.API reference DeliveryTracking.Domain
```

---

## ?? Recommended NuGet Packages

### For Application Layer
- `MediatR` - CQRS Pattern
- `FluentValidation` - Request validation
- `AutoMapper` - DTO mapping

### For Persistence Layer
- `Microsoft.EntityFrameworkCore` - ORM
- `Microsoft.EntityFrameworkCore.SqlServer` - SQL Server provider
- `Microsoft.EntityFrameworkCore.Design` - Migration tools

### For Infrastructure Layer
- `Microsoft.AspNetCore.SignalR` - Real-time communication
- `SendGrid` or `MailKit` - Email services
- `jwt` - JWT authentication

### For API Project
- `Swashbuckle.AspNetCore` - Swagger/OpenAPI
- `Microsoft.AspNetCore.Cors` - CORS support

### For Testing
- `Moq` - Mocking framework
- `FluentAssertions` - Better assertions
- `xunit.runner.visualstudio` - Test runner

---

## ??? Architecture Patterns to Implement

### 1. Repository Pattern
**Location:** `DeliveryTracking.Persistence/Repositories/`
```csharp
public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
```

### 2. CQRS Pattern with MediatR
**Location:** `DeliveryTracking.Application/Features/`
```csharp
// Command
public class CreateDeliveryCommand : IRequest<int>
{
    public string TrackingNumber { get; set; }
    public string Destination { get; set; }
}

// Query
public class GetDeliveryQuery : IRequest<DeliveryDto>
{
    public int DeliveryId { get; set; }
}

// Handler
public class CreateDeliveryCommandHandler : IRequestHandler<CreateDeliveryCommand, int>
{
    public Task<int> Handle(CreateDeliveryCommand request, CancellationToken cancellationToken)
    {
        // Implementation
    }
}
```

### 3. DTO Pattern
**Location:** `DeliveryTracking.Application/DTOs/`
```csharp
public class DeliveryDto
{
    public int Id { get; set; }
    public string TrackingNumber { get; set; }
    public string Destination { get; set; }
    public DeliveryStatus Status { get; set; }
}
```

### 4. Dependency Injection
**Location:** `DeliveryTracking.API/Extensions/`
```csharp
public static IServiceCollection AddApplicationServices(
    this IServiceCollection services)
{
    services.AddMediatR(cfg => 
        cfg.RegisterServicesFromAssembly(typeof(CreateDeliveryCommand).Assembly));

    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    services.AddScoped<IUnitOfWork, UnitOfWork>();

    return services;
}
```

### 5. Middleware
**Location:** `DeliveryTracking.API/Middleware/`
```csharp
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // Exception handling logic
        }
    }
}
```

---

## ??? File Organization Tips

### Domain Entities
```
DeliveryTracking.Domain/
??? Entities/
?   ??? Delivery.cs
?   ??? DeliveryItem.cs
?   ??? Driver.cs
?   ??? Customer.cs
??? Enums/
?   ??? DeliveryStatus.cs
?   ??? PaymentStatus.cs
??? Common/
    ??? BaseEntity.cs
    ??? AuditableEntity.cs
```

### Application Features
```
DeliveryTracking.Application/
??? Features/
    ??? Deliveries/
    ?   ??? Commands/
    ?   ?   ??? CreateDeliveryCommand.cs
    ?   ?   ??? CreateDeliveryCommandHandler.cs
    ?   ??? Queries/
    ?   ?   ??? GetDeliveryQuery.cs
    ?   ?   ??? GetDeliveryQueryHandler.cs
    ?   ??? DeliveryDto.cs
    ??? Drivers/
    ?   ??? Commands/
    ?   ??? Queries/
    ?   ??? DriverDto.cs
    ??? Customers/
        ??? Commands/
        ??? Queries/
        ??? CustomerDto.cs
```

### API Controllers
```
DeliveryTracking.API/
??? Controllers/
    ??? DeliveriesController.cs
    ??? DriversController.cs
    ??? CustomersController.cs
    ??? TrackingController.cs
```

---

## ?? Testing Structure

### Unit Tests Organization
```
DeliveryTracking.UnitTests/
??? Domain/
?   ??? Entities/
?   ??? Enums/
??? Application/
?   ??? Features/
?   ?   ??? DeliveriesTests/
?   ?   ??? DriversTests/
?   ?   ??? CustomersTests/
?   ??? Common/
?   ??? Behaviors/
??? Fixtures/
    ??? (Shared test fixtures and builders)
```

---

## ?? Recommended Reading

- Clean Architecture by Robert C. Martin
- Domain-Driven Design by Eric Evans
- Microsoft Docs: ASP.NET Core Architecture
- MediatR Documentation
- Entity Framework Core Documentation

---

## ? Best Practices Checklist

- [ ] Entities in Domain layer have no dependencies
- [ ] Application layer uses interfaces for external services
- [ ] Infrastructure implements interfaces defined in Application
- [ ] API layer is thin and delegates to Application layer
- [ ] Circular dependencies are avoided
- [ ] All public methods have XML documentation
- [ ] Unit tests use AAA pattern (Arrange, Act, Assert)
- [ ] Async/await is used for I/O operations
- [ ] Dependency Injection is configured in API startup
- [ ] Exception handling middleware is in place

---

**Happy coding! ??**
