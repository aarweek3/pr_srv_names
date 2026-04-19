using DAL.Models.SampleModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Конфигурация для сущности SampleMain
    /// </summary>
    public class SampleMainConfiguration : IEntityTypeConfiguration<SampleMain>
    {
        public void Configure(EntityTypeBuilder<SampleMain> entity)
        {
            entity.ToTable("SamplesMain");
            entity.HasKey(e => e.Id);

            // ==========================================
            // Свойства
            // ==========================================
            entity.Property(e => e.Id)
                  .HasComment("Уникальный идентификатор записи");

            entity.Property(e => e.Name)
                  .IsRequired()
                  .HasMaxLength(255)
                  .HasComment("Техническое название для идентификации в админке");

            entity.Property(e => e.SystemCode)
                  .HasMaxLength(100)
                  .HasComment("Системный код для использования в логике приложения");

            entity.Property(e => e.IsActive)
                  .HasComment("Флаг активности записи");

            entity.Property(e => e.CreatedAt)
                  .HasDefaultValueSql("NOW()")
                  .HasComment("Дата создания записи");

            entity.Property(e => e.UpdatedAt)
                  .HasDefaultValueSql("NOW()")
                  .HasComment("Дата последнего обновления");
        }
    }
}
