using System.Threading.Tasks;
using pr_srv_names.Pages.AGGREGATOR.DeveloperOfAggregator.Dtos;

namespace pr_srv_names.Pages.AGGREGATOR.DeveloperOfAggregator.Interfaces
{
    public interface IDeveloperOfAggregatorService
    {
        Task<DeveloperOfAggregatorPagedResponseDto> GetPagedAsync(DeveloperOfAggregatorPageRequestDto request);
        Task<DeveloperOfAggregatorDetailDto?> GetByIdAsync(int id);
        Task<DeveloperOfAggregatorDetailDto> CreateAsync(DeveloperOfAggregatorCreateDto dto);
        Task<DeveloperOfAggregatorDetailDto> UpdateAsync(DeveloperOfAggregatorUpdateDto dto);
        Task DeleteAsync(int id);
        Task HardDeleteAsync(int id);
        Task RestoreAsync(int id);
        
        Task<bool> IsNameUniqueAsync(string name, int? excludeId = null);
        Task<bool> IsSystemCodeUniqueAsync(string code, int? excludeId = null);
        
        Task<int> ClearAllAsync();
        Task<int> SeedFromJsonAsync();
    }
}
