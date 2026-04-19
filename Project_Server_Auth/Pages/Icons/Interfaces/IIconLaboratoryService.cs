using pr_srv_names.Pages.Icons.Models;

namespace pr_srv_names.Pages.Icons.Interfaces
{
    public interface IIconLaboratoryService
    {
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
        Task SaveIconToDiskAsync(SaveIconToDiskRequest request);
        Task CreateDirectoryAsync(string path);
        Task OpenFileInEditorAsync(string path);
    }
}
