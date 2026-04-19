using pr_srv_names.Pages.Icons.Models;

namespace pr_srv_names.Pages.Icons.Interfaces
{
    public interface IIconService
    {
        Task<List<IconCategoryPage>> GetIconsAsync(bool includeSvgContent = false);
        Task<List<IconMetadata>> GetAllIconsFlatAsync();
        Task<int> GetTotalIconsCountAsync();
        Task SyncToFrontendAsync(string filePath);
        Task UpdateIconContentAsync(string iconType, string svgContent, bool toBackend = true, bool toFrontend = true);
        Task UpdateIconsBatchAsync(List<UpdateIconRequest> requests);
        Task DeleteIconAsync(string iconType, bool fromBackend, bool fromFrontend);
        Task<List<IconSyncStatus>> GetSyncStatusAsync();
        Task<List<IconRenameResult>> RefactorIconNamesAsync(int? categoryId = null);
        Task MoveIconAsync(string iconType, int targetCategoryId);
        Task RenameIconAsync(string oldName, string newName);
        Task<BulkRenameResult> BulkRenameAsync(BulkRenameRequest request);
        Task<List<FileSystemItem>> BrowseFileSystemAsync(string path);

        Task<string?> GetContentByNameAsync(string name);
        Task<List<IconMetadata>> GetIconsByCategoryAsync(int categoryId);
        Task<Dictionary<string, string>> GetIconsContentBatchAsync(List<string> itemNames);
    }

    public class IconSyncStatus
    {
        public string IconType { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public bool ExistsOnBackend { get; set; }
        public bool ExistsOnFrontend { get; set; }
        public bool IsInRegistry { get; set; }
        public bool ContentMatches { get; set; }
    }

    public class IconRenameResult
    {
        public string OldName { get; set; }
        public string NewName { get; set; }
        public string Category { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Location { get; set; }
    }
}
