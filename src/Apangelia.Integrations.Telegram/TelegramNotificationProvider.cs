using Apangelia.Application.NotificationProviders;

namespace Apangelia.Integrations.Telegram;

public class TelegramNotificationProvider : INotificationProvider
{
    public string ProviderKey => "telegram";

    public Task<NotificationProviderResult> SendAsync(NotificationProviderRequest request, CancellationToken cancellationToken)
    {
        Console.WriteLine("Sending Telegram notification...");

        return Task.FromResult(NotificationProviderResult.Success());
    }
}
