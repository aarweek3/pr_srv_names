using pr_srv_names.Pages.SampleMain.Dtos;

namespace pr_srv_names.Pages.SampleMain.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с многоязычной сущностью SampleMain
    /// </summary>
    public interface ISampleMainService
    {
        Task<SampleMainPagedResponseDto> GetPagedAsync(SampleMainPageRequestDto request);
        Task<SampleMainDetailDto?> GetByIdAsync(int id);
        Task<SampleMainDetailDto> CreateAsync(SampleMainCreateRequestDto dto);
        Task<SampleMainDetailDto> UpdateAsync(SampleMainUpdateRequestDto dto);
        Task DeleteAsync(int id);
        
        // Дополнительные методы для работы с переводами, если потребуется
        Task<bool> IsNameUniqueAsync(string name, int? excludeId = null);
        Task<bool> IsSystemCodeUniqueAsync(string code, int? excludeId = null);
    }
}
