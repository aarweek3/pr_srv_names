using DAL.Models.SampleModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Конфигурация для сущности SampleMainSeo
    /// </summary>
    public class SampleMainSeoConfiguration : IEntityTypeConfiguration<SampleMainSeo>
    {
        public void Configure(EntityTypeBuilder<SampleMainSeo> entity)
        {
            entity.ToTable("SamplesMainSeo");
            entity.HasKey(e => e.Id);

            // ==========================================
            // Свойства
            // ==========================================
            entity.Property(e => e.Id)
                  .HasComment("Уникальный идентификатор записи");

            entity.Property(e => e.Name)
                  .IsRequired()
                  .HasMaxLength(255)
                  .HasComment("Техническое название для идентификации");

            entity.Property(e => e.SystemCode)
                  .HasMaxLength(100)
                  .HasComment("Системный код");

            entity.Property(e => e.UrlPictureMain)
                  .HasMaxLength(500)
                  .HasComment("Главное изображение (по умолчанию для всех языков)");

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
