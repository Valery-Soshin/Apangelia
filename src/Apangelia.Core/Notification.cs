namespace Apangelia.Core;

/// <summary>
/// Уведомление, созданное из нормализованного события внешней интеграции.
/// </summary>
public sealed class Notification
{
    /// <summary>
    /// Уникальный идентификатор уведомления.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Ключ источника события, из которого создано уведомление.
    /// </summary>
    public string Source { get; set; } = string.Empty;

    /// <summary>
    /// Тип события во внешней системе.
    /// </summary>
    public string EventType { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор события во внешней системе.
    /// </summary>
    public string ExternalEventId { get; set; } = string.Empty;

    /// <summary>
    /// Краткий заголовок уведомления.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Дополнительный текст уведомления.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Время возникновения или приема исходного события.
    /// </summary>
    public DateTimeOffset OccurredAt { get; set; }

    /// <summary>
    /// Время создания уведомления в системе.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }
}
