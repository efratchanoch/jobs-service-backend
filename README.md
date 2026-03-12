## jobs-service-backend (.NET 8 Web API)

This project is a .NET 8 Web API organized with a simple 3-tier architecture:

- **Data**: EF Core `AppDbContext`, entities, and migrations.
- **BLL**: Repositories and business logic/services.
- **DTOs**: Data transfer objects used by controllers.
- **Controllers**: HTTP API endpoints.

### Project structure

- `Data/Entities` – entity classes (domain models).
- `Data/AppDbContext.cs` – EF Core context.
- `Data/Migrations` – EF Core migrations output folder.
- `BLL/Repositories` – repository interfaces and implementations.
- `BLL/Services` – business logic and application services.
- `DTOs` – DTO types returned/accepted by controllers.
- `Controllers` – API controllers (e.g. `HealthController`).

### EF Core packages

Installed NuGet packages (all targeting .NET 8 / EF Core 8):

- `Microsoft.EntityFrameworkCore` (8.0.0)
- `Microsoft.EntityFrameworkCore.SqlServer` (8.0.0)
- `Microsoft.EntityFrameworkCore.Design` (8.0.0)

### Configuring the database

`Program.cs` wires up `AppDbContext` using a SQL Server connection string named `DefaultConnection`:

- Set `"ConnectionStrings:DefaultConnection"` in `appsettings.Development.json` or `appsettings.json`, for example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\\\mssqllocaldb;Database=JobsServiceDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

If the connection string is missing, the same LocalDB value above is used as a fallback.

### Creating migrations (future)

Once you add entities and `DbSet<T>` properties to `AppDbContext`, you can create and apply migrations:

```bash
dotnet ef migrations add InitialCreate -o Data/Migrations
dotnet ef database update
```

### Running the API

From the project folder:

```bash
dotnet run --project jobs-service-backend.csproj
```

Then browse to:

- `https://localhost:5001/swagger` (or the URL shown in the console) for Swagger UI.
- `https://localhost:5001/api/health` for the simple health-check endpoint.

