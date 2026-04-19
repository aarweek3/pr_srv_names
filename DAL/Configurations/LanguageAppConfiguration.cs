using DAL.Models.LocalizationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    /// <summary>
    /// Конфигурация для сущности LanguageApp (языки интерфейса приложения)
    /// </summary>
    public class LanguageAppConfiguration : IEntityTypeConfiguration<LanguageApp>
    {
        public void Configure(EntityTypeBuilder<LanguageApp> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("LanguagesApp");

            // ==========================================
            // Индексы
            // ==========================================
            entity.HasIndex(e => e.Code)
                  .IsUnique()
                  .HasDatabaseName("IX_LanguagesApp_Code");

            entity.HasIndex(e => e.ShortCode)
                  .HasDatabaseName("IX_LanguagesApp_ShortCode");

            entity.HasIndex(e => new { e.IsDefault, e.Enabled })
                  .HasDatabaseName("IX_LanguagesApp_Default_Enabled");

            // УНИКАЛЬНОСТЬ: только один язык может быть IsDefault=true
            entity.HasIndex(e => e.IsDefault)
                  .IsUnique()
                  .HasDatabaseName("IX_LanguagesApp_Default_Unique")
                  .HasFilter("\"IsDefault\" = true");

            // ==========================================
            // Ограничения на свойства
            // ==========================================
            entity.Property(e => e.Code)
                  .IsRequired()
                  .HasMaxLength(10)
                  .HasComment("Код языка по стандарту BCP-47 (например, ru-RU, en-US)");

            entity.Property(e => e.ShortCode)
                  .IsRequired()
                  .HasMaxLength(5)
                  .HasComment("Краткий код языка для UI (например, RU, EN)");

            entity.Property(e => e.Title)
                  .IsRequired()
                  .HasMaxLength(50)
                  .HasComment("Название на английском для админки");

            entity.Property(e => e.NativeTitle)
                  .IsRequired()
                  .HasMaxLength(50)
                  .HasComment("Название на родном языке для UI");

            entity.Property(e => e.Direction)
                  .IsRequired()
                  .HasMaxLength(3)
                  .HasDefaultValue("ltr")
                  .HasComment("Направление письма (ltr/rtl)");

            entity.Property(e => e.Enabled)
                  .HasDefaultValue(true)
                  .HasComment("Доступность языка для выбора");

            entity.Property(e => e.IsDefault)
                  .HasDefaultValue(false)
                  .HasComment("Язык по умолчанию (может быть только один)");

            entity.Property(e => e.IsSystem)
                  .HasDefaultValue(false)
                  .HasComment("Системный язык (нельзя удалить)");

            entity.Property(e => e.SortOrder)
                  .HasDefaultValue(999)
                  .HasComment("Порядок сортировки в UI");

            entity.Property(e => e.IconKey)
                  .HasMaxLength(20)
                  .HasComment("Ключ иконки/флага");

            entity.Property(e => e.CreatedAt)
                  .HasDefaultValueSql("NOW()")
                  .HasComment("Дата создания записи");

            entity.Property(e => e.UpdatedAt)
                  .HasDefaultValueSql("NOW()")
                  .HasComment("Дата последнего обновления");
        }
    }
}
