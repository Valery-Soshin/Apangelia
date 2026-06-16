using Apangelia.Application.Commands.AcceptNotificationEvent;
using Apangelia.Application.SeedWork;

namespace Apangelia.Integrations.GitHub;

public sealed class GitHubWebhookHandler : IGitHubWebhookHandler
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IGitHubWebhookReceiver _gitHubWebhookReceiver;

    public GitHubWebhookHandler(
        IGitHubWebhookReceiver gitHubWebhookReceiver,
        ICommandDispatcher commandDispatcher)
    {
        _gitHubWebhookReceiver = gitHubWebhookReceiver;
        _commandDispatcher = commandDispatcher;
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

        var handlingResult = await _commandDispatcher.HandleAsync(
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
