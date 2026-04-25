using AutoMapper;
using DAL.Models.Aggregator;
using DAL.Models.Aggregator.Localizations;
using pr_srv_names.Pages.AGGREGATOR.CategoryTagOfAggregator.Dtos;

namespace pr_srv_names.Pages.AGGREGATOR.CategoryTagOfAggregator.Mappings
{
    public class CategoryTagOfAggregatorProfile : Profile
    {
        public CategoryTagOfAggregatorProfile()
        {
            // CategoryTag -> DTOs
            CreateMap<DAL.Models.Aggregator.CategoryTagOfAggregator, CategoryTagOfAggregatorItemDto>()
                .ForMember(d => d.TagsCount, opt => opt.MapFrom(s => s.Tags.Count));

            CreateMap<DAL.Models.Aggregator.CategoryTagOfAggregator, CategoryTagOfAggregatorDetailDto>();

            // Localization -> DTO
            CreateMap<CategoryTagOfAggregatorLocalization, CategoryTagOfAggregatorLocalizationDto>()
                .ForMember(d => d.LanguageCode, opt => opt.MapFrom(s => s.LanguageOfAggregator != null ? s.LanguageOfAggregator.Code : string.Empty))
                .ForMember(d => d.LanguageName, opt => opt.MapFrom(s => s.LanguageOfAggregator != null ? s.LanguageOfAggregator.NativeTitle : string.Empty));

            // DTO -> CategoryTag
            CreateMap<CategoryTagOfAggregatorCreateDto, DAL.Models.Aggregator.CategoryTagOfAggregator>()
                .ForMember(d => d.Localizations, opt => opt.Ignore());

            CreateMap<CategoryTagOfAggregatorUpdateDto, DAL.Models.Aggregator.CategoryTagOfAggregator>()
                .ForMember(d => d.Localizations, opt => opt.Ignore());

            // DTO Localization -> Entity Localization
            CreateMap<CategoryTagOfAggregatorLocalizationDto, CategoryTagOfAggregatorLocalization>();
        }
    }
}
