using DAL.Models.AuthorizationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> entity)
        {
            entity.ToTable("Users");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("Дата создания аккаунта");

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("Имя пользователя");

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("Фамилия пользователя");

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("Активен ли пользователь");

            entity.Property(e => e.Avatar)
                .HasMaxLength(255)
                .HasComment("Путь к аватару пользователя");

            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .HasComment("Отдел пользователя");

            entity.Property(e => e.EmailConfirmedAt)
                .HasComment("Дата подтверждения email");

            entity.Property(e => e.TwoFactorEnabled)
                .HasDefaultValue(false)
                .HasComment("Включена ли двухфакторная аутентификация");

            entity.Property(e => e.UpdatedAt)
                .HasComment("Дата последнего обновления");

            entity.Property(e => e.LastLogin)
                .HasComment("Дата последнего входа");

            entity.Property(e => e.ExternalProvider)
                .HasMaxLength(50)
                .HasComment("Внешний провайдер OAuth");

            entity.Property(e => e.ExternalId)
                .HasMaxLength(100)
                .HasComment("ID пользователя у внешнего провайдера");

            entity.Property(e => e.IsExternalAccount)
                .HasDefaultValue(false)
                .HasComment("Создан ли аккаунт через внешнего провайдера");

            // Индексы
            entity.HasIndex(e => e.Email).IsUnique().HasDatabaseName("IX_Users_Email");
            entity.HasIndex(e => e.IsActive).HasDatabaseName("IX_Users_IsActive");
            entity.HasIndex(e => e.CreatedAt).HasDatabaseName("IX_Users_CreatedAt");
            entity.HasIndex(e => new { e.IsActive, e.CreatedAt }).HasDatabaseName("IX_Users_IsActive_CreatedAt");
            entity.HasIndex(e => e.TwoFactorEnabled).HasDatabaseName("IX_Users_TwoFactorEnabled");
            entity.HasIndex(e => e.Department).HasDatabaseName("IX_Users_Department");
            entity.HasIndex(e => e.LastLogin).HasDatabaseName("IX_Users_LastLogin");
            entity.HasIndex(e => new { e.ExternalProvider, e.ExternalId })
                .IsUnique()
                .HasFilter("\"ExternalProvider\" IS NOT NULL AND \"ExternalId\" IS NOT NULL")
                .HasDatabaseName("IX_Users_ExternalProvider_ExternalId");
            entity.HasIndex(e => e.ExternalProvider).HasDatabaseName("IX_Users_ExternalProvider");
            entity.HasIndex(e => e.IsExternalAccount).HasDatabaseName("IX_Users_IsExternalAccount");

            // Связи
            entity.HasMany(u => u.UserSessions)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(u => u.ActivityLogs)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}