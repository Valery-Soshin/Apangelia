namespace Apangelia.Application.NotificationProviders;

/// <summary>
/// Находит исходящий провайдер уведомлений по ключу маршрута.
/// </summary>
public interface INotificationProviderResolver
{
    /// <summary>
    /// Возвращает провайдер для указанного ключа.
    /// </summary>
    /// <param name="providerKey">Идентификатор исходящего провайдера.</param>
    /// <returns>Зарегистрированный провайдер уведомлений.</returns>
    INotificationProvider Resolve(string providerKey);
}
