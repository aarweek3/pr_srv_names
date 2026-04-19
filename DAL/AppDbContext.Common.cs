using Microsoft.EntityFrameworkCore;
using DAL.Models.GeneralModels;
using DAL.Models.LocalizationModels;
using DAL.Models.SampleModels;
using DAL.Models.AuthorizationModels;
using DAL.Models.NameModels;

namespace DAL
{
    public partial class AppDbContext
    {
        // ==========================================
        // Язык интерфейса приложения
        // ==========================================
        public DbSet<LanguageApp> LanguagesApp { get; set; }

        // ==========================================
        // Папки иконок
        // ==========================================
        public DbSet<IconCategory> IconCategories { get; set; }
        public DbSet<Icon> Icons { get; set; }

        // ==========================================
        // DbSets - Пользователи и безопасность
        // ==========================================
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }

        // ==========================================
        // DbSets - Локализованные справочники
        // ==========================================
        public DbSet<Anecdote> Anecdotes { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Fact> Facts { get; set; }
        public DbSet<Metal> Metals { get; set; }
        public DbSet<Number> Numbers { get; set; }
        public DbSet<Patron> Patrons { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<Stone> Stones { get; set; }
        public DbSet<Synonym> Synonyms { get; set; }
        public DbSet<Talent> Talents { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Tree> Trees { get; set; }

        // ==========================================
        // DbSets - Прочие
        // ==========================================
        public DbSet<SeoData> SeoData { get; set; }
        public DbSet<Sample> Samples { get; set; }
        public DbSet<MediaFile> MediaFiles { get; set; }

        // ==========================================
        // Многоязычные сущности SampleMain
        // ==========================================
        public DbSet<SampleMain> SamplesMain { get; set; }
        public DbSet<SampleMainDescription> SamplesMainDescriptions { get; set; }
        public DbSet<SampleMainSeo> SamplesMainSeo { get; set; }
        public DbSet<SampleMainDescriptionSeo> SamplesMainDescriptionsSeo { get; set; }

        // ==========================================
        // Платформы (Business)
        // ==========================================
        public DbSet<DAL.Models.Business.Platform> Platforms { get; set; }
        public DbSet<DAL.Models.Business.PlatformTranslation> PlatformTranslations { get; set; }
    }
}
