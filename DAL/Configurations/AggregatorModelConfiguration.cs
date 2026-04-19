using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using DAL.Models.Aggregator;
using DAL.Models.Aggregator.Base;
using DAL.Models.Aggregator.Interfaces;
using DAL.Models.Aggregator.Localizations;

namespace DAL.Configurations
{
    public static class AggregatorModelConfiguration
    {
        public static void ConfigureAggregatorModels(this ModelBuilder modelBuilder)
        {
            // =============================================
            // 1. Уникальные индексы
            // =============================================
            modelBuilder.Entity<ProgramOfAggregator>().HasIndex(e => e.Slug).IsUnique();
            modelBuilder.Entity<CategoryOfAggregator>().HasIndex(e => e.Slug).IsUnique();
            modelBuilder.Entity<DeveloperOfAggregator>().HasIndex(e => e.Slug).IsUnique();
            modelBuilder.Entity<PlatformOfAggregator>().HasIndex(e => e.SystemCode).IsUnique();
            modelBuilder.Entity<AggregatorSource>().HasIndex(e => e.Slug).IsUnique();
            modelBuilder.Entity<LicenseTypeOfAggregator>().HasIndex(e => e.Slug).IsUnique();
            modelBuilder.Entity<TagOfAggregator>().HasIndex(e => e.Slug).IsUnique();

            modelBuilder.Entity<VersionOfAggregator>()
                .HasIndex(v => new { v.ProgramOfAggregatorId, v.VersionNumber })
                .IsUnique();

            modelBuilder.Entity<VersionOfAggregator>()
                .HasIndex(v => new { v.ProgramOfAggregatorId, v.IsLatest })
                .IsUnique()
                .HasFilter("\"IsLatest\" = true");

            modelBuilder.Entity<ProgramPlatformOfAggregator>()
                .HasIndex(pp => new { pp.ProgramOfAggregatorId, pp.PlatformOfAggregatorId })
                .IsUnique();

            // =============================================
            // 2. Уникальные локализации (Entity + Language)
            // =============================================
            modelBuilder.Entity<ProgramOfAggregatorLocalization>()
                .HasIndex(e => new { e.ProgramOfAggregatorId, e.LanguageOfAggregatorId }).IsUnique();

            modelBuilder.Entity<CategoryOfAggregatorLocalization>()
                .HasIndex(e => new { e.CategoryOfAggregatorId, e.LanguageOfAggregatorId }).IsUnique();

            modelBuilder.Entity<DeveloperOfAggregatorLocalization>()
                .HasIndex(e => new { e.DeveloperOfAggregatorId, e.LanguageOfAggregatorId }).IsUnique();

            modelBuilder.Entity<PlatformOfAggregatorLocalization>()
                .HasIndex(e => new { e.PlatformOfAggregatorId, e.LanguageOfAggregatorId }).IsUnique();

            modelBuilder.Entity<VersionOfAggregatorLocalization>()
                .HasIndex(e => new { e.VersionOfAggregatorId, e.LanguageOfAggregatorId }).IsUnique();

            modelBuilder.Entity<ScreenshotOfAggregatorLocalization>()
                .HasIndex(e => new { e.ScreenshotOfAggregatorId, e.LanguageOfAggregatorId }).IsUnique();

            modelBuilder.Entity<VideoOfAggregatorLocalization>()
                .HasIndex(e => new { e.VideoOfAggregatorId, e.LanguageOfAggregatorId }).IsUnique();

            modelBuilder.Entity<DownloadLinkOfAggregatorLocalization>()
                .HasIndex(e => new { e.DownloadLinkOfAggregatorId, e.LanguageOfAggregatorId }).IsUnique();

            modelBuilder.Entity<LicenseTypeOfAggregatorLocalization>()
                .HasIndex(e => new { e.LicenseTypeOfAggregatorId, e.LanguageOfAggregatorId }).IsUnique();

            modelBuilder.Entity<TagOfAggregatorLocalization>()
                .HasIndex(e => new { e.TagOfAggregatorId, e.LanguageOfAggregatorId }).IsUnique();

            // =============================================
            // 3. MarketData уникальность
            // =============================================
            modelBuilder.Entity<ProgramMarketDataOfAggregator>()
                .HasIndex(e => new { e.ProgramOfAggregatorId, e.LanguageOfAggregatorId, e.AggregatorSourceId })
                .IsUnique();

            // =============================================
            // 4. Защита языка по умолчанию
            // =============================================
            modelBuilder.Entity<LanguageOfAggregator>()
                .HasIndex(e => e.IsDefault)
                .IsUnique()
                .HasFilter("\"IsDefault\" = true");

            // =============================================
            // 5. Индексы на DownloadLink.VersionId
            // =============================================
            modelBuilder.Entity<DownloadLinkOfAggregator>()
                .HasIndex(d => d.VersionOfAggregatorId);

            modelBuilder.Entity<DownloadLogOfAggregator>().HasIndex(l => l.VersionOfAggregatorId);
            modelBuilder.Entity<DownloadLogOfAggregator>().HasIndex(l => l.CreatedAt);

            // =============================================
            // 6. Индексы для аудита и производительности
            // =============================================
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IAuditableOfAggregator).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(IAuditableOfAggregator.CreatedAt))
                        .HasDefaultValueSql("CURRENT_TIMESTAMP")
                        .ValueGeneratedOnAdd();

