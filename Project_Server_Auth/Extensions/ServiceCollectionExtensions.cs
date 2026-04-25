using DAL;
using DAL.Interfaces;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using pr_srv_names.Pages.AdvancedImageEditor.Services;
using pr_srv_names.Pages.Anecdote.Interfaces;
using pr_srv_names.Pages.Anecdote.Services;
using pr_srv_names.Pages.Health.Services;
using pr_srv_names.Pages.Language.Intarfaces;
using pr_srv_names.Pages.Language.Services;
using pr_srv_names.Pages.LanguageApp.Interfaces;
using pr_srv_names.Pages.LanguageApp.Services;
using pr_srv_names.Pages.NameMain.Intarfaces;
using pr_srv_names.Pages.NameMain.Services;
using pr_srv_names.Pages.Sample.Interfaces;
using pr_srv_names.Pages.Sample.Services;
using pr_srv_names.Pages.SampleMain.Interfaces;
using pr_srv_names.Pages.SampleMain.Services;
using pr_srv_names.Pages.SampleMainSeo.Interfaces;
using pr_srv_names.Pages.SampleMainSeo.Services;
using pr_srv_names.Pages.UserSetting.Interfaces;
using pr_srv_names.Pages.UserSetting.Services;
using pr_srv_names.Services.Editor;
using Project_Server_Auth.Pages.CategoryRepository.Interfaces;
using Project_Server_Auth.Pages.CategoryRepository.Services;
using pr_srv_names.Pages.Platform.Interfaces;
using pr_srv_names.Pages.Platform.Services;
using DAL.Repositories.Interfaces;
using DAL.Repositories;
using pr_srv_names.Pages.Aggregator.LanguageOfAggregator.Interfaces;
using pr_srv_names.Pages.Aggregator.LanguageOfAggregator.Services;
using pr_srv_names.Pages.AGGREGATOR.PlatformOfAggregator.Services;
using pr_srv_names.Pages.AGGREGATOR.LicenseTypeOfAggregator.Interfaces;
using pr_srv_names.Pages.AGGREGATOR.LicenseTypeOfAggregator.Services;
using pr_srv_names.Pages.AGGREGATOR.PlatformOfAggregator.Interfaces;
using pr_srv_names.Pages.AGGREGATOR.DeveloperOfAggregator.Interfaces;
using pr_srv_names.Pages.AGGREGATOR.DeveloperOfAggregator.Services;
using pr_srv_names.Pages.AGGREGATOR.CategoryTagOfAggregator.Interfaces;
using pr_srv_names.Pages.AGGREGATOR.CategoryTagOfAggregator.Services;
using pr_srv_names.Pages.AGGREGATOR.TagOfAggregator.Interfaces;
using pr_srv_names.Pages.AGGREGATOR.TagOfAggregator.Services;
using FluentValidation;
using pr_srv_names.Pages.AGGREGATOR.TagOfAggregator.Validators;
using pr_srv_names.Pages.AGGREGATOR.TagOfAggregator.Dtos;
using pr_srv_names.Pages.AGGREGATOR.CategoryTagOfAggregator.Dtos;
using pr_srv_names.Pages.AGGREGATOR.CategoryOfAggregator.Interfaces;
using pr_srv_names.Pages.AGGREGATOR.CategoryOfAggregator.Services;
using pr_srv_names.Pages.AGGREGATOR.CategoryOfAggregator.Dtos;
using pr_srv_names.Pages.AGGREGATOR.CategoryOfAggregator.Validators;
using pr_srv_names.Pages.AGGREGATOR.ProgramOfAggregator.Interfaces;
using pr_srv_names.Pages.AGGREGATOR.ProgramOfAggregator.Services;
using pr_srv_names.Pages.AGGREGATOR.ProgramOfAggregator.Validators;
using pr_srv_names.Pages.AGGREGATOR.ProgramOfAggregator.Dtos;


namespace pr_srv_names.Extensions
{
    /// <summary>
    /// Extension методы для регистрации доменных сервисов и репозиториев
    /// </summary>
    public static class DomainServicesExtensions
    {
        /// <summary>
        /// Регистрация всех доменных сервисов и репозиториев приложения
        /// </summary>
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Language Services
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<ILanguageAppService, LanguageAppService>();
            services.AddScoped<ILanguageAppRepository, LanguageAppRepository>();

            // Sample Services
            services.AddScoped<ISampleService, SampleService>();
            services.AddScoped<ISampleRepository, SampleRepository>();
            services.AddScoped<ISampleMainService, SampleMainService>();
            services.AddScoped<ISampleMainRepository, SampleMainRepository>();
            services.AddScoped<ISampleMainDescriptionRepository, SampleMainDescriptionRepository>();
            services.AddScoped<ISampleMainSeoService, SampleMainSeoService>();
            services.AddScoped<ISampleMainSeoRepository, SampleMainSeoRepository>();

            // Platform Services
            services.AddScoped<IPlatformService, PlatformService>();
            services.AddScoped<IPlatformRepository, PlatformRepository>();

            // Aggregator Languages
            services.AddScoped<ILanguageOfAggregatorService, LanguageOfAggregatorService>();
            services.AddScoped<ILanguageOfAggregatorRepository, LanguageOfAggregatorRepository>();

            // Aggregator Platforms
            services.AddScoped<IPlatformOfAggregatorService, PlatformOfAggregatorService>();
            services.AddScoped<IPlatformOfAggregatorRepository, PlatformOfAggregatorRepository>();

