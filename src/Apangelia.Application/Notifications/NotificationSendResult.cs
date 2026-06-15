namespace Apangelia.Application.Notifications;

/// <summary>
/// Результат отправки уведомления исходящим провайдером.
/// </summary>
/// <param name="IsSuccess">Признак успешной отправки.</param>
/// <param name="ErrorCode">Машиночитаемый код ошибки отправки.</param>
/// <param name="ErrorMessage">Диагностическое сообщение ошибки отправки.</param>
public sealed record NotificationSendResult(
    bool IsSuccess,
    string? ErrorCode = null,
    string? ErrorMessage = null)
{
    /// <summary>
    /// Создает успешный результат отправки.
    /// </summary>
    /// <returns>Успешный результат отправки.</returns>
    public static NotificationSendResult Success()
    {
        return new NotificationSendResult(true);
    }

    /// <summary>
    /// Создает неуспешный результат отправки.
    /// </summary>
    /// <param name="errorCode">Машиночитаемый код ошибки.</param>
    /// <param name="errorMessage">Диагностическое сообщение ошибки.</param>
    /// <returns>Неуспешный результат отправки.</returns>
    public static NotificationSendResult Failure(string? errorCode, string? errorMessage)
    {
        return new NotificationSendResult(false, errorCode, errorMessage);
    }
}
