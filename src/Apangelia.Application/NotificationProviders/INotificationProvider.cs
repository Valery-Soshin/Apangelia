namespace Apangelia.Application.NotificationProviders;

/// <summary>
/// Отправляет уведомления во внешний исходящий канал.
/// </summary>
public interface INotificationProvider
{
    /// <summary>
    /// Идентификатор исходящего провайдера, используемый в маршруте доставки.
    /// </summary>
    string ProviderKey { get; }

    /// <summary>
    /// Отправляет уведомление по конкретному маршруту доставки.
    /// </summary>
    /// <param name="request">Данные уведомления, маршрута и задания доставки.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат попытки отправки.</returns>
    Task<NotificationProviderResult> SendAsync(
        NotificationProviderRequest request,
        CancellationToken cancellationToken);
}
