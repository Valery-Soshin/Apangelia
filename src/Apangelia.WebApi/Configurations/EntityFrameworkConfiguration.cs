using Apangelia.Persistence.Postgres;
using Apangelia.WebApi.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Apangelia.WebApi.Configurations;

public static class EntityFrameworkConfiguration
{
    public static void AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(configuration);
    }

    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        context.Database.Migrate();

        return app;
    }

    private static void AddDbContext<TDbContext>(this IServiceCollection services, IConfiguration configuration) where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>((serviceProvider, options) =>
        {
            var environment = serviceProvider.GetRequiredService<IHostEnvironment>();

            options.UseNpgsql(configuration["ConnectionStrings:Postgres"]);

            if (environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
            }
        });
    }
}
