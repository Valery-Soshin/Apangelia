using Apangelia.Application.Notifications.RegisterNotificationRoute;
using Apangelia.Application.Shared.CommandBase;

namespace Apangelia.Integrations.Telegram;

public sealed class TelegramWebhookHandler : ITelegramWebhookHandler
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly ITelegramWebhookReceiver _telegramWebhookReceiver;

    public TelegramWebhookHandler(
        ITelegramWebhookReceiver telegramWebhookReceiver,
        ICommandDispatcher commandDispatcher)
    {
        _telegramWebhookReceiver = telegramWebhookReceiver;
        _commandDispatcher = commandDispatcher;
    }

    public async Task<TelegramWebhookHandlingResult> HandleAsync(
        TelegramWebhookReceiveRequest request,
        CancellationToken cancellationToken)
    {
        var receiveResult = await _telegramWebhookReceiver.ReceiveAsync(request, cancellationToken);

        if (receiveResult.Status != TelegramWebhookReceiveStatus.Accepted)
        {
            return TelegramWebhookHandlingResult.FromReceiveStatus(receiveResult.Status);
        }

        var handlingResult = await _commandDispatcher.HandleAsync(
            receiveResult.Command!,
            cancellationToken);

        return handlingResult switch
        {
            RegisterNotificationRouteResult.Registered => TelegramWebhookHandlingResult.Accepted(),
            _ => throw new InvalidOperationException($"Unsupported notification route registration result: {handlingResult}.")
        };
    }
}
