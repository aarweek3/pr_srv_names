using Serilog;

namespace pr_srv_names.Extensions
{
    public static class LoggingExtensions
    {
        public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
        {
            try
            {
                // Получаем текущую директорию и создаем папку логов
                var currentDirectory = Directory.GetCurrentDirectory();
                var logsDirectory = Path.Combine(currentDirectory, "logs");

                // Создаем папку logs, если её нет
                if (!Directory.Exists(logsDirectory))
                {
                    Directory.CreateDirectory(logsDirectory);
                    Console.WriteLine($"Создана папка логов: {logsDirectory}");
                }

                var logFilePath = Path.Combine(logsDirectory, "security-headers-.log");

                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
                    .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
                    .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}")
                    .WriteTo.File(
                        path: logFilePath,
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 7,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}",
                        shared: true,
                        flushToDiskInterval: TimeSpan.FromSeconds(1)
                    )
                    .Enrich.FromLogContext()
                    .CreateLogger();

                Console.WriteLine("Serilog успешно инициализирован");

                // Тестовая запись
                Log.Information("=== СИСТЕМА ЛОГИРОВАНИЯ ЗАПУЩЕНА ===");
                Log.Information("Время запуска: {Time}", DateTime.Now);
                Log.Information("Директория: {Dir}", currentDirectory);
                Log.Information("Логи: {LogDir}", logsDirectory);

                builder.Host.UseSerilog();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ОШИБКА инициализации логирования: {ex.Message}");

                // Fallback - только консоль
                Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .CreateLogger();

                builder.Host.UseSerilog();
            }

            return builder;
        }

        public static WebApplication ConfigureDebugEndpoints(this WebApplication app)
        {
            // Debug endpoint для логов
            app.MapGet("/api/debug/logs", (ILogger<Program> logger) =>
            {
                try
                {
                    var currentDir = Directory.GetCurrentDirectory();
                    var logsDir = Path.Combine(currentDir, "logs");

                    logger.LogInformation("=== ПРОВЕРКА СИСТЕМЫ ЛОГИРОВАНИЯ ===");
                    logger.LogInformation("Директория приложения: {Dir}", currentDir);
                    logger.LogInformation("Директория логов: {LogDir}", logsDir);

                    var result = new
                    {
                        currentDirectory = currentDir,
                        logsDirectory = logsDir,
                        logsDirectoryExists = Directory.Exists(logsDir),
                        logFiles = Directory.Exists(logsDir)
                            ? Directory.GetFiles(logsDir, "*.log").Select(f => new
                            {
                                name = Path.GetFileName(f),
                                size = new FileInfo(f).Length,
                                lastModified = new FileInfo(f).LastWriteTime
                            }).ToArray()
                            : new object[0],
                        timestamp = DateTime.UtcNow
                    };

                    logger.LogInformation("Результат проверки логов: {@Result}", result);
                    return Results.Ok(result);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Ошибка в debug/logs endpoint");
                    return Results.Problem("Ошибка проверки логов: " + ex.Message);
                }
            })
            .WithTags("Debug")
            .AllowAnonymous();

            // Debug endpoint для системы аутентификации (избегаем конфликта с контроллером)
            app.MapGet("/api/debug/auth-system", (HttpContext context, ILogger<Program> logger) =>
            {
                logger.LogInformation("=== DEBUG AUTH-SYSTEM ENDPOINT ВЫЗВАН ===");
                logger.LogInformation("Время: {Time}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                logger.LogInformation("URL: {Url}", $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}");

                var securityHeaders = new[] {
                    "X-Content-Type-Options",
                    "X-Frame-Options",
                    "X-XSS-Protection",
                    "Strict-Transport-Security",
                    "Content-Security-Policy"
                };

                var presentHeaders = new List<string>();
                var missingHeaders = new List<string>();

                foreach (var header in securityHeaders)
                {
                    if (context.Response.Headers.ContainsKey(header))
                    {
                        presentHeaders.Add(header);
                        logger.LogInformation("НАЙДЕН: {Header} = {Value}", header, context.Response.Headers[header]);
                    }
                    else
                    {
                        missingHeaders.Add(header);
                        logger.LogWarning("ОТСУТСТВУЕТ: {Header}", header);
                    }
                }

                var result = new
                {
                    message = "Debug auth system endpoint активен",
                    timestamp = DateTime.UtcNow,
                    url = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}",
                    isHttps = context.Request.IsHttps,
                    securityHeadersCheck = new
                    {
                        total = securityHeaders.Length,
                        present = presentHeaders.Count,
                        missing = missingHeaders.Count,
                        presentHeaders,
                        missingHeaders
                    }
                };

                logger.LogInformation("=== ВОЗВРАЩАЕМ ОТВЕТ ===");
                return Results.Ok(result);
            })
            .WithTags("Debug")
            .AllowAnonymous();

            return app;
        }
    }
}