namespace Apangelia.Core;

public sealed record NotificationEventInput(
    string Source,
    string EventType,
    string ExternalEventId,
    string Title,
    string? Message,
    string RawPayloadJson,
    DateTimeOffset OccurredAt);
