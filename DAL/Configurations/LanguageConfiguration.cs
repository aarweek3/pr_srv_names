using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> entity)
        {
            entity.ToTable("Languages");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasComment("Уникальный идентификатор языка");
            entity.Property(e => e.Code).IsRequired().HasMaxLength(2).HasComment("Код языка по стандарту ISO 639-1");
            entity.Property(e => e.Name).IsRequired().HasMaxLength(128).HasComment("Название языка на родном языке");
            entity.Property(e => e.FlagCode).HasMaxLength(2).HasComment("Код флага для фронтенд-отображения");
            entity.Property(e => e.Description).HasMaxLength(1024).HasComment("Описание языка");
            entity.Property(e => e.DisplayOrder).HasDefaultValue(999).HasComment("Порядок отображения языка");
            entity.Property(e => e.IsDefault).HasDefaultValue(false).HasComment("Является ли язык языком по умолчанию");
            entity.Property(e => e.TextDirection).HasMaxLength(3).HasDefaultValue("ltr")
                .HasComment("Направление текста (ltr/rtl)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("Дата создания записи");
            entity.Property(e => e.UpdatedAt).HasComment("Дата последнего обновления");
            entity.Property(e => e.IsActive).HasDefaultValue(true).HasComment("Активен ли язык");

            entity.HasIndex(e => e.Code).IsUnique().HasDatabaseName("IX_Languages_Code");
            entity.HasIndex(e => e.IsActive).HasDatabaseName("IX_Languages_IsActive");
            entity.HasIndex(e => e.DisplayOrder).HasDatabaseName("IX_Languages_DisplayOrder");

            entity.HasMany(l => l.NameDetails).WithOne(d => d.Language).HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed данных
            var now = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            entity.HasData(
                new Language
                {
                    Id = 1, Code = "en", Name = "English", FlagCode = "gb", DisplayOrder = 1, IsDefault = true,
                    IsActive = true, CreatedAt = now
                },
                new Language
                {
                    Id = 2, Code = "ru", Name = "Русский", FlagCode = "ru", DisplayOrder = 2, IsActive = true,
                    CreatedAt = now
                },
                new Language
                {
                    Id = 3, Code = "es", Name = "Español", FlagCode = "es", DisplayOrder = 3, IsActive = true,
                    CreatedAt = now
                },
                new Language
                {
                    Id = 4, Code = "fr", Name = "Français", FlagCode = "fr", DisplayOrder = 4, IsActive = true,
                    CreatedAt = now
                },
                new Language
                {
                    Id = 5, Code = "de", Name = "Deutsch", FlagCode = "de", DisplayOrder = 5, IsActive = true,
                    CreatedAt = now
                },
                new Language
                {
                    Id = 6, Code = "it", Name = "Italiano", FlagCode = "it", DisplayOrder = 6, IsActive = true,
                    CreatedAt = now
                },
                new Language
                {
                    Id = 7, Code = "pt", Name = "Português", FlagCode = "pt", DisplayOrder = 7, IsActive = true,
                    CreatedAt = now
                }
            );
        }
    }
}