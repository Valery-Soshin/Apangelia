namespace Apangelia.Core;

/// <summary>
/// Унифицированное входящее событие уведомления из внешней интеграции.
/// </summary>
/// <param name="Source">Ключ источника события, например github.</param>
/// <param name="EventType">Тип события во внешней системе.</param>
/// <param name="ExternalEventId">Уникальный идентификатор доставки или события во внешней системе.</param>
/// <param name="Title">Краткий заголовок уведомления.</param>
/// <param name="Message">Дополнительное сообщение уведомления, если источник его предоставил.</param>
/// <param name="RawPayloadJson">Исходное JSON-тело события.</param>
/// <param name="OccurredAt">Время возникновения или приема события.</param>
public sealed record NotificationEvent(
    string Source,
    string EventType,
    string ExternalEventId,
    string Title,
    string? Message,
    string RawPayloadJson,
    DateTimeOffset OccurredAt);
