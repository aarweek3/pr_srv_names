using pr_srv_names.Pages.Health.Models;

namespace pr_srv_names.Pages.Health.Services
{
    public interface IHealthCheckEnhancedService
    {
        Task<DetailedHealthResponse> GetDetailedHealthAsync(string? accessToken);
        Task<ServerMetrics> GetServerMetricsAsync();
        Task<List<HealthLogEntry>> GetRecentLogsAsync(int count);
    }
}
