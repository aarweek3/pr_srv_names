using AutoMapper;
using pr_srv_names.Pages.Language.Dtos;

namespace pr_srv_names.Pages.Language.Services
{
    public class LanguageProfile : Profile
    {
        public LanguageProfile()
        {
            // Маппинг Language -> LanguageDetailDto
            CreateMap<DAL.Models.LocalizationModels.Language, LanguageDetailDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.FlagCode, opt => opt.MapFrom(src => src.FlagCode))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            // Маппинг LanguageCreateRequestDto -> Language
            CreateMap<LanguageCreateRequestDto, DAL.Models.LocalizationModels.Language>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.FlagCode, opt => opt.MapFrom(src => src.FlagCode))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.NameDetails, opt => opt.Ignore());

            // Маппинг LanguageUpdateRequestDto -> Language
            CreateMap<LanguageUpdateRequestDto, DAL.Models.LocalizationModels.Language>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.FlagCode, opt => opt.MapFrom(src => src.FlagCode))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.NameDetails, opt => opt.Ignore());
        }
    }
}