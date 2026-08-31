# BookLibrary — Agent Guide

## Stack
- **.NET 10** Web API + EF Core 10.0 / Npgsql 10.0.1 / PostgreSQL
- Solution: `BookLibrary.slnx` (modern `.slnx` format)
- 2 projects: `BookLibrary.Api` (Web), `BookLibrary.Data` (class library)

## Commands
| Action | Command |
|---|---|
| Restore | `dotnet restore` |
| Build | `dotnet build` |
| Run API | `dotnet run --project BookLibrary.Api` (default: `http://localhost:5002`) |
| EF migration | `dotnet ef migrations add <Name> --project BookLibrary.Data --startup-project BookLibrary.Api` |
| EF update DB | `dotnet ef database update --startup-project BookLibrary.Api` |
| EF tool | `dotnet tool run dotnet-ef` (defined in `dotnet-tools.json`) |
| Health check | `GET http://localhost:5002/health` |

## Architecture
- **No repository layer** — services use `IDbContextFactory<BookDbContext>` directly, creating short-lived contexts per operation.
- **Bootstrapper pattern** — `Bootstrap/{Database,Infrastructure,Services,Api}Bootstrap.cs` contain `IServiceCollection` extension methods registered in `Program.cs`.
- **Soft delete** — `IsDeleted` on Book, Work, Series, Category; enforced via global query filter in `BookDbContext.OnModelCreating`.
- **DTOs** separated by domain in `DTOs/{BookDtos,AuthorDtos,...}/`. Create/update DTOs are separate from response DTOs.
- **Swagger** at `/swagger` in Development only.

## DB prerequisite
Requires a running PostgreSQL with a `BookLibrary` database:
```
Host=localhost;Port=5432;Database=BookLibrary;Username=postgres;Password=postgres
```

## API endpoints
- `GET /books` (supports filter/pagination via `BookFilter` DTO)
- `GET /books/{id}`, `POST /books`, `PUT /books/{id}`, `DELETE /books/{id}`, `POST /books/{id}/restore`
- `GET /authors`, `GET /authors/{id}`, `GET /authors/search?q=`, `POST /authors`
- `GET /categories`, `POST /categories`
- `GET /publishers`, `GET /publishers/{id}`, `POST /publishers`
- `GET /series?onlyRoots=true`, `GET /series/{id}`, `POST /series`, `PUT /series/{id}`, `DELETE /series/{id}`

## Gotchas
- **Dockerfile is broken** — references `BookLibrary/BookLibrary.csproj` but the API project is at `BookLibrary.Api/BookLibrary.Api.csproj`. Needs fix before Docker builds work.
- **No tests** — zero test projects exist; don't assume any test framework.
- **No CI/CD** — no workflow configs committed.
- **Unused DTOs** — `CategoryCreateDto`, `AuthorCreateDto` (in AuthorDtos folder), `PublisherCreateDto` exist but controllers accept `string name` directly instead.
- **`WorkUpdateDto.Id` is unused** — update logic removes all non-deleted works and re-adds them rather than matching by Id.
- **Case-insensitive search** uses `ToLower()` in-memory instead of `EF.Functions.ILike` (may not translate optimally to PostgreSQL).
- **`BookLibrary.http`** still references template `weatherforecast/` endpoint — not useful.
