using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Apangelia.Application.Notifications.RegisterNotificationRoute;
using Microsoft.Extensions.Options;

namespace Apangelia.Integrations.Telegram;

/// <summary>
/// Проверяет Telegram webhook и преобразует команду /start в регистрацию маршрута уведомлений.
/// </summary>
public sealed class TelegramWebhookReceiver : ITelegramWebhookReceiver
{
    private const string InputProviderId = "github";
    private const string OutputProviderId = "telegram";
    private const string PrivateChatType = "private";
    private const string StartCommand = "/start";

    private readonly TelegramNotificationOptions _options;

    public TelegramWebhookReceiver(IOptions<TelegramNotificationOptions> options)
    {
        _options = options.Value;
    }

    public ValueTask<TelegramWebhookReceiveResult> ReceiveAsync(
        TelegramWebhookReceiveRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!HasValidSecretToken(request.SecretToken))
        {
            return ValueTask.FromResult(TelegramWebhookReceiveResult.Unauthorized());
        }

        return ValueTask.FromResult(TryCreateCommand(request.BodyBytes));
    }

    private bool HasValidSecretToken(string? secretToken)
    {
        if (string.IsNullOrWhiteSpace(_options.WebhookSecretToken))
        {
            throw new InvalidOperationException("Telegram webhook secret token is not configured.");
        }

        if (string.IsNullOrWhiteSpace(secretToken))
        {
            return false;
        }

        var expectedTokenBytes = Encoding.UTF8.GetBytes(_options.WebhookSecretToken);
        var providedTokenBytes = Encoding.UTF8.GetBytes(secretToken);

        return expectedTokenBytes.Length == providedTokenBytes.Length
            && CryptographicOperations.FixedTimeEquals(expectedTokenBytes, providedTokenBytes);
    }

    private static TelegramWebhookReceiveResult TryCreateCommand(ReadOnlyMemory<byte> bodyBytes)
    {
        try
        {
            using var payload = JsonDocument.Parse(bodyBytes);

            if (payload.RootElement.ValueKind != JsonValueKind.Object)
            {
                return TelegramWebhookReceiveResult.InvalidPayload();
            }

            if (!payload.RootElement.TryGetProperty("message", out var messageElement))
            {
                return TelegramWebhookReceiveResult.Ignored();
            }

            if (messageElement.ValueKind != JsonValueKind.Object)
            {
                return TelegramWebhookReceiveResult.InvalidPayload();
            }

            if (!TryGetString(messageElement, "text", out var text) || !IsStartCommand(text))
            {
                return TelegramWebhookReceiveResult.Ignored();
            }

            var chatStatus = TryGetPrivateChatId(messageElement, out var chatId);
            if (chatStatus == TelegramWebhookReceiveStatus.Ignored)
            {
                return TelegramWebhookReceiveResult.Ignored();
            }

            if (chatStatus == TelegramWebhookReceiveStatus.InvalidPayload)
            {
                return TelegramWebhookReceiveResult.InvalidPayload();
            }

            var command = new RegisterNotificationRouteCommand(
                InputProviderId,
                OutputProviderId,
                chatId.ToString(CultureInfo.InvariantCulture));

            return TelegramWebhookReceiveResult.Accepted(command);
        }
        catch (JsonException)
        {
            return TelegramWebhookReceiveResult.InvalidPayload();
        }
    }

    private static TelegramWebhookReceiveStatus TryGetPrivateChatId(JsonElement messageElement, out long chatId)
    {
        chatId = default;

        if (!messageElement.TryGetProperty("chat", out var chatElement)
            || chatElement.ValueKind != JsonValueKind.Object
            || !TryGetString(chatElement, "type", out var chatType)
            || !chatElement.TryGetProperty("id", out var chatIdElement))
        {
            return TelegramWebhookReceiveStatus.InvalidPayload;
        }

        if (!string.Equals(chatType, PrivateChatType, StringComparison.Ordinal))
        {
            return TelegramWebhookReceiveStatus.Ignored;
        }

        return chatIdElement.TryGetInt64(out chatId)
            ? TelegramWebhookReceiveStatus.Accepted
            : TelegramWebhookReceiveStatus.InvalidPayload;
    }

    private static bool TryGetString(JsonElement element, string propertyName, out string value)
    {
        value = string.Empty;

        if (!element.TryGetProperty(propertyName, out var propertyElement)
            || propertyElement.ValueKind != JsonValueKind.String)
        {
            return false;
        }

        value = propertyElement.GetString() ?? string.Empty;

        return true;
    }

    private static bool IsStartCommand(string text)
    {
        var trimmedText = text.TrimStart();

        if (!trimmedText.StartsWith(StartCommand, StringComparison.Ordinal))
        {
            return false;
        }

        return trimmedText.Length == StartCommand.Length
            || char.IsWhiteSpace(trimmedText[StartCommand.Length])
            || trimmedText[StartCommand.Length] == '@';
    }
}
