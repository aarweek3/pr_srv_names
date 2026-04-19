using Project_Server_Auth.Pages.CategoryRepository.Dtos;

namespace Project_Server_Auth.Pages.CategoryRepository.Interfaces
{
    public interface IIconCategoryService
    {
        Task<IEnumerable<IconCategoryDto>> GetAllAsync();
        Task<IconCategoryDto?> GetByIdAsync(int id);
        Task<IconCategoryDto> CreateAsync(IconCategoryCreateDto createDto);
        Task<IconCategoryDto?> UpdateAsync(int id, IconCategoryUpdateDto updateDto);
        Task<bool> DeleteAsync(int id);
        Task<IconSyncResultDto> SyncWithFileSystemAsync();
    }
}
