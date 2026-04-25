using pr_srv_names.Pages.AGGREGATOR.ProgramOfAggregator.Dtos;
using System.Threading.Tasks;

namespace pr_srv_names.Pages.AGGREGATOR.ProgramOfAggregator.Interfaces
{
    public interface IProgramOfAggregatorService
    {
        Task<ProgramOfAggregatorPagedResponseDto> GetPagedAsync(ProgramOfAggregatorPageRequestDto request);
        Task<ProgramOfAggregatorDetailDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(ProgramOfAggregatorCreateDto dto);
        Task UpdateAsync(ProgramOfAggregatorUpdateDto dto);
        Task DeleteAsync(int id);
        Task HardDeleteAsync(int id);
        Task RestoreAsync(int id);
        Task<int> SeedFromJsonAsync();
        Task<int> ClearAllAsync();

        // Version management
        Task<List<VersionOfAggregatorItemDto>> GetVersionsAsync(int programId);
        Task<VersionOfAggregatorDetailDto?> GetVersionByIdAsync(int id);
        Task<int> CreateVersionAsync(VersionOfAggregatorCreateDto dto);
        Task UpdateVersionAsync(VersionOfAggregatorUpdateDto dto);
        Task DeleteVersionAsync(int id);
    }
}
