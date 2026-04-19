using AutoMapper;
using pr_srv_names.Pages.Sample.Dtos;

namespace pr_srv_names.Pages.Sample.Services
{
    public class SampleProfile : Profile
    {
        public SampleProfile()
        {
            // Маппинг Sample -> SampleDetailDto (один маппинг вместо двух дублирующихся)
            CreateMap<DAL.Models.SampleModels.Sample, SampleDetailDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            // Маппинг SampleCreateRequestDto -> Sample
            CreateMap<SampleCreateRequestDto, DAL.Models.SampleModels.Sample>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Маппинг SampleUpdateRequestDto -> Sample
            CreateMap<SampleUpdateRequestDto, DAL.Models.SampleModels.Sample>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}