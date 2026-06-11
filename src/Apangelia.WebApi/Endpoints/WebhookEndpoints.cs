using Apangelia.Core;

namespace Apangelia.WebApi.Endpoints;

public static class WebhookEndpoints
{
    public static IEndpointRouteBuilder MapWebhookEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/webhooks/github", HandleGitHubWebhook);

        return app;
    }

    private static async Task<IResult> HandleGitHubWebhook(HttpRequest request, CancellationToken cancellationToken)
    {
        var signature = request.Headers["X-Hub-Signature-256"].ToString();
        var deliveryId = request.Headers["X-GitHub-Delivery"].ToString();
        var eventType = request.Headers["X-GitHub-Event"].ToString();

        using var reader = new StreamReader(request.Body);
        var body = await reader.ReadToEndAsync(cancellationToken);


        return Results.Ok();
    }
}
