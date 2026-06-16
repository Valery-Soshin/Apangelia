using Apangelia.Application.NotificationProviders;
using Apangelia.Application.SeedWork;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Apangelia.Application.NotificationDeliveries;

/// <summary>
/// Обрабатывает очередь доставок уведомлений и обновляет состояние попыток отправки.
/// </summary>
public sealed class NotificationDeliveryProcessor
{
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
            }
            else
            {
                await MarkFailedAsync(
                    claimedDelivery,
                    result.ErrorCode,
                    result.ErrorMessage,
                    cancellationToken);
            }
        }
        catch (Exception exception) when (exception.IsNotExpectedCancellation(cancellationToken))
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

        return CompleteAttemptAsync(
            claimedDelivery,
            state =>
            {
                state.Attempt.MarkSucceeded(completedAt);
                state.Delivery.MarkDelivered(completedAt);
            },
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
        var normalizedErrorCode = NotificationDeliveryProcessorHelper.NormalizeErrorCode(errorCode);

        if (delivery.AttemptCount >= _options.MaxAttempts)
        {
            return CompleteAttemptAsync(
                claimedDelivery,
                state =>
                {
                    state.Attempt.MarkFailed(completedAt, normalizedErrorCode, errorMessage);
                    state.Delivery.MoveToDeadLetter(completedAt);
                },
                cancellationToken);
        }

        var nextAttemptAt = completedAt + NotificationDeliveryProcessorHelper
            .CalculateRetryDelay(delivery.AttemptCount, _options.InitialRetryDelay);

        return CompleteAttemptAsync(
            claimedDelivery,
            state =>
            {
                state.Attempt.MarkFailed(completedAt, normalizedErrorCode, errorMessage);
                state.Delivery.ScheduleRetry(completedAt, nextAttemptAt);
            },
            cancellationToken);
    }

    private async Task CompleteAttemptAsync(
        ClaimedNotificationDelivery claimedDelivery,
        Action<NotificationDeliveryAttemptState> updateState,
        CancellationToken cancellationToken)
    {
        var state = await _deliveryRepository.GetAttemptStateAsync(
            claimedDelivery.SendRequest.Delivery.Id,
            claimedDelivery.AttemptId,
            cancellationToken);

        updateState(state);

        await _deliveryRepository.SaveAttemptStateAsync(state, cancellationToken);
    }
}
