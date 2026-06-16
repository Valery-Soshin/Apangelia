using Apangelia.Application.Abstractions.Persistence;
using Apangelia.Persistence.Postgres.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Apangelia.Persistence.Postgres;

/// <summary>
/// Регистрирует PostgreSQL-хранилище и его реализации портов приложения.
/// </summary>
public static class PostgresPersistenceModule
{
    /// <summary>
    /// Добавляет контекст базы данных, Npgsql-настройки и репозитории PostgreSQL.
    /// </summary>
    public static IServiceCollection AddPostgresPersistenceModule(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            var environment = serviceProvider.GetRequiredService<IHostEnvironment>();

            options.UseNpgsql(connectionString);

            if (environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
            }
        });

        services.AddScoped<INotificationInboxRepository, PostgresNotificationInboxRepository>();
        services.AddScoped<INotificationRepository, PostgresNotificationRepository>();
        services.AddScoped<INotificationRouteRepository, PostgresNotificationRouteRepository>();
        services.AddScoped<INotificationDeliveryRepository, PostgresNotificationDeliveryRepository>();
        services.AddScoped<IUnitOfWork, PostgresUnitOfWork>();

        return services;
    }
}