            // Aggregator License Types
            services.AddScoped<ILicenseTypeOfAggregatorService, LicenseTypeOfAggregatorService>();
            services.AddScoped<ILicenseTypeOfAggregatorRepository, LicenseTypeOfAggregatorRepository>();

            // Aggregator Developers
            services.AddScoped<IDeveloperOfAggregatorService, DeveloperOfAggregatorService>();
            services.AddScoped<IDeveloperOfAggregatorRepository, DeveloperOfAggregatorRepository>();

            // Aggregator Tags
            services.AddScoped<ICategoryTagOfAggregatorService, CategoryTagOfAggregatorService>();
            services.AddScoped<ICategoryTagOfAggregatorRepository, CategoryTagOfAggregatorRepository>();
            services.AddScoped<ITagOfAggregatorService, TagOfAggregatorService>();
            services.AddScoped<ITagOfAggregatorRepository, TagOfAggregatorRepository>();

            // Aggregator Categories
            services.AddScoped<ICategoryOfAggregatorService, CategoryOfAggregatorService>();
            services.AddScoped<ICategoryOfAggregatorRepository, CategoryOfAggregatorRepository>();
            services.AddScoped<IProgramOfAggregatorRepository, ProgramOfAggregatorRepository>();
            services.AddScoped<IVersionOfAggregatorRepository, VersionOfAggregatorRepository>();
            services.AddScoped<IDownloadLinkOfAggregatorRepository, DownloadLinkOfAggregatorRepository>();
            services.AddScoped<IAggregatorSourceRepository, AggregatorSourceRepository>();
            services.AddScoped<IScreenshotOfAggregatorRepository, ScreenshotOfAggregatorRepository>();
            services.AddScoped<IVideoOfAggregatorRepository, VideoOfAggregatorRepository>();
            services.AddScoped<IDownloadLogOfAggregatorRepository, DownloadLogOfAggregatorRepository>();

            services.AddScoped<IProgramOfAggregatorService, ProgramOfAggregatorService>();


            // Validators
            services.AddScoped<IValidator<CategoryTagOfAggregatorCreateDto>, CategoryTagOfAggregatorCreateValidator>();
            services.AddScoped<IValidator<CategoryTagOfAggregatorUpdateDto>, CategoryTagOfAggregatorUpdateValidator>();
            services.AddScoped<IValidator<TagOfAggregatorCreateDto>, TagOfAggregatorCreateValidator>();
            services.AddScoped<IValidator<TagOfAggregatorUpdateDto>, TagOfAggregatorUpdateValidator>();

            services.AddScoped<IValidator<CategoryOfAggregatorCreateDto>, CategoryOfAggregatorCreateValidator>();
            services.AddScoped<IValidator<CategoryOfAggregatorUpdateDto>, CategoryOfAggregatorUpdateValidator>();
            services.AddScoped<IValidator<CategoryOfAggregatorPageRequestDto>, CategoryOfAggregatorPageRequestValidator>();

            services.AddScoped<IValidator<ProgramOfAggregatorCreateDto>, ProgramOfAggregatorCreateValidator>();
            services.AddScoped<IValidator<ProgramOfAggregatorUpdateDto>, ProgramOfAggregatorUpdateValidator>();
            services.AddScoped<IValidator<ProgramOfAggregatorPageRequestDto>, ProgramOfAggregatorPageRequestValidator>();
            services.AddScoped<IValidator<VersionOfAggregatorCreateDto>, VersionOfAggregatorCreateValidator>();
            services.AddScoped<IValidator<VersionOfAggregatorUpdateDto>, VersionOfAggregatorUpdateValidator>();


            // Anecdote Services
            services.AddScoped<IAnecdoteService, AnecdoteService>();
            services.AddScoped<IAnecdoteRepository, AnecdoteRepository>();

            // Name Services
            services.AddScoped<INameMainService, NameMainService>();
            services.AddScoped<INameMainRepository, NameMainRepository>();

            // User Settings
            services.AddScoped<IUserSettingsRepository, UserSettingsRepository>();
            services.AddScoped<IUserSettingsService, UserSettingsService>();

            // Image Processing
            services.AddScoped<IEditorImageService, EditorImageService>();
            services.AddScoped<IAdvancedImageProcessingService, AdvancedImageProcessingService>();

            // Icon Services
            services.AddScoped<pr_srv_names.Pages.Icons.Interfaces.IIconGetService, 
                pr_srv_names.Pages.Icons.Services.IconGetService>();
            services.AddScoped<pr_srv_names.Pages.Icons.Interfaces.IIconLaboratoryService, 
                pr_srv_names.Pages.Icons.Services.IconLaboratoryService>();
            services.AddScoped<pr_srv_names.Pages.Icons.Interfaces.IIconService, 
                pr_srv_names.Pages.Icons.Services.IconService>();
            services.AddScoped<IIconCategoryRepository, IconCategoryRepository>();
            services.AddScoped<IIconCategoryService, IconCategoryService>();

            // Health Check
            services.AddScoped<IHealthCheckEnhancedService, HealthCheckEnhancedService>();

            // Maintenance Helpers
            services.AddScoped<Project_Server_Auth.Services.Maintenance.Interfaces.IMaintenanceSeeder, 
                Project_Server_Auth.Services.Maintenance.JsonMaintenanceSeeder>();

            return services;
        }

        /// <summary>
        /// Регистрация HealthChecks с проверкой базы данных
        /// </summary>
        public static IServiceCollection AddApplicationHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<AppDbContext>(
                    name: "PostgreSQL Database",
                    tags: new[] { "db", "infra", "ready" });

            return services;
        }
    }
}
