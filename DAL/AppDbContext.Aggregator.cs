using Microsoft.EntityFrameworkCore;
using DAL.Models.Aggregator;
using DAL.Models.Aggregator.Localizations;

namespace DAL
{
    public partial class AppDbContext
    {
        // ==========================================
        // Aggregator (v3.5)
        // ==========================================
        public DbSet<LanguageOfAggregator> LanguagesOfAggregator { get; set; }
        public DbSet<AggregatorSource> AggregatorSources { get; set; }
        
        public DbSet<CategoryOfAggregator> CategoriesOfAggregator { get; set; }
        public DbSet<CategoryOfAggregatorLocalization> CategoryLocalizations { get; set; }
        
        public DbSet<DeveloperOfAggregator> DevelopersOfAggregator { get; set; }
        public DbSet<DeveloperOfAggregatorLocalization> DeveloperLocalizations { get; set; }
        
        public DbSet<PlatformOfAggregator> PlatformsOfAggregator { get; set; }
        public DbSet<PlatformOfAggregatorLocalization> PlatformOfAggregatorLocalizations { get; set; }
        
        public DbSet<LicenseTypeOfAggregator> LicenseTypesOfAggregator { get; set; }
        public DbSet<LicenseTypeOfAggregatorLocalization> LicenseTypeLocalizations { get; set; }
        
        public DbSet<ProgramOfAggregator> ProgramsOfAggregator { get; set; }
        public DbSet<ProgramOfAggregatorLocalization> ProgramLocalizations { get; set; }
        public DbSet<ProgramPlatformOfAggregator> ProgramPlatformsOfAggregator { get; set; }
        public DbSet<ProgramMarketDataOfAggregator> ProgramMarketData { get; set; }
        
        public DbSet<VersionOfAggregator> VersionsOfAggregator { get; set; }
        public DbSet<VersionOfAggregatorLocalization> VersionLocalizations { get; set; }
        
        public DbSet<ScreenshotOfAggregator> ScreenshotsOfAggregator { get; set; }
        public DbSet<ScreenshotOfAggregatorLocalization> ScreenshotLocalizations { get; set; }
        
        public DbSet<VideoOfAggregator> VideosOfAggregator { get; set; }
        public DbSet<VideoOfAggregatorLocalization> VideoLocalizations { get; set; }
        
        public DbSet<DownloadLinkOfAggregator> DownloadLinksOfAggregator { get; set; }
        public DbSet<DownloadLinkOfAggregatorLocalization> DownloadLinkLocalizations { get; set; }
        public DbSet<DownloadLogOfAggregator> DownloadLogsOfAggregator { get; set; }

        public DbSet<TagOfAggregator> TagsOfAggregator { get; set; }
        public DbSet<TagOfAggregatorLocalization> TagLocalizations { get; set; }
        public DbSet<ProgramTagOfAggregator> ProgramTagsOfAggregator { get; set; }
    }
}
