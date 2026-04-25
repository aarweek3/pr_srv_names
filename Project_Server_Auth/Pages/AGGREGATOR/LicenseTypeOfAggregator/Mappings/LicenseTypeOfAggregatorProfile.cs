using AutoMapper;
using DAL.Models.Aggregator;
using DAL.Models.Aggregator.Localizations;
using DAL.Models.GeneralModels;
using pr_srv_names.Pages.AGGREGATOR.LicenseTypeOfAggregator.Dtos;
using pr_srv_names.Pages.Shared.Seo.Dtos;
using System.Linq;

namespace pr_srv_names.Pages.AGGREGATOR.LicenseTypeOfAggregator.Mappings
{
    public class LicenseTypeOfAggregatorProfile : Profile
    {
        public LicenseTypeOfAggregatorProfile()
        {
            // --- Основная сущность ---

            // Маппинг для списка (Item)
            CreateMap<DAL.Models.Aggregator.LicenseTypeOfAggregator, LicenseTypeOfAggregatorItemDto>()
                .ForMember(dest => dest.LocalizedName, opt => opt.Ignore()); // Заполняется вручную в сервисе

            // Маппинг для деталей (Detail)
            CreateMap<DAL.Models.Aggregator.LicenseTypeOfAggregator, LicenseTypeOfAggregatorDetailDto>();

            // Маппинг для создания/обновления (в обратную сторону)
            CreateMap<LicenseTypeOfAggregatorCreateDto, DAL.Models.Aggregator.LicenseTypeOfAggregator>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Localizations, opt => opt.MapFrom(src => src.Localizations));

            CreateMap<LicenseTypeOfAggregatorUpdateDto, DAL.Models.Aggregator.LicenseTypeOfAggregator>()
                .ForMember(dest => dest.Localizations, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // --- Локализации ---

            CreateMap<LicenseTypeOfAggregatorLocalization, LicenseTypeOfAggregatorLocalizationDto>()
                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageOfAggregator != null ? src.LanguageOfAggregator.Code : null))
                .ForMember(dest => dest.LanguageName, opt => opt.MapFrom(src => src.LanguageOfAggregator != null ? src.LanguageOfAggregator.Title : null))
                .ForMember(dest => dest.LanguageNativeName, opt => opt.MapFrom(src => src.LanguageOfAggregator != null ? src.LanguageOfAggregator.NativeTitle : null));

            CreateMap<LicenseTypeOfAggregatorLocalizationDto, LicenseTypeOfAggregatorLocalization>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.LicenseTypeOfAggregatorId, opt => opt.Ignore())
                .ForMember(dest => dest.LicenseTypeOfAggregator, opt => opt.Ignore())
                .ForMember(dest => dest.LanguageOfAggregator, opt => opt.Ignore())
                .ForMember(dest => dest.SeoDataId, opt => opt.Ignore());

            // --- SEO Data ---

            CreateMap<SeoData, SeoDataDto>().ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
