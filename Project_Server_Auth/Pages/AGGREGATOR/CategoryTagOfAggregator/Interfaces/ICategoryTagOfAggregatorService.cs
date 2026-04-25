using pr_srv_names.Pages.AGGREGATOR.CategoryTagOfAggregator.Dtos;
using System.Threading.Tasks;

namespace pr_srv_names.Pages.AGGREGATOR.CategoryTagOfAggregator.Interfaces
{
    public interface ICategoryTagOfAggregatorService
    {
        Task<CategoryTagOfAggregatorPagedResponseDto> GetPagedAsync(CategoryTagOfAggregatorPageRequestDto request);
        Task<CategoryTagOfAggregatorDetailDto?> GetByIdAsync(int id);
        Task<CategoryTagOfAggregatorDetailDto> CreateAsync(CategoryTagOfAggregatorCreateDto dto);
        Task<CategoryTagOfAggregatorDetailDto> UpdateAsync(CategoryTagOfAggregatorUpdateDto dto);
        Task DeleteAsync(int id);
        Task RestoreAsync(int id);
        Task<int> SeedFromJsonAsync();
        Task<int> ClearAllAsync();
    }
}
