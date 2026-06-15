using Apangelia.Application.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Apangelia.Application.Notifications;

/// <summary>
/// Обрабатывает очередь доставок уведомлений и обновляет состояние попыток отправки.
/// </summary>
public sealed class NotificationDeliveryProcessor
{
    private const int MaxErrorCodeLength = 128;
    private const string ProviderFailureErrorCode = "ProviderFailure";

    private readonly INotificationDeliveryRepository _deliveryRepository;
    private readonly ILogger<NotificationDeliveryProcessor> _logger;
    private readonly NotificationDeliveryWorkerOptions _options;
    private readonly INotificationProviderResolver _providerResolver;

    public NotificationDeliveryProcessor(
        INotificationDeliveryRepository deliveryRepository,
        INotificationProviderResolver providerResolver,
        IOptions<NotificationDeliveryWorkerOptions> options,
        ILogger<NotificationDeliveryProcessor> logger)
    {
        _deliveryRepository = deliveryRepository;
        _providerResolver = providerResolver;
        _options = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// Закрепляет и обрабатывает одну пачку готовых доставок.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Количество закрепленных доставок.</returns>
    public async Task<int> ProcessNextBatchAsync(CancellationToken cancellationToken)
    {
        var currentDate = DateTimeOffset.UtcNow;

        var claimedDeliveries = await _deliveryRepository.ClaimForProcessingAsync(
            _options.BatchSize,
            currentDate,
            cancellationToken);

        foreach (var claimedDelivery in claimedDeliveries)
        {
            await ProcessDeliveryAsync(claimedDelivery, cancellationToken);
        }

        return claimedDeliveries.Count;
    }

    private async Task ProcessDeliveryAsync(
        ClaimedNotificationDelivery claimedDelivery,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = claimedDelivery.SendRequest;
            var provider = _providerResolver.Resolve(request.Route.OutputProviderId);
            var result = await provider.SendAsync(request, cancellationToken);

            if (result.IsSuccess)
            {
                await MarkDeliveredAsync(claimedDelivery, cancellationToken);
                return;
            }

            await MarkFailedAsync(
                claimedDelivery,
                result.ErrorCode,
                result.ErrorMessage,
                cancellationToken);
        }
        catch (Exception exception) when (exception is not OperationCanceledException || !cancellationToken.IsCancellationRequested)
        {
            _logger.LogWarning(
                exception,
                "Notification delivery '{DeliveryId}' failed during provider processing.",
                claimedDelivery.SendRequest.Delivery.Id);

            await MarkFailedAsync(
                claimedDelivery,
                exception.GetType().Name,
                exception.Message,
                cancellationToken);
        }
    }

    private Task MarkDeliveredAsync(
        ClaimedNotificationDelivery claimedDelivery,
        CancellationToken cancellationToken)
    {
        var completedAt = DateTimeOffset.UtcNow;

        return _deliveryRepository.MarkDeliveredAsync(
            claimedDelivery.SendRequest.Delivery.Id,
            claimedDelivery.AttemptId,
            completedAt,
            cancellationToken);
    }

    private Task MarkFailedAsync(
        ClaimedNotificationDelivery claimedDelivery,
        string? errorCode,
        string? errorMessage,
        CancellationToken cancellationToken)
    {
        var completedAt = DateTimeOffset.UtcNow;
        var delivery = claimedDelivery.SendRequest.Delivery;
        var normalizedErrorCode = NormalizeErrorCode(errorCode);

        if (delivery.AttemptCount >= _options.MaxAttempts)
        {
            return _deliveryRepository.MoveToDeadLetterAsync(
                delivery.Id,
                claimedDelivery.AttemptId,
                completedAt,
                normalizedErrorCode,
                errorMessage,
                cancellationToken);
        }

        var nextAttemptAt = completedAt + CalculateRetryDelay(delivery.AttemptCount, _options.InitialRetryDelay);

        return _deliveryRepository.ScheduleRetryAsync(
            delivery.Id,
            claimedDelivery.AttemptId,
            completedAt,
            nextAttemptAt,
            normalizedErrorCode,
            errorMessage,
            cancellationToken);
    }

    private static TimeSpan CalculateRetryDelay(int attemptCount, TimeSpan initialRetryDelay)
    {
        var multiplier = 1L << Math.Max(0, attemptCount - 1);

        return TimeSpan.FromTicks(initialRetryDelay.Ticks * multiplier);
    }

    private static string NormalizeErrorCode(string? errorCode)
    {
        var normalizedErrorCode = string.IsNullOrWhiteSpace(errorCode)
            ? ProviderFailureErrorCode
            : errorCode;

        return normalizedErrorCode.Length <= MaxErrorCodeLength
            ? normalizedErrorCode
            : normalizedErrorCode[..MaxErrorCodeLength];
    }
}
