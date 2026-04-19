using DAL.Models.GeneralModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Конфигурация для сущности MediaFile
    /// </summary>
    public class MediaFileConfiguration : IEntityTypeConfiguration<MediaFile>
    {
        public void Configure(EntityTypeBuilder<MediaFile> entity)
        {
            entity.ToTable("MediaFiles");
            entity.HasKey(e => e.Id);

            // ==========================================
            // Индексы
            // ==========================================
            entity.HasIndex(e => e.ImageId)
                  .IsUnique()
                  .HasDatabaseName("IX_MediaFiles_ImageId");

            entity.HasIndex(e => e.OriginalName)
                  .HasDatabaseName("IX_MediaFiles_OriginalName");

            entity.HasIndex(e => e.CreatedAt)
                  .HasDatabaseName("IX_MediaFiles_CreatedAt");

            // Комментарии к свойствам (если необходимо)
            entity.Property(e => e.Id)
                  .HasComment("Уникальный идентификатор медиафайла");

            entity.Property(e => e.ImageId)
                  .HasComment("Уникальный идентификатор изображения");

            entity.Property(e => e.OriginalName)
                  .HasComment("Исходное имя файла");

            entity.Property(e => e.CreatedAt)
                  .HasComment("Дата создания записи");
        }
    }
}
