using System.Collections.Generic;
using System.Threading.Tasks;
using pr_srv_names.Pages.AGGREGATOR.CategoryOfAggregator.Dtos;

namespace pr_srv_names.Pages.AGGREGATOR.CategoryOfAggregator.Interfaces
{
    public interface ICategoryOfAggregatorService
    {
        Task<CategoryOfAggregatorPagedResponseDto> GetPagedAsync(CategoryOfAggregatorPageRequestDto request);
        Task<CategoryOfAggregatorDetailDto> GetByIdAsync(int id);
        Task<CategoryOfAggregatorDetailDto> CreateAsync(CategoryOfAggregatorCreateDto request);
        Task<CategoryOfAggregatorDetailDto> UpdateAsync(CategoryOfAggregatorUpdateDto request);
        Task<bool> DeleteAsync(int id);
        Task<bool> HardDeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
        Task<int> SeedFromJsonAsync();
        Task<bool> ClearAllAsync();
        Task<List<CategoryOfAggregatorItemDto>> GetTreeAsync(int? languageId = null);
    }
}
