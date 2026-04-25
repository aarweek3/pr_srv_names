using AutoMapper;
using DAL.Models.Aggregator;
using DAL.Models.Aggregator.Localizations;
using pr_srv_names.Pages.AGGREGATOR.TagOfAggregator.Dtos;

namespace pr_srv_names.Pages.AGGREGATOR.TagOfAggregator.Mappings
{
    public class TagOfAggregatorProfile : Profile
    {
        public TagOfAggregatorProfile()
        {
            // Tag -> ItemDto
            CreateMap<DAL.Models.Aggregator.TagOfAggregator, TagOfAggregatorItemDto>()
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category != null ? s.Category.Slug : string.Empty))
                .ForMember(d => d.DisplayColor, opt => opt.MapFrom(s =>
                    s.Color == "inherit" && s.Category != null ? s.Category.Color : s.Color))
                .ForMember(d => d.DisplayIcon, opt => opt.MapFrom(s =>
                    s.IconPath ?? (s.Category != null ? s.Category.IconPath : "assets/twotone/av-tag-default.svg")));

            // Tag -> DetailDto
            CreateMap<DAL.Models.Aggregator.TagOfAggregator, TagOfAggregatorDetailDto>();

            // Localization -> DTO
            CreateMap<TagOfAggregatorLocalization, TagOfAggregatorLocalizationDto>()
                .ForMember(d => d.LanguageCode, opt => opt.MapFrom(s => s.LanguageOfAggregator != null ? s.LanguageOfAggregator.Code : string.Empty))
                .ForMember(d => d.LanguageName, opt => opt.MapFrom(s => s.LanguageOfAggregator != null ? s.LanguageOfAggregator.NativeTitle : string.Empty));

            // DTO -> Tag
            CreateMap<TagOfAggregatorCreateDto, DAL.Models.Aggregator.TagOfAggregator>()
                .ForMember(d => d.Localizations, opt => opt.Ignore());

            CreateMap<TagOfAggregatorUpdateDto, DAL.Models.Aggregator.TagOfAggregator>()
                .ForMember(d => d.Localizations, opt => opt.Ignore());

            // DTO Localization -> Entity Localization
            CreateMap<TagOfAggregatorLocalizationDto, TagOfAggregatorLocalization>();
        }
    }
}
