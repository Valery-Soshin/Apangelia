using Apangelia.Application.Notifications.AcceptEvent;

namespace Apangelia.Integrations.GitHub;

public sealed class GitHubWebhookHandler : IGitHubWebhookHandler
{
    private readonly IAcceptNotificationEventCommandHandler _acceptNotificationEventCommandHandler;
    private readonly IGitHubWebhookReceiver _gitHubWebhookReceiver;

    public GitHubWebhookHandler(
        IGitHubWebhookReceiver gitHubWebhookReceiver,
        IAcceptNotificationEventCommandHandler acceptNotificationEventCommandHandler)
    {
        _gitHubWebhookReceiver = gitHubWebhookReceiver;
        _acceptNotificationEventCommandHandler = acceptNotificationEventCommandHandler;
    }

    public async Task<GitHubWebhookHandlingResult> HandleAsync(
        GitHubWebhookReceiveRequest request,
        CancellationToken cancellationToken)
    {
        var receiveResult = await _gitHubWebhookReceiver.ReceiveAsync(request, cancellationToken);

        if (receiveResult.Status != GitHubWebhookReceiveStatus.Accepted)
        {
            return GitHubWebhookHandlingResult.FromReceiveStatus(receiveResult.Status);
        }

        var handlingResult = await _acceptNotificationEventCommandHandler.HandleAsync(
            new AcceptNotificationEventCommand(receiveResult.NotificationEvent!),
            cancellationToken);

        return handlingResult switch
        {
            AcceptNotificationEventResult.Accepted or AcceptNotificationEventResult.Duplicate
                => GitHubWebhookHandlingResult.Accepted(),
            _ => throw new InvalidOperationException($"Unsupported notification event handling result: {handlingResult}.")
        };
    }
}
