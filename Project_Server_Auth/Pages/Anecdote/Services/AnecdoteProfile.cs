using AutoMapper;
using pr_srv_names.Pages.Anecdote.Dtos;

namespace pr_srv_names.Pages.Anecdote.Services
{
    public class AnecdoteProfile : Profile
    {
        public AnecdoteProfile()
        {
            // Маппинг Anecdote -> AnecdoteDetailDto
            CreateMap<DAL.Models.NameModels.Anecdote, AnecdoteDetailDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.NameMainId, opt => opt.MapFrom(src => src.NameMainId))
                .ForMember(dest => dest.NameMainName,
                    opt => opt.MapFrom(src => src.NameMain != null ? src.NameMain.Name : null))
                .ForMember(dest => dest.LanguageId, opt => opt.MapFrom(src => src.LanguageId))
                .ForMember(dest => dest.LanguageName,
                    opt => opt.MapFrom(src => src.Language != null ? src.Language.Name : null))
                .ForMember(dest => dest.LanguageCode,
                    opt => opt.MapFrom(src => src.Language != null ? src.Language.Code : null))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            // Маппинг AnecdoteCreateRequestDto -> Anecdote
            CreateMap<AnecdoteCreateRequestDto, DAL.Models.NameModels.Anecdote>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.NameMainId, opt => opt.MapFrom(src => src.NameMainId))
                .ForMember(dest => dest.LanguageId, opt => opt.MapFrom(src => src.LanguageId))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive)) // Добавлен маппинг IsActive
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.NameMain, opt => opt.Ignore())
                .ForMember(dest => dest.Language, opt => opt.Ignore());

            // Маппинг AnecdoteUpdateRequestDto -> Anecdote
            CreateMap<AnecdoteUpdateRequestDto, DAL.Models.NameModels.Anecdote>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.NameMainId, opt => opt.MapFrom(src => src.NameMainId))
                .ForMember(dest => dest.LanguageId, opt => opt.MapFrom(src => src.LanguageId))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive)) // Добавлен маппинг IsActive
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.NameMain, opt => opt.Ignore())
                .ForMember(dest => dest.Language, opt => opt.Ignore());
        }
    }
}