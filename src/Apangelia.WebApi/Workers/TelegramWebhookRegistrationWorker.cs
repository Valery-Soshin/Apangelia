using Apangelia.Integrations.Telegram;
using Microsoft.Extensions.Options;

namespace Apangelia.WebApi.Workers;

/// <summary>
/// Регистрирует Telegram webhook при старте приложения, если регистрация включена в конфигурации.
/// </summary>
public sealed class TelegramWebhookRegistrationWorker : IHostedService
{
    private readonly ILogger<TelegramWebhookRegistrationWorker> _logger;
    private readonly TelegramNotificationOptions _telegramOptions;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly TelegramWebhookRegistrationOptions _webhookOptions;

    public TelegramWebhookRegistrationWorker(
        ITelegramBotClient telegramBotClient,
        IOptions<TelegramNotificationOptions> telegramOptions,
        IOptions<TelegramWebhookRegistrationOptions> webhookOptions,
        ILogger<TelegramWebhookRegistrationWorker> logger)
    {
        _telegramBotClient = telegramBotClient;
        _telegramOptions = telegramOptions.Value;
        _webhookOptions = webhookOptions.Value;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!_webhookOptions.Enabled)
        {
            _logger.LogDebug("Telegram webhook registration is disabled.");
            return;
        }

        var webhookUrl = new Uri(_webhookOptions.PublicUrl, UriKind.Absolute);
        var request = new TelegramWebhookRegistrationRequest(
            webhookUrl,
            _webhookOptions.AllowedUpdates,
            _webhookOptions.DropPendingUpdates,
            _telegramOptions.WebhookSecretToken);

        var result = await _telegramBotClient.SetWebhookAsync(request, cancellationToken);

        if (result.Succeeded)
        {
            _logger.LogInformation(
                "Telegram webhook was registered for {WebhookUrl}.",
                webhookUrl);

            return;
        }

        _logger.LogError(
            "Telegram webhook registration failed with {FailureCode}: {FailureDescription}.",
            result.FailureCode,
            result.FailureDescription);

        throw new InvalidOperationException(
            $"Telegram webhook registration failed with {result.FailureCode}.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
