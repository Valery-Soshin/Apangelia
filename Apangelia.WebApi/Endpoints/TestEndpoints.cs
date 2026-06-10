namespace Apangelia.WebApi.Endpoints;

public static class TestEndpoints
{
    public static IEndpointRouteBuilder MapTestEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", () => GetAll());
        return app;
    }

    private static async Task<string> GetAll()
    {
        return await Task.FromResult("Hello, World!");
    }
}
