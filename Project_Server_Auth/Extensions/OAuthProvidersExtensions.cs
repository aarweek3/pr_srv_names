namespace pr_srv_names.Extensions
{
    /// <summary>
    /// Extension методы для настройки внешних OAuth провайдеров
    /// </summary>
    public static class OAuthProvidersExtensions
    {
        /// <summary>
        /// Добавляет внешних OAuth провайдеров (Google, Facebook и др.)
        /// </summary>
        public static IServiceCollection AddOAuthProviders(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var authBuilder = services.AddAuthentication();

            // Google OAuth
            var googleClientId = configuration["Authentication:Google:ClientId"];
            var googleClientSecret = configuration["Authentication:Google:ClientSecret"];

            if (!string.IsNullOrWhiteSpace(googleClientId) && 
                !string.IsNullOrWhiteSpace(googleClientSecret))
            {
                authBuilder.AddGoogle(options =>
                {
                    options.ClientId = googleClientId;
                    options.ClientSecret = googleClientSecret;
                    options.CallbackPath = "/signin-google";
                });
            }

            // Facebook OAuth
            var facebookAppId = configuration["Authentication:Facebook:AppId"];
            var facebookAppSecret = configuration["Authentication:Facebook:AppSecret"];

            if (!string.IsNullOrWhiteSpace(facebookAppId) && 
                !string.IsNullOrWhiteSpace(facebookAppSecret))
            {
                authBuilder.AddFacebook(options =>
                {
                    options.AppId = facebookAppId;
                    options.AppSecret = facebookAppSecret;
                });
            }

            return services;
        }
    }
}
