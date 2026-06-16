namespace Apangelia.Integrations.Telegram;

/// <summary>
/// Результат выполнения запроса к Telegram Bot API.
/// </summary>
/// <param name="Succeeded">Признак успешного выполнения запроса.</param>
/// <param name="FailureCode">Код ошибки для неуспешного запроса.</param>
/// <param name="FailureDescription">Описание ошибки для неуспешного запроса.</param>
public sealed record TelegramBotApiRequestResult(
    bool Succeeded,
    string? FailureCode,
    string? FailureDescription)
{
    /// <summary>
    /// Создает успешный результат запроса.
    /// </summary>
    /// <returns>Успешный результат запроса.</returns>
    public static TelegramBotApiRequestResult Success()
    {
        return new TelegramBotApiRequestResult(true, null, null);
    }

    /// <summary>
    /// Создает неуспешный результат запроса.
    /// </summary>
    /// <param name="failureCode">Код ошибки.</param>
    /// <param name="failureDescription">Описание ошибки.</param>
    /// <returns>Неуспешный результат запроса.</returns>
    public static TelegramBotApiRequestResult Failure(string failureCode, string? failureDescription)
    {
        return new TelegramBotApiRequestResult(false, failureCode, failureDescription);
    }
}
