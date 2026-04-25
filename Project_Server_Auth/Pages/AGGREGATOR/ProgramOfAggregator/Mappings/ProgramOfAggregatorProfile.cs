using AutoMapper;
using DAL.Models.Aggregator;
using DAL.Models.Aggregator.Localizations;
using pr_srv_names.Pages.AGGREGATOR.ProgramOfAggregator.Dtos;
using System.Linq;

namespace pr_srv_names.Pages.AGGREGATOR.ProgramOfAggregator.Mappings
{
    public class ProgramOfAggregatorProfile : Profile
    {
        public ProgramOfAggregatorProfile()
        {
            // Program
            CreateMap<DAL.Models.Aggregator.ProgramOfAggregator, ProgramOfAggregatorItemDto>()
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.CategoryOfAggregator.CanonicalName))
                .ForMember(d => d.DeveloperName,
                    o => o.MapFrom(s => s.DeveloperOfAggregator != null ? s.DeveloperOfAggregator.Name : null))
                .ForMember(d => d.VersionsCount, o => o.MapFrom(s => s.Versions.Count))
                .ForMember(d => d.LocalizedName, o => o.Ignore());

            CreateMap<DAL.Models.Aggregator.ProgramOfAggregator, ProgramOfAggregatorDetailDto>()
                .ForMember(d => d.PlatformIds,
                    o => o.MapFrom(s => s.ProgramPlatforms.Select(p => p.PlatformOfAggregatorId)))
                .ForMember(d => d.TagIds, o => o.MapFrom(s => s.ProgramTags.Select(t => t.TagOfAggregatorId)));

            CreateMap<ProgramOfAggregatorCreateDto, DAL.Models.Aggregator.ProgramOfAggregator>()
                .ForMember(d => d.ProgramPlatforms, o => o.Ignore())
                .ForMember(d => d.ProgramTags, o => o.Ignore());

            CreateMap<ProgramOfAggregatorUpdateDto, DAL.Models.Aggregator.ProgramOfAggregator>()
                .ForMember(d => d.ProgramPlatforms, o => o.Ignore())
                .ForMember(d => d.ProgramTags, o => o.Ignore());

            // Localizations
            CreateMap<ProgramOfAggregatorLocalization, ProgramOfAggregatorLocalizationDto>()
                .ForMember(d => d.LanguageCode, o => o.MapFrom(s => s.LanguageOfAggregator.Code))
                .ForMember(d => d.LanguageName, o => o.MapFrom(s => s.LanguageOfAggregator.NativeTitle))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.LocalizedName));

            CreateMap<ProgramOfAggregatorLocalizationDto, ProgramOfAggregatorLocalization>()
                .ForMember(d => d.LocalizedName, o => o.MapFrom(s => s.Name));

            // Versions
            CreateMap<VersionOfAggregator, VersionOfAggregatorItemDto>()
                .ForMember(d => d.DownloadLinksCount, o => o.MapFrom(s => s.DownloadLinks.Count));

            CreateMap<VersionOfAggregator, VersionOfAggregatorDetailDto>();

            CreateMap<VersionOfAggregatorCreateDto, VersionOfAggregator>();
            CreateMap<VersionOfAggregatorUpdateDto, VersionOfAggregator>();

            CreateMap<VersionOfAggregatorLocalization, VersionOfAggregatorLocalizationDto>()
                .ForMember(d => d.LanguageCode, o => o.MapFrom(s => s.LanguageOfAggregator.Code))
                .ForMember(d => d.LanguageName, o => o.MapFrom(s => s.LanguageOfAggregator.NativeTitle))
                .ForMember(d => d.Changelog, o => o.MapFrom(s => s.WhatsNew));

            CreateMap<VersionOfAggregatorLocalizationDto, VersionOfAggregatorLocalization>()
                .ForMember(d => d.WhatsNew, o => o.MapFrom(s => s.Changelog));

            // Download Links
            CreateMap<DownloadLinkOfAggregator, DownloadLinkOfAggregatorDto>();
            CreateMap<DownloadLinkOfAggregatorDto, DownloadLinkOfAggregator>();

            CreateMap<DownloadLinkOfAggregatorLocalization, DownloadLinkOfAggregatorLocalizationDto>()
                .ForMember(d => d.LanguageCode, o => o.MapFrom(s => s.LanguageOfAggregator.Code))
                .ForMember(d => d.Label, o => o.MapFrom(s => s.Title));

            CreateMap<DownloadLinkOfAggregatorLocalizationDto, DownloadLinkOfAggregatorLocalization>()
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Label));
        }
    }
}