using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Apangelia.Application.Commands.AcceptNotificationEvent;
using Microsoft.Extensions.Options;

namespace Apangelia.Integrations.GitHub;

/// <summary>
/// Проверяет подпись GitHub webhook и преобразует доставку в событие уведомления.
/// </summary>
public sealed class GitHubWebhookReceiver : IGitHubWebhookReceiver
{
    private const string SignaturePrefix = "sha256=";
    private const string Source = "github";

    private readonly GitHubWebhookOptions _options;

    public GitHubWebhookReceiver(IOptions<GitHubWebhookOptions> options)
    {
        _options = options.Value;
    }

    public ValueTask<GitHubWebhookReceiveResult> ReceiveAsync(
        GitHubWebhookReceiveRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(request.DeliveryId) || string.IsNullOrWhiteSpace(request.EventType))
        {
            return ValueTask.FromResult(GitHubWebhookReceiveResult.MissingRequiredHeaders());
        }

        if (!HasValidSignature(request.BodyBytes.Span, request.Signature))
        {
            return ValueTask.FromResult(GitHubWebhookReceiveResult.Unauthorized());
        }

        return TryCreateCommand(request, out var command)
            ? ValueTask.FromResult(GitHubWebhookReceiveResult.Accepted(command))
            : ValueTask.FromResult(GitHubWebhookReceiveResult.InvalidPayload());
    }

    private bool HasValidSignature(ReadOnlySpan<byte> bodyBytes, string? signatureHeader)
    {
        if (string.IsNullOrWhiteSpace(_options.WebhookSecret))
        {
            throw new InvalidOperationException("GitHub webhook secret is not configured.");
        }

        if (string.IsNullOrWhiteSpace(signatureHeader)
            || !signatureHeader.StartsWith(SignaturePrefix, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var signatureHex = signatureHeader[SignaturePrefix.Length..].Trim();

        byte[] providedHash;

        try
        {
            providedHash = Convert.FromHexString(signatureHex);
        }
        catch (FormatException)
        {
            return false;
        }

        var secretBytes = Encoding.UTF8.GetBytes(_options.WebhookSecret);
        var expectedHash = HMACSHA256.HashData(secretBytes, bodyBytes);

        return CryptographicOperations.FixedTimeEquals(providedHash, expectedHash);
    }

    private static bool TryCreateCommand(
        GitHubWebhookReceiveRequest request,
        out AcceptNotificationEventCommand command)
    {
        try
        {
            using var payload = JsonDocument.Parse(request.BodyBytes);

            if (payload.RootElement.ValueKind != JsonValueKind.Object)
            {
                command = default!;
                return false;
            }

            var action = TryGetAction(payload.RootElement);
            var title = string.IsNullOrWhiteSpace(action)
                ? $"GitHub {request.EventType}"
                : $"GitHub {request.EventType}: {action}";

            command = new AcceptNotificationEventCommand(
                Source,
                request.EventType!,
                request.DeliveryId!,
                title,
                null,
                request.Body,
                DateTimeOffset.UtcNow);

            return true;
        }
        catch (JsonException)
        {
            command = default!;
            return false;
        }
    }

    private static string? TryGetAction(JsonElement root)
    {
        return root.TryGetProperty("action", out var actionElement) && actionElement.ValueKind == JsonValueKind.String
            ? actionElement.GetString()
            : null;
    }
}
