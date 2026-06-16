using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace Apangelia.Integrations.Telegram;

/// <summary>
/// HTTP-клиент используемых методов Telegram Bot API.
/// </summary>
public sealed class TelegramBotClient : ITelegramBotClient
{
    private readonly Uri _apiBaseUri;
    private readonly HttpClient _httpClient;
    private readonly TelegramNotificationOptions _options;

    public TelegramBotClient(
        HttpClient httpClient,
        IOptions<TelegramNotificationOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _apiBaseUri = CreateApiBaseUri(_options.ApiBaseUrl);
    }

    public Task<TelegramBotApiRequestResult> SendMessageAsync(
        string chatId,
        string text,
        CancellationToken cancellationToken)
    {
        return SendJsonRequestAsync(
            "sendMessage",
            new TelegramSendMessageRequest(chatId, text),
            cancellationToken);
    }

    public Task<TelegramBotApiRequestResult> SetWebhookAsync(
        TelegramWebhookRegistrationRequest request,
        CancellationToken cancellationToken)
    {
        return SendJsonRequestAsync(
            "setWebhook",
            new TelegramSetWebhookRequest(
                request.Url.ToString(),
                request.AllowedUpdates,
                request.DropPendingUpdates,
                request.SecretToken),
            cancellationToken);
    }

    private async Task<TelegramBotApiRequestResult> SendJsonRequestAsync<TRequest>(
        string methodName,
        TRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            using var response = await _httpClient.PostAsJsonAsync(
                CreateMethodUri(methodName),
                request,
                cancellationToken);

            var telegramResponse = await ReadTelegramResponseAsync(response, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return CreateHttpFailure(response.StatusCode, response.ReasonPhrase, telegramResponse);
            }

            if (telegramResponse is null)
            {
                return TelegramBotApiRequestResult.Failure(
                    "TelegramInvalidResponse",
                    "Telegram API returned an invalid JSON response.");
            }

            if (!telegramResponse.Ok)
            {
                return CreateTelegramFailure(telegramResponse);
            }

            return TelegramBotApiRequestResult.Success();
        }
        catch (HttpRequestException)
        {
            return TelegramBotApiRequestResult.Failure(
                "TelegramHttpRequestFailed",
                "Telegram request failed before receiving a response.");
        }
        catch (JsonException)
        {
            return TelegramBotApiRequestResult.Failure(
                "TelegramInvalidResponse",
                "Telegram API returned an invalid JSON response.");
        }
    }

    private Uri CreateMethodUri(string methodName)
    {
        return new Uri(_apiBaseUri, $"bot{_options.BotToken}/{methodName}");
    }

    private static Uri CreateApiBaseUri(string apiBaseUrl)
    {
        return apiBaseUrl.EndsWith("/", StringComparison.Ordinal)
            ? new Uri(apiBaseUrl, UriKind.Absolute)
            : new Uri($"{apiBaseUrl}/", UriKind.Absolute);
    }

    private static async Task<TelegramApiResponse?> ReadTelegramResponseAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        try
        {
            return await response.Content.ReadFromJsonAsync<TelegramApiResponse>(cancellationToken);
        }
        catch (JsonException) when (!response.IsSuccessStatusCode)
        {
            return null;
        }
    }

    private static TelegramBotApiRequestResult CreateHttpFailure(
        HttpStatusCode statusCode,
        string? reasonPhrase,
        TelegramApiResponse? telegramResponse)
    {
        if (telegramResponse is not null)
        {
            return CreateTelegramFailure(telegramResponse);
        }

        return TelegramBotApiRequestResult.Failure(
            $"TelegramHttp{(int)statusCode}",
            reasonPhrase);
    }

    private static TelegramBotApiRequestResult CreateTelegramFailure(TelegramApiResponse? response)
    {
        var errorCode = response?.ErrorCode is null
            ? "TelegramApiRequestFailed"
            : $"TelegramApi{response.ErrorCode}";

        return TelegramBotApiRequestResult.Failure(errorCode, response?.Description);
    }

    private sealed record TelegramSendMessageRequest(
        [property: JsonPropertyName("chat_id")] string ChatId,
        [property: JsonPropertyName("text")] string Text);

    private sealed record TelegramSetWebhookRequest(
        [property: JsonPropertyName("url")] string Url,
        [property: JsonPropertyName("allowed_updates")] IReadOnlyCollection<string> AllowedUpdates,
        [property: JsonPropertyName("drop_pending_updates")] bool DropPendingUpdates,
        [property: JsonPropertyName("secret_token")] string SecretToken);

    private sealed record TelegramApiResponse(
        [property: JsonPropertyName("ok")] bool Ok,
        [property: JsonPropertyName("error_code")] int? ErrorCode,
        [property: JsonPropertyName("description")] string? Description);
}
