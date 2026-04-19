using AutoMapper;
using LanguageOfAggregatorEntity = DAL.Models.Aggregator.LanguageOfAggregator;
using pr_srv_names.Pages.Aggregator.LanguageOfAggregator.Dtos;

namespace pr_srv_names.Pages.Aggregator.LanguageOfAggregator.Services
{
    /// <summary>
    /// Профиль маппинга для языков агрегатора.
    /// </summary>
    public class LanguageOfAggregatorProfile : Profile
    {
        public LanguageOfAggregatorProfile()
        {
            // Entity -> DTO
            CreateMap<LanguageOfAggregatorEntity, LanguageOfAggregatorDto>();

            // CreateDTO -> Entity
            CreateMap<CreateLanguageOfAggregatorDto, LanguageOfAggregatorEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            // UpdateDTO -> Entity
            CreateMap<UpdateLanguageOfAggregatorDto, LanguageOfAggregatorEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
