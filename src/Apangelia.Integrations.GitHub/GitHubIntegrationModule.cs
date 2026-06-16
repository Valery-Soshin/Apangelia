using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Apangelia.Integrations.GitHub;

/// <summary>
/// Регистрирует зависимости интеграции с GitHub.
/// </summary>
public static class GitHubIntegrationModule
{
    /// <summary>
    /// Добавляет настройки и обработчики входящих GitHub webhook-событий.
    /// </summary>
    public static IServiceCollection AddGitHubIntegrationModule(this IServiceCollection services, IConfigurationSection configurationSection)
    {
        services.AddOptions<GitHubWebhookOptions>()
            .Bind(configurationSection)
            .Validate(options => !string.IsNullOrWhiteSpace(options.WebhookSecret), "GitHub webhook secret must be configured.")
            .ValidateOnStart();

        services.AddScoped<IGitHubWebhookReceiver, GitHubWebhookReceiver>();
        services.AddScoped<IGitHubWebhookHandler, GitHubWebhookHandler>();

        return services;
    }
}
