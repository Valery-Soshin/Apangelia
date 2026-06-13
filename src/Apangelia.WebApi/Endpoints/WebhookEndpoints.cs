using System.Text;
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
        IGitHubWebhookHandler gitHubWebhookHandler,
        CancellationToken cancellationToken)
    {
        using var bodyBuffer = new MemoryStream();
        await request.Body.CopyToAsync(bodyBuffer, cancellationToken);

        var bodyBytes = bodyBuffer.ToArray();
        var body = Encoding.UTF8.GetString(bodyBytes);

        var handlingResult = await gitHubWebhookHandler.HandleAsync(
            new GitHubWebhookReceiveRequest(
                request.Headers["X-Hub-Signature-256"].ToString(),
                request.Headers["X-GitHub-Delivery"].ToString(),
                request.Headers["X-GitHub-Event"].ToString(),
                body,
                bodyBytes),
            cancellationToken);

        if (handlingResult.Status == GitHubWebhookReceiveStatus.Unauthorized)
        {
            return Results.Unauthorized();
        }

        if (handlingResult.Status is GitHubWebhookReceiveStatus.MissingRequiredHeaders
            or GitHubWebhookReceiveStatus.InvalidPayload)
        {
            return Results.BadRequest();
        }

        return Results.Accepted();
    }
}
