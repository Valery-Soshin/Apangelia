using Apangelia.WebApi.Endpoints;

namespace Apangelia.WebApi.Configurations;

public static class EndpointConfiguration
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapWebhookEndpoints();

        return app;
    }
}