using DAL.Models.SampleModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Конфигурация для сущности SampleMainDescriptionSeo (таблица переводов для SampleMainSeo с поддержкой SEO)
    /// </summary>
    public class SampleMainDescriptionSeoConfiguration : IEntityTypeConfiguration<SampleMainDescriptionSeo>
    {
        public void Configure(EntityTypeBuilder<SampleMainDescriptionSeo> entity)
        {
            entity.ToTable("SamplesMainDescriptionsSeo");
            entity.HasKey(e => e.Id);

            // ==========================================
            // Свойства
            // ==========================================
            entity.Property(e => e.Id)
                  .HasComment("Уникальный идентификатор перевода");

            entity.Property(e => e.Name)
                  .HasMaxLength(255)
                  .HasComment("Локализованное название");

            entity.Property(e => e.Description)
                  .HasComment("Локализованное описание");

            entity.Property(e => e.HtmlContent)
                  .HasComment("HTML контент (статья/полное описание)");

            entity.Property(e => e.UrlPicture)
                  .HasMaxLength(500)
                  .HasComment("Локализованное изображение (если пусто, берется Main)");

            entity.Property(e => e.CreatedAt)
                  .HasDefaultValueSql("NOW()")
                  .HasComment("Дата создания записи");

            entity.Property(e => e.UpdatedAt)
                  .HasDefaultValueSql("NOW()")
                  .HasComment("Дата последнего обновления");

            // ==========================================
            // Связи
            // ==========================================
            
            // Связь с SampleMainSeo
            entity.HasOne(d => d.SampleMainSeo)
                  .WithMany(m => m.Descriptions)
                  .HasForeignKey(d => d.SampleMainSeoId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .HasConstraintName("FK_SamplesMainDescriptionsSeo_SamplesMainSeo");

            // Связь с LanguageApp
            entity.HasOne(d => d.LanguageApp)
                  .WithMany()
                  .HasForeignKey(d => d.LanguageAppId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .HasConstraintName("FK_SamplesMainDescriptionsSeo_LanguagesApp");

            // Связь 1:1 с SeoData (Композиция)
            entity.HasOne(d => d.SeoData)
                  .WithOne()
                  .HasForeignKey<SampleMainDescriptionSeo>(d => d.SeoDataId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .HasConstraintName("FK_SamplesMainDescriptionsSeo_SeoData");

            // ==========================================
            // Индексы
            // ==========================================
            
            // УНИКАЛЬНОСТЬ: один перевод на один язык для одной записи
            entity.HasIndex(e => new { e.SampleMainSeoId, e.LanguageAppId })
                  .IsUnique()
                  .HasDatabaseName("IX_SamplesMainDescriptionsSeo_Main_Language");
        }
    }
}
