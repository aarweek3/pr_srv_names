using AutoMapper;
using DAL.Models.SampleModels;
using pr_srv_names.Pages.SampleMain.Dtos;

namespace pr_srv_names.Pages.SampleMain.Services
{
    public class SampleMainProfile : Profile
    {
        public SampleMainProfile()
        {
            // Маппинг SampleMain -> SampleMainItemDto
            CreateMap<DAL.Models.SampleModels.SampleMain, SampleMainItemDto>()
                .ForMember(dest => dest.LocalizedName, opt => opt.MapFrom(src => 
                    src.Descriptions.Select(d => d.Name).FirstOrDefault())); // Базовый маппинг, уточняется в сервисе

            // Маппинг SampleMain -> SampleMainDetailDto
            CreateMap<DAL.Models.SampleModels.SampleMain, SampleMainDetailDto>();

            // Маппинг SampleMainDescription -> SampleMainDescriptionDto
            CreateMap<SampleMainDescription, SampleMainDescriptionDto>()
                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageApp.Code));

            // Маппинг SampleMainCreateRequestDto -> SampleMain
            CreateMap<SampleMainCreateRequestDto, DAL.Models.SampleModels.SampleMain>()
                .ForMember(dest => dest.Descriptions, opt => opt.MapFrom(src => src.Descriptions))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            // Маппинг SampleMainUpdateRequestDto -> SampleMain
            CreateMap<SampleMainUpdateRequestDto, DAL.Models.SampleModels.SampleMain>()
                .ForMember(dest => dest.Descriptions, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            // Маппинг SampleMainDescriptionDto -> SampleMainDescription
            CreateMap<SampleMainDescriptionDto, SampleMainDescription>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SampleMainId, opt => opt.Ignore())
                .ForMember(dest => dest.SampleMain, opt => opt.Ignore())
                .ForMember(dest => dest.LanguageApp, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
}
