using Apangelia.WebApi.Endpoints;

namespace Apangelia.WebApi.Configuration;

public static class EndpointConfiguration
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapWebhookEndpoints();

        return app;
    }
}