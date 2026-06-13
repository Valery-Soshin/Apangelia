using System.Text;
using Apangelia.Application;
using Apangelia.Integrations.GitHub;

namespace Apangelia.WebApi.Endpoints;

public static class WebhookEndpoints
{
    public static IEndpointRouteBuilder MapWebhookEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/webhooks/github", HandleGitHubWebhook);

        return app;
    }

    private static async Task<IResult> HandleGitHubWebhook(
        HttpRequest request,
        IGitHubWebhookReceiver gitHubWebhookReceiver,
        INotificationEventHandler notificationEventHandler,
        CancellationToken cancellationToken)
    {
        using var bodyBuffer = new MemoryStream();
        await request.Body.CopyToAsync(bodyBuffer, cancellationToken);

        var bodyBytes = bodyBuffer.ToArray();
        var body = Encoding.UTF8.GetString(bodyBytes);

        var receiveResult = await gitHubWebhookReceiver.ReceiveAsync(
            new GitHubWebhookReceiveRequest(
                request.Headers["X-Hub-Signature-256"].ToString(),
                request.Headers["X-GitHub-Delivery"].ToString(),
                request.Headers["X-GitHub-Event"].ToString(),
                body,
                bodyBytes),
            cancellationToken);

        if (receiveResult.Status == GitHubWebhookReceiveStatus.Unauthorized)
        {
            return Results.Unauthorized();
        }

        if (receiveResult.Status is GitHubWebhookReceiveStatus.MissingRequiredHeaders
            or GitHubWebhookReceiveStatus.InvalidPayload)
        {
            return Results.BadRequest();
        }

        await notificationEventHandler.HandleAsync(receiveResult.NotificationEvent!, cancellationToken);

        return Results.Accepted();
    }
}
