namespace pr_srv_names.Pages.Health.Models
{
    public class HealthCheckResult
    {
        public string Status { get; set; } = "Unknown";
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public TimeSpan Duration { get; set; }
        public Dictionary<string, object> Details { get; set; } = new();
    }
    public class TokenValidationResult
    {
        public bool IsAccessTokenValid { get; set; }
        public bool IsRefreshTokenValid { get; set; }
        public DateTime? AccessTokenExpiry { get; set; }
        public string UserId { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
    }
    public class ServerMetrics
    {
        public double CpuUsagePercentage { get; set; }
        public long MemoryUsageBytes { get; set; }
        public long AvailableMemoryBytes { get; set; }
        public int ActiveConnections { get; set; }
        public long UptimeMs { get; set; }
    }
    public class HealthLogEntry
    {
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Endpoint { get; set; } = string.Empty;
        public string Status { get; set; } = "Unknown";
        public double DurationMs { get; set; }
        public string? ClientIp { get; set; }
        public string? UserId { get; set; }
        public bool HasAccessToken { get; set; }
        public bool HasRefreshToken { get; set; }
        public string? TokenFingerprint { get; set; }
        public Dictionary<string, object> AdditionalData { get; set; } = new();
    }
    public class DetailedHealthResponse
    {
        public HealthCheckResult Health { get; set; } = new();
        public TokenValidationResult Tokens { get; set; } = new();
        public ServerMetrics ServerMetrics { get; set; } = new();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Environment { get; set; } = string.Empty;
        public string Version { get; set; } = "1.0.0";
    }
}
