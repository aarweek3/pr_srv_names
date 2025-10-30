using pr_srv_names.Pages.AdvancedImageEditor.Models;

namespace pr_srv_names.Pages.AdvancedImageEditor.Services
{
    public interface IAdvancedImageProcessingService
    {
        // Обработка изображений (заглушки, так как вся логика на клиенте)
        Task<AdvancedImageProcessResult> ProcessImageAsync(AdvancedImageProcessRequest request);
        Task<ImageInfoResult> GetImageInfoAsync(ImageInfoRequest request);
        Task<ProcessingCapabilitiesResponse> GetProcessingCapabilitiesAsync();

        // Хранение изображений
        Task<SaveImageResult> SaveImageAsync(SaveImageRequest request);
        Task<LoadImageResult> LoadImageAsync(string imageId);
        Task<bool> DeleteImageAsync(string imageId);
        Task<List<string>> GetImageListAsync();
    }
}