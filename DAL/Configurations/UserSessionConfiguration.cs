using DAL.Models.AuthorizationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
    {
        public void Configure(EntityTypeBuilder<UserSession> entity)
        {
            entity.ToTable("UserSessions");

            entity.Property(e => e.Id).HasComment("Уникальный идентификатор сессии");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasComment("Дата создания сессии");
            entity.Property(e => e.RefreshToken).IsRequired().HasMaxLength(500).HasComment("Refresh токен");
            entity.Property(e => e.ExpiresAt).IsRequired().HasComment("Дата окончания действия токена");
            entity.Property(e => e.IsRevoked).HasDefaultValue(false).HasComment("Отозван ли токен");
            entity.Property(e => e.RevokedAt).HasComment("Время аннулирования сессии");
            entity.Property(e => e.DeviceInfo).HasMaxLength(500).HasComment("Информация об устройстве (браузер, ОС)");
            entity.Property(e => e.IpAddress).HasMaxLength(45).HasComment("IP адрес создания сессии");
            entity.Property(e => e.UserAgent).HasMaxLength(500).HasComment("User Agent браузера");

            entity.HasIndex(e => e.UserId).HasDatabaseName("IX_UserSessions_UserId");
            entity.HasIndex(e => new { e.RefreshToken, e.IsRevoked }).IsUnique().HasDatabaseName("IX_UserSessions_RefreshToken_IsRevoked");
            entity.HasIndex(e => e.ExpiresAt).HasDatabaseName("IX_UserSessions_ExpiresAt");
            entity.HasIndex(e => e.IsRevoked).HasDatabaseName("IX_UserSessions_IsRevoked");
            entity.HasIndex(e => new { e.UserId, e.IsRevoked, e.ExpiresAt }).HasDatabaseName("IX_UserSessions_UserId_IsRevoked_ExpiresAt");
        }
    }
}