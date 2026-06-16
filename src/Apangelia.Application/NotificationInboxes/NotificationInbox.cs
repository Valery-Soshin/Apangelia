namespace Apangelia.Application.NotificationInboxes;

/// <summary>
/// Данные входящего события уведомления для проверки идемпотентности во входящем журнале.
/// </summary>
/// <param name="Source">Ключ источника события, например github.</param>
/// <param name="EventType">Тип события во внешней системе.</param>
/// <param name="ExternalEventId">Уникальный идентификатор доставки или события во внешней системе.</param>
/// <param name="RawPayloadJson">Исходное JSON-тело события.</param>
/// <param name="OccurredAt">Время возникновения или приема события.</param>
public sealed record NotificationInbox(
    string Source,
    string EventType,
    string ExternalEventId,
    string RawPayloadJson,
    DateTimeOffset OccurredAt);
