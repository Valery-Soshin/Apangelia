using Apangelia.Application.Notifications;

namespace Apangelia.Integrations.Telegram;

public class TelegramNotificationProvider : INotificationProvider
{
    public string ProviderKey => "telegram";

    public Task<NotificationSendResult> SendAsync(NotificationSendRequest request, CancellationToken cancellationToken)
    {
        Console.WriteLine("Sending Telegram notification...");

        return Task.FromResult(NotificationSendResult.Success());
    }
}