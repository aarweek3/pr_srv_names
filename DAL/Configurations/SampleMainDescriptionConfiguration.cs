using DAL.Models.SampleModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Конфигурация для сущности SampleMainDescription (таблица переводов для SampleMain)
    /// </summary>
    public class SampleMainDescriptionConfiguration : IEntityTypeConfiguration<SampleMainDescription>
    {
        public void Configure(EntityTypeBuilder<SampleMainDescription> entity)
        {
            entity.ToTable("SamplesMainDescriptions");
            entity.HasKey(e => e.Id);

            // ==========================================
            // Свойства
            // ==========================================
            entity.Property(e => e.Id)
                  .HasComment("Уникальный идентификатор перевода");

            entity.Property(e => e.Name)
                  .IsRequired()
                  .HasMaxLength(255)
                  .HasComment("Локализованное название");

            entity.Property(e => e.Description)
                  .HasComment("Локализованное описание");

            entity.Property(e => e.CreatedAt)
                  .HasDefaultValueSql("NOW()")
                  .HasComment("Дата создания записи");

            entity.Property(e => e.UpdatedAt)
                  .HasDefaultValueSql("NOW()")
                  .HasComment("Дата последнего обновления");

            // ==========================================
            // Связи
            // ==========================================
            
            // Связь с SampleMain
            entity.HasOne(d => d.SampleMain)
                  .WithMany(m => m.Descriptions)
                  .HasForeignKey(d => d.SampleMainId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .HasConstraintName("FK_SamplesMainDescriptions_SamplesMain");

            // Связь с LanguageApp
            entity.HasOne(d => d.LanguageApp)
                  .WithMany()
                  .HasForeignKey(d => d.LanguageAppId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .HasConstraintName("FK_SamplesMainDescriptions_LanguagesApp");

            // ==========================================
            // Индексы
            // ==========================================
            
            // УНИКАЛЬНОСТЬ: один перевод на один язык для одной записи
            entity.HasIndex(e => new { e.SampleMainId, e.LanguageAppId })
                  .IsUnique()
                  .HasDatabaseName("IX_SamplesMainDescriptions_Main_Language");
        }
    }
}
