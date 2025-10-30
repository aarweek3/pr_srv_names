using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class SeoDataConfiguration : IEntityTypeConfiguration<SeoData>
    {
        public void Configure(EntityTypeBuilder<SeoData> entity)
        {
            entity.ToTable("SeoData");
            entity.HasKey(e => e.Id);

            // Свойства
            entity.Property(e => e.Id)
                .HasComment("Уникальный идентификатор SEO-данных");
            entity.Property(e => e.NameMainId)
                .IsRequired()
                .HasComment("Внешний ключ на таблицу Names");
            entity.Property(e => e.LanguageId)
                .IsRequired()
                .HasComment("Внешний ключ на таблицу Languages");
            entity.Property(e => e.MetaTitle)
                .HasMaxLength(70)
                .HasComment("SEO заголовок (рекомендуется до 70 символов)");
            entity.Property(e => e.MetaDescription)
                .HasMaxLength(160)
                .HasComment("META описание (рекомендуется до 160 символов)");
            entity.Property(e => e.MetaKeywords)
                .HasMaxLength(200)
                .HasComment("META ключевые слова");
            entity.Property(e => e.UrlSlug)
                .HasMaxLength(200)
                .HasComment("ЧПУ URL");
            entity.Property(e => e.CanonicalUrl)
                .HasMaxLength(300)
                .HasComment("Каноническая ссылка");
            entity.Property(e => e.OgTitle)
                .HasMaxLength(100)
                .HasComment("Open Graph заголовок");
            entity.Property(e => e.OgDescription)
                .HasMaxLength(200)
                .HasComment("Open Graph описание");
            entity.Property(e => e.OgImage)
                .HasMaxLength(500)
                .HasComment("Open Graph изображение");
            entity.Property(e => e.OgType)
                .HasMaxLength(50)
                .HasDefaultValue("article")
                .HasComment("Open Graph тип контента");
            entity.Property(e => e.OgUrl)
                .HasMaxLength(300)
                .HasComment("Open Graph URL");
            entity.Property(e => e.TwitterCard)
                .HasMaxLength(100)
                .HasDefaultValue("summary_large_image")
                .HasComment("Twitter Card тип");
            entity.Property(e => e.TwitterTitle)
                .HasMaxLength(100)
                .HasComment("Twitter заголовок");
            entity.Property(e => e.TwitterDescription)
                .HasMaxLength(200)
                .HasComment("Twitter описание");
            entity.Property(e => e.TwitterImage)
                .HasMaxLength(500)
                .HasComment("Twitter изображение");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(500)
                .HasDefaultValue(string.Empty)
                .HasComment("URL изображения");
            entity.Property(e => e.ImageAltText)
                .HasMaxLength(125)
                .HasComment("Alt текст изображения");
            entity.Property(e => e.ImageCaption)
                .HasMaxLength(200)
                .HasComment("Подпись изображения");
            entity.Property(e => e.SchemaType)
                .HasMaxLength(50)
                .HasComment("Тип схемы Schema.org");
            entity.Property(e => e.SchemaJsonLd)
                .HasColumnType("jsonb")
                .HasComment("JSON-LD разметка");
            entity.Property(e => e.AuthorName)
                .HasMaxLength(100)
                .HasDefaultValue(string.Empty)
                .HasComment("Имя автора");
            entity.Property(e => e.PublisherName)
                .HasMaxLength(100)
                .HasDefaultValue(string.Empty)
                .HasComment("Имя издателя");
            entity.Property(e => e.PublishedDate)
                .HasComment("Дата публикации");
            entity.Property(e => e.ModifiedDate)
                .HasComment("Дата последнего изменения");
            entity.Property(e => e.ArticleSection)
                .HasMaxLength(100)
                .HasDefaultValue(string.Empty)
                .HasComment("Раздел статьи");
            entity.Property(e => e.NoIndex)
                .HasDefaultValue(false)
                .HasComment("Запрет индексации (robots noindex)");
            entity.Property(e => e.NoFollow)
                .HasDefaultValue(false)
                .HasComment("Запрет следования по ссылкам (robots nofollow)");
            entity.Property(e => e.Priority)
                .HasDefaultValue(5)
                .HasComment("Приоритет страницы для sitemap.xml (0-10)");
            entity.Property(e => e.Region)
                .HasMaxLength(100)
                .HasComment("Географический регион");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("Дата создания записи");
            entity.Property(e => e.UpdatedAt)
                .HasComment("Дата последнего обновления");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(450)
                .HasComment("Пользователь, создавший запись");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(450)
                .HasComment("Пользователь, обновивший запись");

            // Индексы
            entity.HasIndex(e => new { e.NameMainId, e.LanguageId })
                .IsUnique()
                .HasDatabaseName("IX_SeoData_NameMainId_LanguageId");
            entity.HasIndex(e => e.UrlSlug)
                .HasDatabaseName("IX_SeoData_UrlSlug");
            entity.HasIndex(e => e.NoIndex)
                .HasDatabaseName("IX_SeoData_NoIndex");
            entity.HasIndex(e => e.Priority)
                .HasDatabaseName("IX_SeoData_Priority");

            // Связи // Связь 1:1 - главная конфигурация здесь
            entity.HasOne(s => s.NameMain)
                .WithOne(n => n.SeoData) // Исправлено: 1:1 связь с NameMain
                .HasForeignKey<SeoData>(s => s.NameMainId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(s => s.Language)
                .WithMany() // Без навигационного свойства на стороне Language
                .HasForeignKey(s => s.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}