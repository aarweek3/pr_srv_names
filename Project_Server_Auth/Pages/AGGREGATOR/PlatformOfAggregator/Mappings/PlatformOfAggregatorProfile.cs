using AutoMapper;
using DAL.Models.Aggregator;
using DAL.Models.Aggregator.Localizations;
using DAL.Models.GeneralModels;
using pr_srv_names.Pages.AGGREGATOR.PlatformOfAggregator.Dtos;
using pr_srv_names.Pages.Shared.Seo.Dtos;
using System.Linq;

namespace pr_srv_names.Pages.AGGREGATOR.PlatformOfAggregator.Mappings
{
    public class PlatformOfAggregatorProfile : Profile
    {
        public PlatformOfAggregatorProfile()
        {
            // --- Основная сущность ---

            // Маппинг для списка (Item)
            CreateMap<DAL.Models.Aggregator.PlatformOfAggregator, PlatformOfAggregatorItemDto>()
                .ForMember(dest => dest.ProgramsCount, opt => opt.MapFrom(src => src.ProgramPlatforms.Count))
                .ForMember(dest => dest.LocalizedName, opt => opt.Ignore()); // Заполняется вручную в сервисе

            // Маппинг для деталей (Detail)
            CreateMap<DAL.Models.Aggregator.PlatformOfAggregator, PlatformOfAggregatorDetailDto>();

            // Маппинг для создания/обновления (в обратную сторону)
            CreateMap<PlatformOfAggregatorCreateDto, DAL.Models.Aggregator.PlatformOfAggregator>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ProgramPlatforms, opt => opt.Ignore());

            CreateMap<PlatformOfAggregatorUpdateDto, DAL.Models.Aggregator.PlatformOfAggregator>()
                .ForMember(dest => dest.ProgramPlatforms, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // --- Локализации ---

            CreateMap<PlatformOfAggregatorLocalization, PlatformOfAggregatorLocalizationDto>()
                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageOfAggregator != null ? src.LanguageOfAggregator.Code : null))
                .ForMember(dest => dest.LanguageName, opt => opt.MapFrom(src => src.LanguageOfAggregator != null ? src.LanguageOfAggregator.Title : null))
                .ForMember(dest => dest.LanguageNativeName, opt => opt.MapFrom(src => src.LanguageOfAggregator != null ? src.LanguageOfAggregator.NativeTitle : null));

            CreateMap<PlatformOfAggregatorLocalizationDto, PlatformOfAggregatorLocalization>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PlatformOfAggregatorId, opt => opt.Ignore())
                .ForMember(dest => dest.PlatformOfAggregator, opt => opt.Ignore())
                .ForMember(dest => dest.LanguageOfAggregator, opt => opt.Ignore())
                .ForMember(dest => dest.SeoDataId, opt => opt.Ignore());

            // --- SEO Data ---

            CreateMap<SeoData, SeoDataDto>().ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
