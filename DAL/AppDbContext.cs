using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DAL.Configurations;
using DAL.Models.AuthorizationModels;

namespace DAL
{
    public partial class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

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
                .OnDelete(DeleteBehavior.Cascade);

            // Применение конфигураций сущностей
            builder.ApplyConfiguration(new LanguageAppConfiguration());
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
            builder.ApplyConfiguration(new IconConfiguration());
            builder.ApplyConfiguration(new MediaFileConfiguration());
            builder.ApplyConfiguration(new SampleMainConfiguration());
            builder.ApplyConfiguration(new SampleMainDescriptionConfiguration());
            builder.ApplyConfiguration(new SampleMainSeoConfiguration());
            builder.ApplyConfiguration(new SampleMainDescriptionSeoConfiguration());

            // Инициализация моделей Агрегатора (v3.5)
            builder.ConfigureAggregatorModels();
        }
    }
}