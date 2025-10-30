using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class DeclensionConfiguration : IEntityTypeConfiguration<Declension>
    {
        public void Configure(EntityTypeBuilder<Declension> entity)
        {
            entity.HasIndex(e => new { e.NameMainId, e.LanguageId }).HasDatabaseName("IX_Declensions_NameMainId_LanguageId");
            entity.Property(e => e.Nominative).HasComment("Именительный падеж");
            entity.Property(e => e.Genitive).HasComment("Родительный падеж");
            entity.Property(e => e.Dative).HasComment("Дательный падеж");
            entity.Property(e => e.Accusative).HasComment("Винительный падеж");
            entity.Property(e => e.Instrumental).HasComment("Творительный падеж");
            entity.Property(e => e.Prepositional).HasComment("Предложный падеж");
        }
    }
}