namespace Apangelia.Application.NotificationDeliveries.ProcessNotificationDeliveryBatch;

internal static class NotificationDeliveryProcessorHelper
{
    public const int MaxErrorCodeLength = 128;
    public const string ProviderFailureErrorCode = "ProviderFailure";

    public static TimeSpan CalculateRetryDelay(int attemptCount, TimeSpan initialRetryDelay)
    {
        var multiplier = 1L << Math.Max(0, attemptCount - 1);

        return TimeSpan.FromTicks(initialRetryDelay.Ticks * multiplier);
    }

    public static string NormalizeErrorCode(string? errorCode)
    {
        var normalizedErrorCode = string.IsNullOrWhiteSpace(errorCode)
            ? ProviderFailureErrorCode
            : errorCode;

        return normalizedErrorCode.Length <= MaxErrorCodeLength
            ? normalizedErrorCode
            : normalizedErrorCode[..MaxErrorCodeLength];
    }
}