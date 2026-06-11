namespace Apangelia.Core;

public interface INotificationProvider
{
    string ProviderKey { get; }

    Task<SendResult> SendAsync(NotificationDelivery delivery, CancellationToken ct);
}