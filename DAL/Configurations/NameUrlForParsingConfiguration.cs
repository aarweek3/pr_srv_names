using DAL.Models.NameModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class NameUrlForParsingConfiguration : IEntityTypeConfiguration<NameUrlForParsing>
    {
        public void Configure(EntityTypeBuilder<NameUrlForParsing> entity)
        {
            entity.HasIndex(e => new { e.NameMainId, e.LanguageId }).IsUnique().HasDatabaseName("IX_NameUrlForParsings_NameMainId_LanguageId");
            entity.Property(e => e.Status).HasConversion<string>().HasComment("Статус парсинга URL: NotReady/Processing/Ready/Failed");
            entity.Property(e => e.LastParsedAt).HasComment("Дата последнего успешного парсинга");
            entity.Property(e => e.ParseAttempts).HasDefaultValue(0).HasComment("Количество попыток парсинга");
            entity.Property(e => e.ErrorMessage).HasComment("Сообщение об ошибке парсинга");
            entity.Property(e => e.Url).HasComment("URL для парсинга данных об имени");

            entity.HasIndex(e => new { e.Status, e.ParseAttempts }).HasDatabaseName("IX_NameUrlForParsings_Status_ParseAttempts");
            entity.HasIndex(e => e.LastParsedAt).HasDatabaseName("IX_NameUrlForParsings_LastParsedAt");
            entity.HasIndex(e => e.Status).HasDatabaseName("IX_NameUrlForParsings_Status");
        }
    }
}