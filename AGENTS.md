# Repository Instructions

## Working Style

Read the existing code before changing it and keep edits scoped to the user's request. Prefer the repository's current structure and conventions over introducing new patterns.

This is a .NET solution with Aspire-style projects. Treat `Apangelia.AppHost`, `Apangelia.ServiceDefaults`, and `Apangelia.WebApi` as coordinated parts of the same application unless the task says otherwise.

Do not introduce new frameworks, persistence layers, background job systems, or architectural layers unless the current task clearly needs them. When adding dependencies, explain why they are needed.

Use normal .NET configuration patterns: `appsettings*.json`, environment variables, dependency injection, and typed options when configuration grows beyond a one-off value. Do not log secrets or put secrets in committed config files.

Prefer async APIs for I/O-bound work, include `CancellationToken` on cancellable async boundaries, and use structured logging for operational messages.

## Verification

For code changes, run the narrowest useful build or test command before finishing. If verification cannot be run, say why.

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
