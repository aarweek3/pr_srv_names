using pr_srv_names.Pages.AGGREGATOR.TagOfAggregator.Dtos;
using System.Threading.Tasks;

namespace pr_srv_names.Pages.AGGREGATOR.TagOfAggregator.Interfaces
{
    public interface ITagOfAggregatorService
    {
        Task<TagOfAggregatorPagedResponseDto> GetPagedAsync(TagOfAggregatorPageRequestDto request);
        Task<TagOfAggregatorDetailDto?> GetByIdAsync(int id);
        Task<TagOfAggregatorDetailDto> CreateAsync(TagOfAggregatorCreateDto dto);
        Task<TagOfAggregatorDetailDto> UpdateAsync(TagOfAggregatorUpdateDto dto);
        Task DeleteAsync(int id);
        Task RestoreAsync(int id);
        Task<int> UpdateSortOrderAsync(int id, int newSortOrder);
        Task<int> SeedFromJsonAsync();
        Task<int> ClearAllAsync();
    }
}
