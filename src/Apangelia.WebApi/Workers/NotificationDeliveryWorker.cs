using Apangelia.Application.Commands.ProcessNotificationDeliveryBatch;
using Apangelia.Application.SeedWork;
using Microsoft.Extensions.Options;

namespace Apangelia.WebApi.Workers;

/// <summary>
/// Фоновый worker доставки уведомлений во внешние исходящие каналы.
/// </summary>
public sealed class NotificationDeliveryWorker : BackgroundService
{
    private readonly ILogger<NotificationDeliveryWorker> _logger;
    private readonly NotificationDeliveryWorkerOptions _options;
    private readonly IServiceScopeFactory _scopeFactory;

    public NotificationDeliveryWorker(
        IServiceScopeFactory scopeFactory,
        IOptions<NotificationDeliveryWorkerOptions> options,
        ILogger<NotificationDeliveryWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _options = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Notification delivery worker started.");

        try
        {
            await ProcessNextBatchAsync(stoppingToken);

            using var timer = new PeriodicTimer(_options.PollingInterval);
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await ProcessNextBatchAsync(stoppingToken);
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // Отмена при остановке host-а является штатным завершением worker-а.
        }

        _logger.LogInformation("Notification delivery worker stopped.");
    }

    private async Task ProcessNextBatchAsync(CancellationToken stoppingToken)
    {
        try
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var dispatcher = scope.ServiceProvider.GetRequiredService<ICommandDispatcher>();

            var processedCount = await dispatcher.HandleAsync(
                new ProcessNotificationDeliveryBatchCommand(),
                stoppingToken);

            if (processedCount > 0)
            {
                _logger.LogInformation(
                    "Notification delivery worker processed {ProcessedCount} deliveries.",
                    processedCount);
            }
        }
        catch (Exception exception) when (exception.IsNotExpectedCancellation(stoppingToken))
        {
            _logger.LogError(
                exception,
                "Notification delivery worker failed while processing a batch.");
        }
    }
}
