# Novus

API backend built with **.NET 10**.

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (optional, for containerized setup)

### Run Locally

```bash
dotnet run --project src/Novus.Api
```

Or via Makefile:

```bash
make run
```

### Run with Docker Compose

Spins up the API along with MySQL and MsSQL databases:

```bash
docker compose up --build
```

### Useful Endpoints

| Endpoint | URL |
|---|---|
| API Docs (Scalar) | http://localhost:5000/api-doc/ |
| Health Check | http://localhost:5000/healthz/ready |
| Version | http://localhost:5000/version |

## Project Structure

```
src/
├── Novus.Api              # API entry point, controllers, middleware, infrastructure
├── Novus.Core             # Business logic, models, repositories, services, DB contexts
├── Novus.BooksModule      # Separate module (modular monolith, vertical slices)
└── Novus.Db               # DB migration tool (DbUp)

test/
├── Novus.Api.IntegrationTests
├── Novus.Api.UnitTests
├── Novus.Core.UnitTests
├── Novus.ArchitecturalTests
└── Novus.BooksModule.IntegrationTests
```

## Database Migrations

Run the migration tool against MsSQL:

```bash
dotnet run --project src/Novus.Db
```

## Docker

Build image:

```bash
docker build -t novus:local .
```

Run container:

```bash
docker run --rm -p 5000:8080 novus:local
```

## Tech Stack

- **Framework**: ASP.NET Core 10
- **ORM**: EF Core (MySQL + MsSQL), Dapper (Sqlite)
- **API Docs**: OpenAPI + Scalar
- **Logging**: Serilog
- **Migrations**: DbUp
- **Feature Flags**: Microsoft.FeatureManagement
- **Health Checks**: AspNetCore.Diagnostics.HealthChecks
- **Testing**: xUnit v3, AutoFixture, Moq, AwesomeAssertions, Verify
- **Code Quality**: NetArchTest, CodeQL, EditorConfig

## License

See [LICENSE](LICENSE).
