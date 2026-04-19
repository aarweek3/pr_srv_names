using AutoMapper;
using pr_srv_names.Pages.LanguageApp.Dtos;

namespace pr_srv_names.Pages.LanguageApp.Services
{
    public class LanguageAppProfile : Profile
    {
        public LanguageAppProfile()
        {
            // LanguageApp -> LanguageAppDto
            CreateMap<DAL.Models.LocalizationModels.LanguageApp, LanguageAppDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.ShortCode, opt => opt.MapFrom(src => src.ShortCode))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.NativeTitle, opt => opt.MapFrom(src => src.NativeTitle))
                .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => src.Enabled))
                .ForMember(dest => dest.Direction, opt => opt.MapFrom(src => src.Direction))
                .ForMember(dest => dest.SortOrder, opt => opt.MapFrom(src => src.SortOrder))
                .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.IsDefault))
                .ForMember(dest => dest.IsSystem, opt => opt.MapFrom(src => src.IsSystem))
                .ForMember(dest => dest.IconKey, opt => opt.MapFrom(src => src.IconKey));

            // CreateLanguageAppDto -> LanguageApp
            CreateMap<CreateLanguageAppDto, DAL.Models.LocalizationModels.LanguageApp>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.ShortCode, opt => opt.MapFrom(src => src.ShortCode))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.NativeTitle, opt => opt.MapFrom(src => src.NativeTitle))
                .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => src.Enabled))
                .ForMember(dest => dest.Direction, opt => opt.MapFrom(src => src.Direction))
                .ForMember(dest => dest.SortOrder, opt => opt.MapFrom(src => src.SortOrder))
                .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.IsDefault))
                .ForMember(dest => dest.IsSystem, opt => opt.MapFrom(src => src.IsSystem))
                .ForMember(dest => dest.IconKey, opt => opt.MapFrom(src => src.IconKey))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            // UpdateLanguageAppDto -> LanguageApp
            CreateMap<UpdateLanguageAppDto, DAL.Models.LocalizationModels.LanguageApp>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.ShortCode, opt => opt.MapFrom(src => src.ShortCode))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.NativeTitle, opt => opt.MapFrom(src => src.NativeTitle))
                .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => src.Enabled))
                .ForMember(dest => dest.Direction, opt => opt.MapFrom(src => src.Direction))
                .ForMember(dest => dest.SortOrder, opt => opt.MapFrom(src => src.SortOrder))
                .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.IsDefault))
                .ForMember(dest => dest.IsSystem, opt => opt.MapFrom(src => src.IsSystem))
                .ForMember(dest => dest.IconKey, opt => opt.MapFrom(src => src.IconKey))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
