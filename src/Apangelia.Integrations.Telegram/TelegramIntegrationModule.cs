using Apangelia.Application.Abstractions.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Apangelia.Integrations.Telegram;

/// <summary>
/// Регистрирует зависимости интеграции с Telegram.
/// </summary>
public static class TelegramIntegrationModule
{
    /// <summary>
    /// Добавляет настройки, HTTP-клиент, провайдер уведомлений и webhook-обработчики Telegram.
    /// </summary>
    public static IServiceCollection AddTelegramIntegrationModule(this IServiceCollection services, IConfigurationSection configurationSection)
    {
        services.AddOptions<TelegramNotificationOptions>()
            .Bind(configurationSection)
            .Validate(TelegramNotificationOptions.IsValidApiBaseUrl, "Telegram API base URL must be an absolute HTTPS URL.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.BotToken), "Telegram bot token must be configured.")
            .Validate(TelegramNotificationOptions.IsValidWebhookSecretToken, "Telegram webhook secret token must contain 1-256 characters: A-Z, a-z, 0-9, _ or -.")
            .ValidateOnStart();

        services.AddHttpClient<ITelegramBotClient, TelegramBotClient>()
            .RemoveAllLoggers();

        services.AddScoped<INotificationProvider, TelegramNotificationProvider>();
        services.AddScoped<ITelegramWebhookReceiver, TelegramWebhookReceiver>();
        services.AddScoped<ITelegramWebhookHandler, TelegramWebhookHandler>();

        return services;
    }
}
