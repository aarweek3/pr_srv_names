using DAL.Models.NameModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class ZodiacHoroscopeConfiguration : IEntityTypeConfiguration<ZodiacHoroscope>
    {
        public void Configure(EntityTypeBuilder<ZodiacHoroscope> entity)
        {
            entity.HasIndex(e => new { e.ZodiacId, e.NameMainId, e.LanguageId }).HasDatabaseName("IX_ZodiacHoroscopes_ZodiacId_NameMainId_LanguageId");
            entity.HasOne(zh => zh.Zodiac).WithMany().HasForeignKey(zh => zh.ZodiacId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}