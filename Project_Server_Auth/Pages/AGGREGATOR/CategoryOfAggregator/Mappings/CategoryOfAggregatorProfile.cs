using AutoMapper;
using DAL.Models.Aggregator.Localizations;
using pr_srv_names.Pages.AGGREGATOR.CategoryOfAggregator.Dtos;
using System.Linq;

using CategoryEntity = DAL.Models.Aggregator.CategoryOfAggregator;

namespace pr_srv_names.Pages.AGGREGATOR.CategoryOfAggregator.Mappings
{
    public class CategoryOfAggregatorProfile : Profile
    {
        public CategoryOfAggregatorProfile()
        {
            CreateMap<CategoryEntity, CategoryOfAggregatorItemDto>()
                .ForMember(d => d.LocalizedName, o => o.Ignore()) // Мапим вручную в сервисе
                .ForMember(d => d.ProgramsCount, o => o.Ignore()) // Мапим вручную в сервисе
                .ForMember(d => d.ChildrenCount, o => o.MapFrom(s => s.Children.Count))
                .ForMember(d => d.Children, o => o.Ignore()); // Мапим для дерева отдельно

            CreateMap<CategoryEntity, CategoryOfAggregatorDetailDto>();

            CreateMap<CategoryOfAggregatorLocalization, CategoryOfAggregatorLocalizationDto>()
                .ForMember(d => d.LanguageCode, o => o.MapFrom(s => s.LanguageOfAggregator != null ? s.LanguageOfAggregator.Code : null))
                .ForMember(d => d.LanguageName, o => o.MapFrom(s => s.LanguageOfAggregator != null ? s.LanguageOfAggregator.NativeTitle : null));

            CreateMap<CategoryOfAggregatorCreateDto, CategoryEntity>()
                .ForMember(d => d.Localizations, o => o.MapFrom(s => s.Localizations));

            CreateMap<CategoryOfAggregatorUpdateDto, CategoryEntity>()
                .ForMember(d => d.Localizations, o => o.MapFrom(s => s.Localizations));

            CreateMap<CategoryOfAggregatorLocalizationDto, CategoryOfAggregatorLocalization>();
        }
    }
}
