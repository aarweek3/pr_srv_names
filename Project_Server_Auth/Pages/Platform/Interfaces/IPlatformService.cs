using pr_srv_names.Pages.Platform.Dtos;
using System;
using System.Threading.Tasks;

namespace pr_srv_names.Pages.Platform.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с платформами (многоязычность + SEO)
    /// </summary>
    public interface IPlatformService
    {
        Task<PlatformPagedResponseDto> GetPagedAsync(PlatformPageRequestDto request);
        Task<PlatformDetailDto?> GetByIdAsync(Guid id);
        Task<PlatformDetailDto> CreateAsync(PlatformCreateDto dto);
        Task<PlatformDetailDto> UpdateAsync(PlatformUpdateDto dto);
        Task DeleteAsync(Guid id);
        
        Task<bool> IsCodeUniqueAsync(string code, Guid? excludeId = null);
    }
}
