using pr_srv_names.Pages.SampleMainSeo.Dtos;
using System.Threading.Tasks;

namespace pr_srv_names.Pages.SampleMainSeo.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с SampleMainSeo (Образцовая модель с универсальным SEO)
    /// </summary>
    public interface ISampleMainSeoService
    {
        Task<SampleMainSeoPagedResponseDto> GetPagedAsync(SampleMainSeoPageRequestDto request);
        Task<SampleMainSeoDetailDto?> GetByIdAsync(int id);
        Task<SampleMainSeoDetailDto> CreateAsync(SampleMainSeoCreateDto dto);
        Task<SampleMainSeoDetailDto> UpdateAsync(SampleMainSeoUpdateDto dto);
        Task DeleteAsync(int id);
        
        Task<bool> IsNameUniqueAsync(string name, int? excludeId = null);
        Task<bool> IsSystemCodeUniqueAsync(string code, int? excludeId = null);
    }
}
