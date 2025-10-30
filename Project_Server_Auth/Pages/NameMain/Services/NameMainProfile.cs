using AutoMapper;
using pr_srv_names.Pages.NameMain.Dtos;

namespace pr_srv_names.Pages.NameMain.Services
{
    public class NameMainProfile : Profile
    {
        public NameMainProfile()
        {
            // Маппинг NameMain -> NameMainDetailDto (один маппинг вместо двух дублирующихся)
            CreateMap<DAL.Models.NameMain, NameMainDetailDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            // Маппинг NameMainCreateRequestDto -> NameMain
            CreateMap<NameMainCreateRequestDto, DAL.Models.NameMain>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Маппинг NameMainUpdateRequestDto -> NameMain
            CreateMap<NameMainUpdateRequestDto, DAL.Models.NameMain>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}