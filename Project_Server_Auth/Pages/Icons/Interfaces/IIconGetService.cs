using pr_srv_names.Pages.Icons.Models;

namespace pr_srv_names.Pages.Icons.Interfaces
{
    public interface IIconGetService
    {
        Task<List<IconCategoryPage>> GetIconsAsync(bool includeSvgContent = false);
        Task<List<IconMetadata>> GetAllIconsFlatAsync();
        Task<int> GetTotalIconsCountAsync();
        Task<string?> GetContentByNameAsync(string name);
        Task<List<IconMetadata>> GetIconsByCategoryAsync(int categoryId);
        Task<Dictionary<string, string>> GetIconsContentBatchAsync(List<string> itemNames);
    }
}
