# Repository Instructions

## Working Style

Read the existing code before changing it and keep edits scoped to the user's request. Prefer the repository's current structure and conventions over introducing new patterns.

This is a .NET solution with Aspire-style projects. Treat the projects as coordinated parts of the same application unless the task says otherwise.

Do not introduce new frameworks, persistence layers, background job systems, or architectural layers unless the current task clearly needs them. When adding dependencies, explain why they are needed, especially for infrastructure packages. Keep package versions centralized in `Directory.Packages.props`.

Use normal .NET configuration patterns: `appsettings*.json`, environment variables, dependency injection, and typed options when configuration grows beyond a one-off value. Do not log secrets or put secrets in committed config files.

Do not add constructor null checks for dependencies provided by dependency injection. Rely on DI container validation and nullable annotations for required services; reserve explicit null validation for non-DI inputs and public API boundaries.

For options classes, keep configuration values immutable after binding: use `required` properties with `init` setters. Put default configurable values in `appsettings.json` rather than property initializers on options classes. Do not add section-name constants to options types for one-off bindings; bind the configuration section explicitly from the composition root.

Do not unwrap `IOptions<TOptions>` in DI factory registrations only to pass options into a service. Register the service normally, inject `IOptions<TOptions>` into the constructor, and read `.Value` there so DI wiring does not change when options gain new properties.

Prefer async APIs for I/O-bound work, include `CancellationToken` on cancellable async boundaries, and use structured logging for operational messages.

Use file-scoped namespaces in every C# file: declare namespaces as `namespace Apangelia.SomeProject;` and do not use block-scoped `namespace ... { }` declarations.

Formatting style rules do not apply to automatically generated files. Do not reformat generated code solely to match repository style.

Use `_camelCase` names for private readonly fields. Do not qualify instance member access with `this.`; `this` is only acceptable where C# syntax requires it, such as extension-method parameters.

For generated `Guid` entity identifiers, use Guid v7 from the persistence layer through an EF `ValueGenerator`; do not call `Guid.NewGuid()` or assign entity IDs manually in Application/Core mapping code unless the task explicitly requires it.

Use `record` for DTOs and other immutable data carriers. Use `class` for mutable entities and objects with identity or lifecycle.

For empty class, interface, or struct declarations, use a semicolon declaration instead of an empty `{ }` body, and do not add extra blank lines for empty bodies.

When creating or editing files, remove trailing whitespace on every line, including whitespace-only blank lines. New files must end with exactly one final newline and must not contain indented empty lines.

## Comments and Documentation

Write all new and updated code comments in Russian, including XML documentation comments.

Cover public entities and contracts with useful XML comments when they are part of the repository's public or cross-layer surface: interfaces, DTOs, options, public records/classes/enums, and public members that define a contract. This includes domain notification contracts such as provider interfaces, event input records, deliveries, and send results.

Do not duplicate comments on simple implementation classes when the implemented contract is already documented. Add implementation comments only when the implementation exposes additional public behavior, constraints, or non-obvious details not covered by the contract.

Keep comments purposeful: document intent, contract expectations, invariants, and edge cases rather than restating names or obvious code.

If a `catch` block intentionally swallows an exception, include a concise Russian comment inside the block explaining why the exception is ignored.

## Project Architecture

`Apangelia.AppHost` is the Aspire orchestration entry point. Keep it focused on composing runnable services and infrastructure resources, including Aspire-managed PostgreSQL; do not put application behavior there. Do not commit real database credentials or other secret resource parameters.

`Apangelia.ServiceDefaults` holds shared service-host defaults such as health checks, service discovery, HTTP resilience, logging, and OpenTelemetry. Reference it from runnable service projects that need those defaults, not from domain or infrastructure libraries.

`Apangelia.WebApi` is the HTTP composition root. Keep routing, request/response concerns, OpenAPI setup, and endpoint registration here. The `Configurations` extension-method classes are the wiring layer for dependency injection, middleware, Swagger, Entity Framework, and endpoint mapping; keep business behavior out of them. Thin endpoints should delegate non-trivial webhook handling, validation, persistence, and integration work to `Apangelia.Application`.

`Apangelia.Application` is the use-case layer. Put application services, commands/queries, workflow orchestration, validation that is not HTTP-specific, and ports for persistence or external integrations here. It may depend on `Apangelia.Core`; keep it independent of ASP.NET Core, Aspire, concrete database providers, and external API SDKs.

Use the shared command abstractions in `Apangelia.Application.Commands` for application commands. New command handlers should implement `ICommandHandler<TCommand, TResult>` and be invoked through `ICommandDispatcher` so pipeline behaviors can wrap them consistently. Do not add feature-specific handler interfaces just to call commands directly. Keep the dispatcher scoped; if a singleton or background worker needs to dispatch commands, create a service scope at that boundary and resolve the dispatcher inside it. Keep transaction handling inside the handler unless a task explicitly introduces a transaction pipeline behavior.

`Apangelia.Core` is the intended home for domain model, domain contracts, and business rules. Keep it independent of ASP.NET Core, Aspire, database providers, external API SDKs, and application orchestration unless there is a deliberate architectural change.

`Apangelia.Integrations.GitHub` is the boundary for GitHub-specific behavior such as webhook payload models, signature verification, API clients, and GitHub mapping logic. Use it to implement application ports; keep GitHub details out of `Apangelia.Application` and `Apangelia.Core` except for neutral contracts or domain concepts.

`Apangelia.Persistence.Postgres` is the boundary for PostgreSQL persistence. Keep EF Core `DbContext`, migrations, Npgsql mapping, and provider-specific storage code here, then expose them by implementing abstractions that `Apangelia.Application` can consume. Registration and migration application may be wired from `Apangelia.WebApi`, but `Apangelia.Application` and `Apangelia.Core` must not depend on EF Core or Npgsql.

Do not edit existing EF Core migration files unless the migration was created in the current diff. For model changes after a migration already exists, create a separate migration and let `AppDbContextModelSnapshot` reflect the latest model.

When changing a model or EF configuration that represents database schema, include the corresponding EF Core migration in the same change. Do not leave schema-affecting entity changes without a migration.

Create EF Core migrations from the repository root with `dotnet ef migrations add <Name> --project src\Apangelia.Persistence.Postgres\Apangelia.Persistence.Postgres.csproj --startup-project src\Apangelia.WebApi\Apangelia.WebApi.csproj --output-dir Migrations`. Do not use `--no-build` unless the solution has just been built successfully and the compiled artifacts are known to match the current source.

Prefer dependency flow from hosts/adapters inward: `AppHost` composes services, `WebApi` wires features, integration and persistence projects implement external boundaries, `Application` orchestrates use cases, and `Core` stays the most independent project. Update `Apangelia.slnx` and project references together when adding or wiring projects.

## Verification

For code changes, run the narrowest useful build or test command before finishing. If verification cannot be run, say why.

For docs-only changes to this file, inspect `git diff -- AGENTS.md` and run `git diff --check`; a build is not required unless code or project files were also changed.

When adding tests, match the test framework already used by the repository. If no test project exists yet, ask or keep the change small enough that a build is the main verification step.

## Git

When creating commits, follow Conventional Commits 1.0.0:

```text
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]
```

Prefer common types such as `feat`, `fix`, `docs`, `refactor`, `test`, `build`, `ci`, and `chore`. Keep the description imperative, concise, and lowercase unless a proper noun requires capitalization.

Do not commit local tool state. Serena project configuration may be committed, but `.serena/cache` and `.serena/project.local.yml` must stay ignored.
