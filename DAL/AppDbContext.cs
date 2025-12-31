using DAL.Models;
using DAL.Models.Base;
using DAL.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // ==========================================
        // DbSets - Пользователи и безопасность
        // ==========================================
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }

        // ==========================================
        // DbSets - Основные сущности
        // ==========================================
        public DbSet<NameMain> Names { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<NameDetail> NameDetails { get; set; }

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
        // DbSets - Расширенные локализованные сущности
        // ==========================================
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Declension> Declensions { get; set; }
        public DbSet<ForeignVariant> ForeignVariants { get; set; }
        public DbSet<HoroscopeOfName> HoroscopesOfNames { get; set; }
        public DbSet<NameUrlForParsing> NameUrlsForParsing { get; set; }
        public DbSet<Planet> Planets { get; set; }
        public DbSet<Zodiac> Zodiacs { get; set; }
        public DbSet<ZodiacHoroscope> ZodiacHoroscopes { get; set; }
        public DbSet<ZodiacTalisman> ZodiacTalismans { get; set; }

        // ==========================================
        // DbSets - Прочие
        // ==========================================
        public DbSet<SeoData> SeoData { get; set; }
        public DbSet<Sample> Samples { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Настройка таблиц Identity
            builder.Entity<Microsoft.AspNetCore.Identity.IdentityRole>(entity => { entity.ToTable("Roles"); });
            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            // Настройка связи One-to-One для UserSettings
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Settings)
                .WithOne(s => s.User)
                .HasForeignKey<UserSettings>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade); // При удалении User удаляются и Settings

            // Применение конфигураций
            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new UserSessionConfiguration());
            builder.ApplyConfiguration(new ActivityLogConfiguration());
            builder.ApplyConfiguration(new NameMainConfiguration());
            builder.ApplyConfiguration(new LanguageConfiguration());
            builder.ApplyConfiguration(new NameDetailConfiguration());
            builder.ApplyConfiguration(new AnecdoteConfiguration());
            builder.ApplyConfiguration(new ColorConfiguration());
            builder.ApplyConfiguration(new FactConfiguration());
            builder.ApplyConfiguration(new MetalConfiguration());
            builder.ApplyConfiguration(new NumberConfiguration());
            builder.ApplyConfiguration(new PatronConfiguration());
            builder.ApplyConfiguration(new PlantConfiguration());
            builder.ApplyConfiguration(new ProfessionConfiguration());
            builder.ApplyConfiguration(new StoneConfiguration());
            builder.ApplyConfiguration(new SynonymConfiguration());
            builder.ApplyConfiguration(new TalentConfiguration());
            builder.ApplyConfiguration(new AnimalConfiguration());
            builder.ApplyConfiguration(new TreeConfiguration());
            builder.ApplyConfiguration(new PlanetConfiguration());
            builder.ApplyConfiguration(new ForeignVariantConfiguration());
            builder.ApplyConfiguration(new DeclensionConfiguration());
            builder.ApplyConfiguration(new ZodiacConfiguration());
            builder.ApplyConfiguration(new ZodiacTalismanConfiguration());
            builder.ApplyConfiguration(new ZodiacHoroscopeConfiguration());
            builder.ApplyConfiguration(new HoroscopeOfNameConfiguration());
            builder.ApplyConfiguration(new CommentConfiguration());
            builder.ApplyConfiguration(new NameUrlForParsingConfiguration());
            builder.ApplyConfiguration(new SeoDataConfiguration());
        }

        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditFields()
        {
            var now = DateTime.UtcNow;

            // Обновление ApplicationUser
            var userEntries = ChangeTracker.Entries<ApplicationUser>().Where(e => e.State == EntityState.Modified);
            foreach (var entry in userEntries)
            {
                entry.Entity.UpdatedAt = now;
            }

            // Обновление UserSession
            var sessionEntries = ChangeTracker.Entries<UserSession>().Where(e => e.State == EntityState.Modified);
            foreach (var entry in sessionEntries)
            {
                var session = entry.Entity;
                if (session.IsRevoked && session.RevokedAt == null)
                {
                    session.RevokedAt = now;
                }
            }

            // Обновление BaseEntity
            var baseEntityEntries = ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.State is EntityState.Added or EntityState.Modified);
            foreach (var entry in baseEntityEntries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.IsActive = true;
                }

                entry.Entity.UpdatedAt = now;
            }

            // Обновление SeoData
            var seoEntries = ChangeTracker.Entries<SeoData>()
                .Where(e => e.State is EntityState.Added or EntityState.Modified);
            foreach (var entry in seoEntries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy ??= "system";
                }

                entry.Entity.UpdatedAt = now;
                entry.Entity.UpdatedBy ??= "system";
                entry.Entity.ModifiedDate = now;
            }
        }
    }
}