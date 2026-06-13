namespace Apangelia.Core;

/// <summary>
/// Пользовательское правило маршрутизации входящего уведомления в исходящий канал.
/// </summary>
public sealed class NotificationRoute
{
    /// <summary>
    /// Уникальный идентификатор маршрута.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор пользователя, которому принадлежит маршрут.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Идентификатор входящего провайдера, от которого маршрут принимает уведомления.
    /// </summary>
    public string InputProviderId { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор исходящего провайдера, в который маршрут направляет уведомления.
    /// </summary>
    public string OutputProviderId { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор назначения внутри исходящего провайдера.
    /// </summary>
    public string DestinationId { get; set; } = string.Empty;

    /// <summary>
    /// JSON с дополнительными условиями применения маршрута.
    /// </summary>
    public string? ConditionsJson { get; set; }

    /// <summary>
    /// Время создания маршрута.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }
}
