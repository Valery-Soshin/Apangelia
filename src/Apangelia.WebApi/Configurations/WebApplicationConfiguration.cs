namespace Apangelia.WebApi.Configurations;

public static class WebApplicationConfiguration
{
    public static void UseConfiguration(this WebApplication app)
    {
        app.ApplyMigrations();

        app.UseSwaggerConf();

        app.UseHttpLogging();

        app.MapDefaultEndpoints();

        app.MapEndpoints();
    }
}