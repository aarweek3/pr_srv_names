// Services/Editor/IEditorImageService.cs

using System.Collections.Generic;
using System.Threading.Tasks;
using pr_srv_names.Models.Editor;


namespace pr_srv_names.Services.Editor
{
    /// <summary>
    /// Интерфейс сервиса для работы с изображениями в редакторе
    /// </summary>
    public interface IEditorImageService
    {
        /// <summary>
        /// Загружает изображение из Base64 строки и сохраняет на сервер
        /// </summary>
        /// <param name="request">Запрос с данными изображения (Base64, имя файла, формат)</param>
        /// <returns>Результат загрузки с URL загруженного изображения</returns>
        Task<ImageUploadResult> UploadImageAsync(EditorImageUploadRequest request);

        /// <summary>
        /// Валидирует изображение по формату, размеру и разрешению
        /// </summary>
        /// <param name="request">Запрос с Base64 данными и форматом файла</param>
        /// <returns>Метаданные изображения, если валидация успешна</returns>
        /// <exception cref="Exception">Выбрасывается при ошибке валидации</exception>
        Task<ImageMetadata> ValidateImageAsync(EditorImageValidationRequest request);

        /// <summary>
        /// Извлекает метаданные изображения из Base64 строки
        /// </summary>
        /// <param name="request">Запрос с Base64 данными изображения</param>
        /// <returns>Метаданные изображения (размеры, формат, MIME-тип и т.д.)</returns>
        Task<ImageMetadata> GetImageMetadataAsync(EditorImageMetadataRequest request);

        /// <summary>
        /// Получает список поддерживаемых форматов изображений
        /// </summary>
        /// <returns>Список MIME-типов поддерживаемых форматов (image/jpeg, image/png и т.д.)</returns>
        Task<List<string>> GetSupportedFormatsAsync();

        /// <summary>
        /// Удаляет изображение с сервера по имени файла или пути
        /// </summary>
        /// <param name="filePath">Имя файла или путь к файлу для удаления</param>
        /// <returns>True, если файл успешно удален, иначе False</returns>
        Task<bool> DeleteImageAsync(string filePath);

        /// <summary>
        /// Очищает старые изображения, созданные более указанного количества дней назад
        /// </summary>
        /// <param name="daysOld">Количество дней (файлы старше будут удалены). По умолчанию 30 дней</param>
        /// <returns>Количество удаленных файлов</returns>
        Task<int> CleanupOldImagesAsync(int daysOld = 30);

        /// <summary>
        /// Получает конфигурацию сервиса загрузки изображений
        /// </summary>
        /// <returns>Конфигурация с лимитами размеров, поддерживаемыми форматами и настройками обрезки</returns>
        EditorConfigResponse GetConfiguration();
    }
}