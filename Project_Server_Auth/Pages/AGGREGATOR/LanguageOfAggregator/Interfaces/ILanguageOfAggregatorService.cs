using System.Collections.Generic;
using System.Threading.Tasks;
using pr_srv_names.Pages.Aggregator.LanguageOfAggregator.Dtos;

namespace pr_srv_names.Pages.Aggregator.LanguageOfAggregator.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса управления языками агрегатора.
    /// </summary>
    public interface ILanguageOfAggregatorService
    {
        Task<IEnumerable<LanguageOfAggregatorDto>> GetAllLanguagesAsync(bool includeDisabled = true);
        Task<IEnumerable<LanguageOfAggregatorDto>> GetAvailableLanguagesAsync();
        Task<LanguageOfAggregatorDto> GetLanguageByIdAsync(int id);
        Task<LanguageOfAggregatorDto> CreateLanguageAsync(CreateLanguageOfAggregatorDto request);
        Task<LanguageOfAggregatorDto> UpdateLanguageAsync(int id, UpdateLanguageOfAggregatorDto request);
        Task<bool> DeleteLanguageAsync(int id);
        Task<bool> SetDefaultLanguageAsync(int id);
        Task<bool> ToggleLanguageStatusAsync(int id, bool enabled);
        Task HardResetAsync();
        Task InitializeAsync();
    }
}
