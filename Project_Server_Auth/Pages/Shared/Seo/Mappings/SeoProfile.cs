using AutoMapper;
using DAL.Models.GeneralModels;
using pr_srv_names.Pages.Shared.Seo.Dtos;

namespace pr_srv_names.Pages.Shared.Seo.Mappings
{
    public class SeoProfile : Profile
    {
        public SeoProfile()
        {
            CreateMap<SeoData, SeoDataDto>()
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
