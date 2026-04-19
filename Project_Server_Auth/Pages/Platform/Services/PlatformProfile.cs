using AutoMapper;
using DAL.Models.Business;
using pr_srv_names.Pages.Platform.Dtos;

namespace pr_srv_names.Pages.Platform.Services
{
    public class PlatformProfile : Profile
    {
        public PlatformProfile()
        {
            CreateMap<DAL.Models.Business.Platform, PlatformItemDto>();
            CreateMap<DAL.Models.Business.Platform, PlatformDetailDto>();
            
            CreateMap<PlatformTranslation, PlatformTranslationDto>()
                .ForMember(d => d.LanguageCode, o => o.MapFrom(s => s.Language != null ? s.Language.Code : null));
            
            CreateMap<PlatformCreateDto, DAL.Models.Business.Platform>()
                .ForMember(d => d.Translations, o => o.Ignore()); // Ручная синхронизация в сервисе
                
            CreateMap<PlatformUpdateDto, DAL.Models.Business.Platform>()
                .ForMember(d => d.Translations, o => o.Ignore());

            CreateMap<PlatformTranslationDto, PlatformTranslation>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.DescriptionFull, o => o.MapFrom(s => s.DescriptionFull))
                .ForMember(d => d.SeoData, o => o.Ignore()); // Синхронизируется в сервисе
        }
    }
}
