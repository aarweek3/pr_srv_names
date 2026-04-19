using DAL.Models.NameModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class ZodiacTalismanConfiguration : IEntityTypeConfiguration<ZodiacTalisman>
    {
        public void Configure(EntityTypeBuilder<ZodiacTalisman> entity)
        {
            entity.HasIndex(e => new { e.ZodiacId, e.NameMainId, e.LanguageId }).HasDatabaseName("IX_ZodiacTalismans_ZodiacId_NameMainId_LanguageId");
            entity.Property(e => e.TalismanType).HasComment("Тип талисмана (камень, металл, растение)");
        }
    }
}