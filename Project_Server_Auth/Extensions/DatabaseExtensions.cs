using DAL;
using Microsoft.EntityFrameworkCore;

namespace pr_srv_names.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' не найдена в конфигурации");
            }

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    // Настройки PostgreSQL
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorCodesToAdd: null);

                    npgsqlOptions.CommandTimeout(30);
                });

                // Настройки для разработки
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                }
            });

            return services;
        }

        public static async Task<WebApplication> EnsureDatabaseCreatedAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            try
            {
                logger.LogInformation("Проверяем подключение к базе данных...");

                // Проверяем, можем ли подключиться к БД
                var canConnect = await context.Database.CanConnectAsync();
                if (!canConnect)
                {
                    logger.LogError("Не удается подключиться к базе данных");
                    throw new InvalidOperationException("Не удается подключиться к базе данных");
                }

                // Применяем миграции
                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                    logger.LogInformation("Найдено {Count} неприменённых миграций. Применяем...",
                        pendingMigrations.Count());

                    await context.Database.MigrateAsync();
                    logger.LogInformation("Миграции успешно применены");
                }
                else
                {
                    logger.LogInformation("Все миграции уже применены");
                }

                logger.LogInformation("База данных готова к работе");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при настройке базы данных");
                throw;
            }

            return app;
        }

        public static async Task<WebApplication> SeedDatabaseAsync(this WebApplication app)
        {
            try
            {
                await DatabaseSeeder.SeedAsync(app.Services);
            }
            catch (Exception ex)
            {
                using var scope = app.Services.CreateScope();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Ошибка при инициализации данных в базе данных");
                throw;
            }

            return app;
        }

        public static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
        {
            // Комбинированный метод для полной инициализации БД
            await app.EnsureDatabaseCreatedAsync();
            await app.SeedDatabaseAsync();

            return app;
        }
    }
}