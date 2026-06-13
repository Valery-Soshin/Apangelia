using Microsoft.OpenApi;

namespace Apangelia.WebApi.Configurations;

public static class SwaggerConfiguration
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Apangelia WebAPI"
            });
        });
    }

    public static void UseSwaggerConf(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Apangelia WebAPI v1");
            });
        }
    }
}
