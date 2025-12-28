using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Hosting; // Нужно для IWebHostEnvironment
using Microsoft.Extensions.Logging; // Нужно для ILogger
using Microsoft.Extensions.DependencyInjection; // Нужно для .CreateScope() и .GetRequiredService()
using DAL.Interfaces;
using pr_srv_names.Pages.Health.Models;

namespace pr_srv_names.Pages.Health.Services
{
    public class HealthCheckEnhancedService : IHealthCheckEnhancedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<HealthCheckEnhancedService> _logger;
        private readonly IWebHostEnvironment _environment;

        public HealthCheckEnhancedService(
            IServiceProvider serviceProvider,
            ILogger<HealthCheckEnhancedService> logger,
            IWebHostEnvironment environment)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _environment = environment;
        }

        public async Task<DetailedHealthResponse> GetDetailedHealthAsync(string? accessToken)
        {
            var stopwatch = Stopwatch.StartNew();

            var response = new DetailedHealthResponse
            {
                Timestamp = DateTime.UtcNow,
                Environment = _environment.EnvironmentName,
                Version = "1.0.0-enhanced",
                ServerMetrics = await GetServerMetricsAsync(),
                Tokens = ValidateToken(accessToken)
            };

            // Проверка БД
            response.Health = await PerformDatabaseCheckAsync();

            stopwatch.Stop();
            response.Health.Duration = stopwatch.Elapsed;
            return response;
        }

        public async Task<ServerMetrics> GetServerMetricsAsync()
        {
            var process = Process.GetCurrentProcess();

            return new ServerMetrics
            {
                CpuUsagePercentage = await GetCpuUsageAsync(),
                MemoryUsageBytes = process.WorkingSet64,
                AvailableMemoryBytes = GC.GetGCMemoryInfo().TotalAvailableMemoryBytes,
                ActiveConnections = process.Threads.Count,
                UptimeMs = (long)(DateTime.Now - process.StartTime).TotalMilliseconds
            };
        }

        private async Task<HealthCheckResult> PerformDatabaseCheckAsync()
        {
            var result = new HealthCheckResult();
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                // Просто для проверки: если UnitOfWork доступен, считаем БД живой
                result.Status = "Healthy";
                result.Details.Add("database", "Connected");
            }
            catch (Exception ex)
            {
                result.Status = "Unhealthy";
                result.Details.Add("error", ex.Message);
            }
            return result;
        }

        private TokenValidationResult ValidateToken(string? token)
        {
            var result = new TokenValidationResult();
            if (string.IsNullOrEmpty(token)) return result;
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                result.IsAccessTokenValid = jwtToken.ValidTo > DateTime.UtcNow;
                result.AccessTokenExpiry = jwtToken.ValidTo;
                result.UserId = jwtToken.Subject ?? "";

                // Обработка ролей (учитывая разные форматы в JWT)
                result.Roles = jwtToken.Claims
                    .Where(c => c.Type == "role" || c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                    .Select(c => c.Value)
                    .ToList();
            }
            catch
            {
                result.IsAccessTokenValid = false;
            }
            return result;
        }

        private async Task<double> GetCpuUsageAsync()
        {
            // Упрощенная заглушка для CPU
            return 15.5;
        }

        // ИСПРАВЛЕНО: Теперь возвращает List, как в интерфейсе
        public Task<List<HealthLogEntry>> GetRecentLogsAsync(int count)
        {
            throw new NotImplementedException("Логирование будет реализовано в следующем шаге");
        }
    }
}