using DAL.Models.AuthorizationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class ActivityLogConfiguration : IEntityTypeConfiguration<ActivityLog>
    {
        public void Configure(EntityTypeBuilder<ActivityLog> entity)
        {
            entity.ToTable("ActivityLogs");

            entity.Property(e => e.Id).HasComment("Уникальный идентификатор записи лога");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasComment("Дата создания записи");
            entity.Property(e => e.Timestamp).HasDefaultValueSql("CURRENT_TIMESTAMP").HasComment("Время выполнения действия");
            entity.Property(e => e.Action).IsRequired().HasConversion<string>().HasMaxLength(50).HasComment("Тип действия");
            entity.Property(e => e.Success).HasDefaultValue(true).HasComment("Успешно ли выполнено действие");
            entity.Property(e => e.EntityType).HasMaxLength(100).HasComment("Тип сущности над которой выполнено действие");
            entity.Property(e => e.EntityId).HasMaxLength(50).HasComment("ID сущности над которой выполнено действие");
            entity.Property(e => e.Details).HasMaxLength(500).HasComment("Дополнительная информация о действии");
            entity.Property(e => e.DeviceType).HasConversion<int>().HasComment("Тип устройства, с которого выполнено действие");
            entity.Property(e => e.IpAddress).HasMaxLength(45).HasComment("IP адрес откуда выполнено действие");
            entity.Property(e => e.UserAgent).HasMaxLength(500).HasComment("User Agent браузера");

            entity.HasIndex(e => e.UserId).HasDatabaseName("IX_ActivityLogs_UserId");
            entity.HasIndex(e => e.Timestamp).HasDatabaseName("IX_ActivityLogs_Timestamp");
            entity.HasIndex(e => e.Action).HasDatabaseName("IX_ActivityLogs_Action");
            entity.HasIndex(e => e.Success).HasDatabaseName("IX_ActivityLogs_Success");
            entity.HasIndex(e => new { e.UserId, e.Timestamp }).HasDatabaseName("IX_ActivityLogs_UserId_Timestamp");
            entity.HasIndex(e => new { e.Action, e.Timestamp }).HasDatabaseName("IX_ActivityLogs_Action_Timestamp");
            entity.HasIndex(e => new { e.Success, e.Timestamp }).HasDatabaseName("IX_ActivityLogs_Success_Timestamp");
            entity.HasIndex(e => e.EntityType).HasDatabaseName("IX_ActivityLogs_EntityType");
            entity.HasIndex(e => new { e.EntityType, e.EntityId }).HasDatabaseName("IX_ActivityLogs_EntityType_EntityId");
            entity.HasIndex(e => e.DeviceType).HasDatabaseName("IX_ActivityLogs_DeviceType");
        }
    }
}