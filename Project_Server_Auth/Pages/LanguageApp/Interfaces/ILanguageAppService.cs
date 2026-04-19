using pr_srv_names.Pages.LanguageApp.Dtos;

namespace pr_srv_names.Pages.LanguageApp.Interfaces
{
    public interface ILanguageAppService
    {
        Task<IEnumerable<LanguageAppDto>> GetAllLanguagesAsync(bool includeDisabled = true);
        Task<IEnumerable<LanguageAppDto>> GetAvailableLanguagesAsync();
        Task<LanguageAppDto> GetLanguageByIdAsync(int id);
        Task<LanguageAppDto> CreateLanguageAsync(CreateLanguageAppDto request);
        Task<LanguageAppDto> UpdateLanguageAsync(int id, UpdateLanguageAppDto request);
        Task<bool> DeleteLanguageAsync(int id);
        Task<bool> SetDefaultLanguageAsync(int id);
        Task<bool> ToggleLanguageStatusAsync(int id, bool enabled);
        Task HardResetAsync();
        Task InitializeAsync();
    }
}
