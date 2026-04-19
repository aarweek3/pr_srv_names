using Microsoft.EntityFrameworkCore;
using DAL.Models.NameModels;
using DAL.Models.LocalizationModels;

namespace DAL
{
    public partial class AppDbContext
    {
        // ==========================================
        // DbSets - Основные сущности Имен
        // ==========================================
        public DbSet<NameMain> Names { get; set; }
        public DbSet<Language> Languages { get; set; } 
        public DbSet<NameDetail> NameDetails { get; set; }
        public DbSet<NameUrlForParsing> NameUrlsForParsing { get; set; }

        // ==========================================
        // DbSets - Расширенные сущности Имен
        // ==========================================
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Declension> Declensions { get; set; }
        public DbSet<ForeignVariant> ForeignVariants { get; set; }
        public DbSet<HoroscopeOfName> HoroscopesOfNames { get; set; }
        public DbSet<Planet> Planets { get; set; }
        public DbSet<Zodiac> Zodiacs { get; set; }
        public DbSet<ZodiacHoroscope> ZodiacHoroscopes { get; set; }
        public DbSet<ZodiacTalisman> ZodiacTalismans { get; set; }
    }
}
