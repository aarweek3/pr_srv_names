using DAL;
using DAL.Models.AuthorizationModels;
using Microsoft.AspNetCore.Identity;

namespace pr_srv_names.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                ConfigurePasswordOptions(options);
                ConfigureUserOptions(options);
                ConfigureLockoutOptions(options);
                ConfigureSignInOptions(options);
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider);

            return services;
        }

        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                ConfigurePasswordOptions(options, configuration);
                ConfigureUserOptions(options, configuration);
                ConfigureLockoutOptions(options, configuration);
                ConfigureSignInOptions(options, configuration);
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider);

            return services;
        }

        private static void ConfigurePasswordOptions(IdentityOptions options, IConfiguration? configuration = null)
        {
            var passwordSection = configuration?.GetSection("Identity:Password");

            options.Password.RequiredLength = passwordSection?.GetValue<int>("RequiredLength") ?? 8;
            options.Password.RequireDigit = passwordSection?.GetValue<bool>("RequireDigit") ?? true;
            options.Password.RequireNonAlphanumeric = passwordSection?.GetValue<bool>("RequireNonAlphanumeric") ?? false;
            options.Password.RequireUppercase = passwordSection?.GetValue<bool>("RequireUppercase") ?? true;
            options.Password.RequireLowercase = passwordSection?.GetValue<bool>("RequireLowercase") ?? true;
            options.Password.RequiredUniqueChars = passwordSection?.GetValue<int>("RequiredUniqueChars") ?? 1;
        }

        private static void ConfigureUserOptions(IdentityOptions options, IConfiguration? configuration = null)
        {
            var userSection = configuration?.GetSection("Identity:User");

            options.User.RequireUniqueEmail = userSection?.GetValue<bool>("RequireUniqueEmail") ?? true;
            options.User.AllowedUserNameCharacters = userSection?.GetValue<string>("AllowedUserNameCharacters") ??
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        }

        private static void ConfigureLockoutOptions(IdentityOptions options, IConfiguration? configuration = null)
        {
            var lockoutSection = configuration?.GetSection("Identity:Lockout");

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(
                lockoutSection?.GetValue<int>("DefaultLockoutTimeSpanMinutes") ?? 15);
            options.Lockout.MaxFailedAccessAttempts = lockoutSection?.GetValue<int>("MaxFailedAccessAttempts") ?? 5;
            options.Lockout.AllowedForNewUsers = lockoutSection?.GetValue<bool>("AllowedForNewUsers") ?? true;
        }

        private static void ConfigureSignInOptions(IdentityOptions options, IConfiguration? configuration = null)
        {
            var signInSection = configuration?.GetSection("Identity:SignIn");

            options.SignIn.RequireConfirmedEmail = signInSection?.GetValue<bool>("RequireConfirmedEmail") ?? false;
            options.SignIn.RequireConfirmedPhoneNumber = signInSection?.GetValue<bool>("RequireConfirmedPhoneNumber") ?? false;
            options.SignIn.RequireConfirmedAccount = signInSection?.GetValue<bool>("RequireConfirmedAccount") ?? false;
        }

        public static IServiceCollection AddIdentityPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                // Базовые политики
                options.AddPolicy("RequireAdminRole", policy =>
                    policy.RequireRole("Admin"));

                options.AddPolicy("RequireUserRole", policy =>
                    policy.RequireRole("User", "Admin", "Moderator"));

                options.AddPolicy("RequireModeratorRole", policy =>
                    policy.RequireRole("Moderator", "Admin"));

                // Политики на основе Claims
                options.AddPolicy("ManageUsers", policy =>
                    policy.RequireAssertion(context =>
                        context.User.IsInRole("Admin") ||
                        context.User.HasClaim("Permission", "ManageUsers")));

                options.AddPolicy("ViewReports", policy =>
                    policy.RequireAssertion(context =>
                        context.User.IsInRole("Admin") ||
                        context.User.IsInRole("Moderator") ||
                        context.User.HasClaim("Permission", "ViewReports")));

                // Минимальный возраст (если используется)
                options.AddPolicy("MinimumAge18", policy =>
                    policy.RequireAssertion(context =>
                    {
                        var birthDateClaim = context.User.FindFirst("BirthDate");
                        if (birthDateClaim != null && DateTime.TryParse(birthDateClaim.Value, out var birthDate))
                        {
                            return DateTime.Now.Year - birthDate.Year >= 18;
                        }
                        return false;
                    }));

                // Активный пользователь
                options.AddPolicy("ActiveUser", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasClaim("IsActive", "true")));
            });

            return services;
        }

        public static IServiceCollection ConfigureIdentityOptions(this IServiceCollection services, Action<IdentityOptions> configureOptions)
        {
            services.Configure<IdentityOptions>(configureOptions);
            return services;
        }

        public static IServiceCollection AddCustomIdentityValidators(this IServiceCollection services)
        {
            // Кастомные валидаторы
            services.AddTransient<IUserValidator<ApplicationUser>, CustomUserValidator>();
            services.AddTransient<IPasswordValidator<ApplicationUser>, CustomPasswordValidator>();

            return services;
        }
    }

    // Кастомный валидатор пользователей
    public class CustomUserValidator : IUserValidator<ApplicationUser>
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            var errors = new List<IdentityError>();

            // Проверка имени и фамилии
            if (string.IsNullOrWhiteSpace(user.FirstName))
            {
                errors.Add(new IdentityError
                {
                    Code = "MissingFirstName",
                    Description = "Имя обязательно для заполнения"
                });
            }

            if (string.IsNullOrWhiteSpace(user.LastName))
            {
                errors.Add(new IdentityError
                {
                    Code = "MissingLastName",
                    Description = "Фамилия обязательна для заполнения"
                });
            }

            // Проверка на недопустимые домены email
            if (!string.IsNullOrEmpty(user.Email))
            {
                var forbiddenDomains = new[] { "tempmail.com", "10minutemail.com", "guerrillamail.com" };
                var domain = user.Email.Split('@').LastOrDefault()?.ToLower();

                if (domain != null && forbiddenDomains.Contains(domain))
                {
                    errors.Add(new IdentityError
                    {
                        Code = "ForbiddenEmailDomain",
                        Description = "Временные email адреса не разрешены"
                    });
                }
            }

            return errors.Any() ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
        }
    }

    public class CustomPasswordValidator : IPasswordValidator<ApplicationUser>
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user, string? password)
        {
            var errors = new List<IdentityError>();

            // Проверка на null или пустой пароль
            if (string.IsNullOrEmpty(password))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordRequired",
                    Description = "Пароль обязателен"
                });
                return IdentityResult.Failed(errors.ToArray());
            }

            // Проверка на содержание имени пользователя в пароле
            if (!string.IsNullOrEmpty(user.UserName) &&
                password.ToLower().Contains(user.UserName.ToLower()))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordContainsUserName",
                    Description = "Пароль не должен содержать имя пользователя"
                });
            }

            // Проверка на общие пароли
            var commonPasswords = new[] { "password", "123456", "qwerty", "admin", "root" };
            if (commonPasswords.Any(cp => password.ToLower().Contains(cp.ToLower())))
            {
                errors.Add(new IdentityError
                {
                    Code = "CommonPassword",
                    Description = "Пароль слишком простой"
                });
            }

            // Проверка на последовательные символы
            if (HasSequentialChars(password))
            {
                errors.Add(new IdentityError
                {
                    Code = "SequentialPassword",
                    Description = "Пароль не должен содержать последовательные символы"
                });
            }

            return errors.Any() ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
        }

        private static bool HasSequentialChars(string password)
        {
            for (int i = 0; i < password.Length - 2; i++)
            {
                if (char.IsDigit(password[i]) && char.IsDigit(password[i + 1]) && char.IsDigit(password[i + 2]))
                {
                    if (password[i + 1] == password[i] + 1 && password[i + 2] == password[i + 1] + 1)
                        return true;
                }
            }
            return false;
        }
    }
}