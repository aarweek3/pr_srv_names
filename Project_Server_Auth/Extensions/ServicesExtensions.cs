using pr_srv_names.Services;
using pr_srv_names.Services.Interfaces;

namespace pr_srv_names.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Бизнес-сервисы приложения
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IActivityLogService, ActivityLogService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ISimpleAdminService, SimpleAdminService>();


            return services;
        }

        public static IServiceCollection AddCacheServices(this IServiceCollection services)
        {
            // Кэширование для rate limiting и других нужд
            services.AddMemoryCache();

            return services;
        }

        public static IServiceCollection AddControllerServices(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                // Настройки контроллеров
                options.SuppressAsyncSuffixInActionNames = false;
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                // Настройка поведения API
                options.SuppressModelStateInvalidFilter = false;
                options.SuppressMapClientErrors = false;
            });

            return services;
        }

        public static IServiceCollection RegisterAllServices(this IServiceCollection services)
        {
            // Метод для регистрации всех сервисов одним вызовом (без Swagger)
            services.AddApplicationServices();
            services.AddCacheServices();
            services.AddControllerServices();
            // Swagger регистрируется отдельно через существующий SwaggerExtensions

            return services;
        }
    }
}