                    modelBuilder.Entity(entityType.ClrType).HasIndex("CreatedAt");
                    modelBuilder.Entity(entityType.ClrType).HasIndex("UpdatedAt");
                }

                if (typeof(ISoftDeletableOfAggregator).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType).HasIndex("IsDeleted");
                }
            }

            // =============================================
            // 7. Computed column для RatingNormalized
            // =============================================
            modelBuilder.Entity<ProgramMarketDataOfAggregator>()
                .Property(e => e.RatingNormalized)
                .HasComputedColumnSql(
                    "CASE WHEN \"RatingMax\" > 0 THEN \"RatingValue\" / \"RatingMax\" ELSE NULL END",
                    stored: true)
                .ValueGeneratedOnAddOrUpdate();

            // =============================================
            // 8. Глобальный Query Filter для Soft Delete
            // =============================================
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletableOfAggregator).IsAssignableFrom(entityType.ClrType))
                {
                    // Для динамической настройки фильтра используем вспомогательный метод
                    var method = typeof(AggregatorModelConfiguration).GetMethod(nameof(SetSoftDeleteFilter), BindingFlags.Static | BindingFlags.NonPublic);
                    var genericMethod = method.MakeGenericMethod(entityType.ClrType);
                    genericMethod.Invoke(null, new object[] { modelBuilder });
                }
            }


            // =============================================
            // 10. Дополнительные связи
            // =============================================
            modelBuilder.Entity<ProgramOfAggregator>()
                .HasMany(p => p.Versions)
                .WithOne(v => v.ProgramOfAggregator)
                .HasForeignKey(v => v.ProgramOfAggregatorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProgramPlatformOfAggregator>()
                .HasOne(pp => pp.ProgramOfAggregator)
                .WithMany(p => p.ProgramPlatforms)
                .HasForeignKey(pp => pp.ProgramOfAggregatorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProgramPlatformOfAggregator>()
                .HasOne(pp => pp.PlatformOfAggregator)
                .WithMany(pl => pl.ProgramPlatforms)
                .HasForeignKey(pp => pp.PlatformOfAggregatorId)
                .OnDelete(DeleteBehavior.Restrict);

            // =============================================
            // 11. Теги
            // =============================================
            modelBuilder.Entity<ProgramTagOfAggregator>()
                .HasIndex(pt => new { pt.ProgramOfAggregatorId, pt.TagOfAggregatorId })
                .IsUnique();

            modelBuilder.Entity<ProgramTagOfAggregator>()
                .HasOne(pt => pt.ProgramOfAggregator)
                .WithMany(p => p.ProgramTags)
                .HasForeignKey(pt => pt.ProgramOfAggregatorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProgramTagOfAggregator>()
                .HasOne(pt => pt.TagOfAggregator)
                .WithMany(t => t.ProgramTags)
                .HasForeignKey(pt => pt.TagOfAggregatorId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void SetSoftDeleteFilter<TEntity>(ModelBuilder modelBuilder) where TEntity : class, ISoftDeletableOfAggregator
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
        }
    }
}
