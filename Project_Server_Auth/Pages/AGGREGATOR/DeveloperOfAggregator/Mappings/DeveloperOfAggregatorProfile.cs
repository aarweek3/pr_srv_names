using AutoMapper;
using DAL.Models.Aggregator;
using DAL.Models.Aggregator.Localizations;
using pr_srv_names.Pages.AGGREGATOR.DeveloperOfAggregator.Dtos;

namespace pr_srv_names.Pages.AGGREGATOR.DeveloperOfAggregator.Mappings
{
    public class DeveloperOfAggregatorProfile : Profile
    {
        public DeveloperOfAggregatorProfile()
        {
            CreateMap<DAL.Models.Aggregator.DeveloperOfAggregator, DeveloperOfAggregatorItemDto>()
                .ForMember(dest => dest.ProgramsCount, opt => opt.MapFrom(src => src.Programs != null ? src.Programs.Count : 0));

            CreateMap<DAL.Models.Aggregator.DeveloperOfAggregator, DeveloperOfAggregatorDetailDto>();
            
            CreateMap<DeveloperOfAggregatorLocalization, DeveloperOfAggregatorLocalizationDto>()
                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageOfAggregator != null ? src.LanguageOfAggregator.Code : null))
                .ForMember(dest => dest.LanguageName, opt => opt.MapFrom(src => src.LanguageOfAggregator != null ? src.LanguageOfAggregator.Title : null));

            CreateMap<DeveloperOfAggregatorCreateDto, DAL.Models.Aggregator.DeveloperOfAggregator>();
            CreateMap<DeveloperOfAggregatorUpdateDto, DAL.Models.Aggregator.DeveloperOfAggregator>()
                .ForMember(dest => dest.Localizations, opt => opt.Ignore());

            CreateMap<DeveloperOfAggregatorLocalizationDto, DeveloperOfAggregatorLocalization>();
        }
    }
}
