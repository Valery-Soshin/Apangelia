# Repository Instructions

## Git Commit Messages

When creating Git commits, follow Conventional Commits 1.0.0.

Use the format:

```text
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]
```

Prefer common types such as `feat`, `fix`, `docs`, `style`, `refactor`, `perf`, `test`, `build`, `ci`, and `chore`. Use `!` after the type/scope or a `BREAKING CHANGE:` footer for breaking changes. Keep the description imperative, concise, and lowercase unless a proper noun requires capitalization.

## .NET Defaults

Treat these as default .NET preferences unless a repository's existing conventions or local instructions say otherwise. Prefer nullable reference types, dependency injection over service location or static global state, async APIs for I/O-bound work, `Async` suffixes for asynchronous method names, `CancellationToken` on cancellable async boundaries, structured logging without secrets, and typed options for configuration.

When working in .NET projects, prefer xUnit for tests unless the repository already standardizes on another framework. Keep tests focused on observable behavior and use clear arrange/act/assert structure.

Prefer clean layered architecture: domain/core code should stay dependency-light, application code should own use-case orchestration and contracts, and infrastructure code should contain framework, SDK, database, HTTP, file system, and platform-specific adapters.

For ASP.NET Core APIs, prefer controller-based endpoints over Minimal API endpoints unless the repository already standardizes on Minimal APIs or the task explicitly calls for them. Keep environment-specific configuration in `appsettings*.json`, environment variables, and the normal .NET configuration pipeline; bind typed settings through the Options pattern with `IOptions<T>`, `IOptionsSnapshot<T>`, or `IOptionsMonitor<T>` instead of scattering raw configuration reads through the codebase. Enable OpenAPI documentation for APIs, using Swagger UI when appropriate for local development and diagnostics.

For data access, prefer Entity Framework Core as the default ORM choice when the project does not already standardize on another data access approach. Respect existing choices such as Dapper, raw SQL, document databases, or provider-specific SDKs when they are already established.

For background jobs and task scheduling, prefer Hangfire when the application needs persistent jobs, retries, delayed or recurring execution, and operational visibility. For simple in-process background work, use `BackgroundService` or `IHostedService` when that better fits the scope.

Use C# type shapes intentionally. Prefer `record`/`record struct` for immutable value-oriented models and sealed classes/records when inheritance is not part of the design. Avoid using classes by default when a smaller value type or record better communicates intent.

Name private fields with a leading underscore and camelCase, for example `_service` and `_settings`. Do not add leading underscores to locals, parameters, properties, constants, or public/protected/internal members.

Avoid trailing whitespace and keep file endings tidy: no extra spaces at the end of lines and no unnecessary blank lines at the end of files.
