using AutoMapper;
using DAL.Models.SampleModels;
using pr_srv_names.Pages.SampleMainSeo.Dtos;
using System.Linq;

namespace pr_srv_names.Pages.SampleMainSeo.Services
{
    public class SampleMainSeoProfile : Profile
    {
        public SampleMainSeoProfile()
        {
            // Маппинг SampleMainSeo -> SampleMainSeoItemDto
            CreateMap<DAL.Models.SampleModels.SampleMainSeo, SampleMainSeoItemDto>()
                .ForMember(dest => dest.LocalizedName, opt => opt.Ignore()) // Вычисляется в сервисе
                .ForMember(dest => dest.SeoScore, opt => opt.Ignore()); // Вычисляется в сервисе

            // Маппинг SampleMainSeo -> SampleMainSeoDetailDto
            CreateMap<DAL.Models.SampleModels.SampleMainSeo, SampleMainSeoDetailDto>();

            // Маппинг SampleMainDescriptionSeo -> SampleMainDescriptionSeoDto
            CreateMap<SampleMainDescriptionSeo, SampleMainDescriptionSeoDto>()
                .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.LanguageApp.Code));

            // Маппинг SampleMainSeoCreateDto -> SampleMainSeo
            CreateMap<SampleMainSeoCreateDto, DAL.Models.SampleModels.SampleMainSeo>()
                .ForMember(dest => dest.Descriptions, opt => opt.MapFrom(src => src.Descriptions))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            // Маппинг SampleMainSeoUpdateDto -> SampleMainSeo
            CreateMap<SampleMainSeoUpdateDto, DAL.Models.SampleModels.SampleMainSeo>()
                .ForMember(dest => dest.Descriptions, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            // Маппинг SampleMainDescriptionSeoDto -> SampleMainDescriptionSeo
            CreateMap<SampleMainDescriptionSeoDto, SampleMainDescriptionSeo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.SampleMainSeoId, opt => opt.Ignore())
                .ForMember(dest => dest.SampleMainSeo, opt => opt.Ignore())
                .ForMember(dest => dest.LanguageApp, opt => opt.Ignore())
                .ForMember(dest => dest.SeoDataId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
}
