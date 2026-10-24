# BookStore

A learning project: an ASP.NET Core Web API for a book store, backed by PostgreSQL and Entity Framework Core, with a small CLI for migrations and seeding.

## Tech Stack

- .NET 10 / ASP.NET Core (controllers + OpenAPI)
- Entity Framework Core 10 with Npgsql (PostgreSQL 17)
- Bogus for fake seed data
- Docker Compose for the database

## Project Structure

```
BookStore.sln
├── BookStore/        # Web API
│   ├── Controllers/
│   ├── Data/         # AppDbContext, entity configurations, DataSeeder
│   ├── Migrations/
│   └── Models/Entities/   # Author, Book, BookDetail, Category, Loan, Member, Review
├── BookStore.Cli/    # Console app: migrate / seed / fresh
├── compose.yaml      # PostgreSQL container
└── Makefile          # Shortcuts for CLI commands
```

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)
- `make` (optional)

## Getting Started

1. Start the database:

   ```bash
   docker compose up -d
   ```

2. Apply migrations and seed sample data:

   ```bash
   make fresh-and-seed
   ```

3. Run the API:

   ```bash
   dotnet run --project BookStore
   ```

   In Development, the OpenAPI document is served at `/openapi/v1.json`.

## Database CLI

| Command | Description |
| --- | --- |
| `make migrate` | Apply pending migrations |
| `make seed` | Seed the database with fake data |
| `make fresh` | Drop the database and re-run migrations |
| `make fresh-and-seed` | Drop, migrate, and seed |

These wrap `dotnet run --project BookStore.Cli -- <migrate|seed|fresh [--seed]>`.

## Configuration

The connection string is in `BookStore/appsettings.json` and matches the credentials in `compose.yaml`:

```
Host=localhost;Port=5432;Database=bookstore;Username=bookstore;Password=bookstore_pass
```

These are local development credentials only; change them for any other environment.

## Adding a Migration

```bash
dotnet ef migrations add <Name> --project BookStore
```
