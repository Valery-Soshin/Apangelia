# Apangelia

Apangelia is a .NET-based smart notification router that receives external events, normalizes them into notifications, applies idempotency and routing rules, and delivers them asynchronously through providers like Telegram.

## Tech Stack

- .NET / ASP.NET Core Minimal APIs
- .NET Aspire
- PostgreSQL
- EF Core
- Background workers
- GitHub webhooks
- Telegram Bot API

## Architecture & Patterns

- Layered architecture
- Integration layer for external systems
- Idempotency / Inbox pattern
- Outbox-based async delivery
- Provider strategy pattern
- Delivery retries and dead-letter handling planned

## Status

Early MVP in development.

Current focus: GitHub webhook ingestion, event normalization, idempotency, notification creation, routing, and Telegram delivery.
