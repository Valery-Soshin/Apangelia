using Apangelia.Persistence.Postgres;
using Microsoft.EntityFrameworkCore;

namespace Apangelia.WebApi.Configurations;

public static class EntityFrameworkConfiguration
{
    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        context.Database.Migrate();

        return app;
    }
}
