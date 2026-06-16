using System.Text;
using Apangelia.Application.Abstractions.Providers;

namespace Apangelia.Integrations.Telegram;

public sealed class TelegramNotificationProvider : INotificationProvider
{
    private const int MaxMessageLength = 4096;

    private readonly ITelegramBotClient _telegramBotClient;

    public TelegramNotificationProvider(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }

    public string ProviderKey => "telegram";

    public async Task<NotificationProviderResult> SendAsync(
        NotificationProviderRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Route.DestinationId))
        {
            return NotificationProviderResult.Failure(
                "TelegramDestinationMissing",
                "Telegram destination chat id is not configured.");
        }

        var text = BuildMessageText(request);
        if (string.IsNullOrWhiteSpace(text))
        {
            return NotificationProviderResult.Failure(
                "TelegramMessageEmpty",
                "Telegram message text is empty.");
        }

        var telegramResult = await _telegramBotClient.SendMessageAsync(
            request.Route.DestinationId,
            text,
            cancellationToken);

        return telegramResult.Succeeded
            ? NotificationProviderResult.Success()
            : NotificationProviderResult.Failure(
                telegramResult.FailureCode!,
                telegramResult.FailureDescription);
    }

    private static string BuildMessageText(NotificationProviderRequest request)
    {
        var title = request.Notification.Title.Trim();
        var message = request.Notification.Message?.Trim();

        var text = string.IsNullOrWhiteSpace(message)
            ? title
            : string.Join(Environment.NewLine + Environment.NewLine, title, message);

        return TruncateMessage(text);
    }

    private static string TruncateMessage(string text)
    {
        if (text.Length <= MaxMessageLength)
        {
            return text;
        }

        var builder = new StringBuilder(MaxMessageLength);

        foreach (var rune in text.EnumerateRunes())
        {
            if (builder.Length + rune.Utf16SequenceLength > MaxMessageLength)
            {
                break;
            }

            builder.Append(rune);
        }

        return builder.ToString();
    }
}
