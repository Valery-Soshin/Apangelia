namespace Apangelia.Application.NotificationProviders;

/// <summary>
/// Стандартный resolver исходящих провайдеров уведомлений из контейнера зависимостей.
/// </summary>
public sealed class NotificationProviderResolver : INotificationProviderResolver
{
    private readonly IReadOnlyDictionary<string, INotificationProvider> _providers;

    public NotificationProviderResolver(IEnumerable<INotificationProvider> providers)
    {
        _providers = providers.ToDictionary(
            provider => provider.ProviderKey,
            StringComparer.OrdinalIgnoreCase);
    }

    public INotificationProvider Resolve(string providerKey)
    {
        if (_providers.TryGetValue(providerKey, out var provider))
        {
            return provider;
        }

        throw new InvalidOperationException(
            $"Notification provider '{providerKey}' is not registered.");
    }
}
