using DAL.Models.NameModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class HoroscopeOfNameConfiguration : IEntityTypeConfiguration<HoroscopeOfName>
    {
        public void Configure(EntityTypeBuilder<HoroscopeOfName> entity)
        {
            entity.HasIndex(e => new { e.ZodiacId, e.NameMainId, e.LanguageId }).HasDatabaseName("IX_HoroscopesOfNames_ZodiacId_NameMainId_LanguageId");
            entity.Property(e => e.HoroscopeDate).HasComment("Дата гороскопа");
            entity.Property(e => e.HoroscopeType).HasComment("Тип гороскопа (ежедневный, еженедельный, годовой)");
            entity.HasOne(h => h.Zodiac).WithMany(z => z.Horoscopes).HasForeignKey(h => h.ZodiacId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}