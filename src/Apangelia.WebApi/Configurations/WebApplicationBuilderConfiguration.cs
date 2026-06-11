namespace Apangelia.WebApi.Configurations;

public static class WebApplicationBuilderConfiguration
{
    public static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();

        builder.Services.ConfigureServices(builder.Configuration);

        return builder;
    }
}