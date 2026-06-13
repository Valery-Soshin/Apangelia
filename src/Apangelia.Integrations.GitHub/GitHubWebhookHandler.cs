using Apangelia.Application;

namespace Apangelia.Integrations.GitHub;

public sealed class GitHubWebhookHandler : IGitHubWebhookHandler
{
    private readonly IGitHubWebhookReceiver _gitHubWebhookReceiver;
    private readonly INotificationEventHandler _notificationEventHandler;

    public GitHubWebhookHandler(
        IGitHubWebhookReceiver gitHubWebhookReceiver,
        INotificationEventHandler notificationEventHandler)
    {
        _gitHubWebhookReceiver = gitHubWebhookReceiver;
        _notificationEventHandler = notificationEventHandler;
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

        var handlingResult = await _notificationEventHandler.HandleAsync(
            receiveResult.NotificationEvent!,
            cancellationToken);

        return handlingResult switch
        {
            NotificationEventHandlingResult.Accepted or NotificationEventHandlingResult.Duplicate
                => GitHubWebhookHandlingResult.Accepted(),
            _ => throw new InvalidOperationException($"Unsupported notification event handling result: {handlingResult}.")
        };
    }
}
