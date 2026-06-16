namespace Apangelia.Persistence.Postgres.Entities;

/// <summary>
/// Входящее сообщение уведомления, принятое из внешней интеграции.
/// </summary>
public sealed class NotificationInboxEntity
{
    public Guid Id { get; set; }

    public string Source { get; set; } = string.Empty;

    public string EventType { get; set; } = string.Empty;

    public string ExternalEventId { get; set; } = string.Empty;

    public string RawPayloadJson { get; set; } = string.Empty;

    public DateTimeOffset OccurredAt { get; set; }
}
