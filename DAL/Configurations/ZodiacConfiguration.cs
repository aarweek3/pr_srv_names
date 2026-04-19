using DAL.Models.NameModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class ZodiacConfiguration : IEntityTypeConfiguration<Zodiac>
    {
        public void Configure(EntityTypeBuilder<Zodiac> entity)
        {
            entity.HasIndex(e => new { e.NameMainId, e.LanguageId }).HasDatabaseName("IX_Zodiacs_NameMainId_LanguageId");
            entity.Property(e => e.ZodiacSign).HasComment("Номер знака зодиака (1-12)");
            entity.Property(e => e.StartMonth).HasComment("Месяц начала периода знака (1-12)");
            entity.Property(e => e.StartDay).HasComment("День начала периода знака (1-31)");
            entity.Property(e => e.EndMonth).HasComment("Месяц окончания периода знака (1-12)");
            entity.Property(e => e.EndDay).HasComment("День окончания периода знака (1-31)");
            entity.Property(e => e.Symbol).HasComment("Астрологический символ знака");

            entity.HasMany(z => z.Talismans).WithOne(t => t.Zodiac).HasForeignKey(t => t.ZodiacId).OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(z => z.Horoscopes).WithOne(h => h.Zodiac).HasForeignKey(h => h.ZodiacId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}