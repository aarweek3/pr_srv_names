using DAL.Models.AuthorizationModels;
using Microsoft.AspNetCore.Identity;

namespace pr_srv_names.Extensions
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            try
            {
                logger.LogInformation("=== НАЧИНАЕМ ИНИЦИАЛИЗАЦИЮ БАЗЫ ДАННЫХ ===");

                await SeedRolesAsync(roleManager, logger);
                await SeedAdminUserAsync(userManager, logger);

                logger.LogInformation("=== ИНИЦИАЛИЗАЦИЯ БАЗЫ ДАННЫХ ЗАВЕРШЕНА ===");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "КРИТИЧЕСКАЯ ОШИБКА при инициализации базы данных");
                throw; // Пробрасываем исключение, чтобы приложение не запустилось с неправильными данными
            }
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager, ILogger logger)
        {
            logger.LogInformation("--- Начинаем создание ролей ---");

            var roles = new[] { "Admin", "User", "Moderator" };

            foreach (var role in roles)
            {
                logger.LogInformation("Проверяем роль: {Role}", role);

                if (!await roleManager.RoleExistsAsync(role))
                {
                    logger.LogInformation("Создаем роль: {Role}", role);
                    var result = await roleManager.CreateAsync(new IdentityRole(role));

                    if (result.Succeeded)
                    {
                        logger.LogInformation("✅ Роль {Role} успешно создана", role);
                    }
                    else
                    {
                        logger.LogError("❌ Ошибка создания роли {Role}: {Errors}",
                            role, string.Join(", ", result.Errors.Select(e => e.Description)));
                        throw new Exception($"Не удалось создать роль {role}");
                    }
                }
                else
                {
                    logger.LogInformation("✅ Роль {Role} уже существует", role);
                }
            }

            logger.LogInformation("--- Создание ролей завершено ---");
        }

        private static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager, ILogger logger)
        {
            logger.LogInformation("--- Начинаем создание администратора ---");

            const string adminEmail = "admin@example.com";
            const string adminPassword = "Admin123!";

            logger.LogInformation("Ищем пользователя с email: {Email}", adminEmail);
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                logger.LogInformation("Администратор не найден, создаем нового пользователя");

                adminUser = new ApplicationUser
                {
                    FirstName = "Admin",
                    LastName = "User",
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    IsActive = true
                };

                logger.LogInformation("Создаем пользователя с данными: Email={Email}, UserName={UserName}",
                    adminUser.Email, adminUser.UserName);

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    logger.LogInformation("✅ Пользователь успешно создан, добавляем роль Admin");

                    var roleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
                    if (roleResult.Succeeded)
                    {
                        logger.LogInformation("✅ Администратор успешно создан: {Email}", adminEmail);

                        // Проверяем, что пользователь действительно в роли
                        var isInRole = await userManager.IsInRoleAsync(adminUser, "Admin");
                        logger.LogInformation("Проверка роли Admin: {IsInRole}", isInRole);

                        // Проверяем все роли пользователя
                        var userRoles = await userManager.GetRolesAsync(adminUser);
                        logger.LogInformation("Роли пользователя: [{Roles}]", string.Join(", ", userRoles));
                    }
                    else
                    {
                        logger.LogError("❌ Ошибка назначения роли Admin: {Errors}",
                            string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                        throw new Exception("Не удалось назначить роль Admin администратору");
                    }
                }
                else
                {
                    logger.LogError("❌ Ошибка создания администратора: {Errors}",
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                    throw new Exception("Не удалось создать администратора");
                }
            }
            else
            {
                logger.LogInformation("Администратор {Email} уже существует", adminEmail);

                // Проверяем активность пользователя
                logger.LogInformation("Проверяем статус пользователя: IsActive={IsActive}, EmailConfirmed={EmailConfirmed}",
                    adminUser.IsActive, adminUser.EmailConfirmed);

                // Проверяем роли
                var userRoles = await userManager.GetRolesAsync(adminUser);
                logger.LogInformation("Текущие роли пользователя: [{Roles}]", string.Join(", ", userRoles));

                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    logger.LogInformation("Добавляем роль Admin существующему пользователю");
                    var roleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
                    if (roleResult.Succeeded)
                    {
                        logger.LogInformation("✅ Роль Admin добавлена существующему пользователю");
                    }
                    else
                    {
                        logger.LogError("❌ Ошибка добавления роли Admin: {Errors}",
                            string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                    }
                }

                // Убеждаемся, что пользователь активен
                if (!adminUser.IsActive)
                {
                    logger.LogInformation("Активируем пользователя");
                    adminUser.IsActive = true;
                    await userManager.UpdateAsync(adminUser);
                }
            }

            logger.LogInformation("--- Создание администратора завершено ---");
        }
    }
}