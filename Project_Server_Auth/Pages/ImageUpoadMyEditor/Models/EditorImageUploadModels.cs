// Models/Editor/EditorModels.cs

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pr_srv_names.Models.Editor
{
    #region Request Models

    /// <summary>
    /// Запрос на загрузку изображения
    /// </summary>
    public class EditorImageUploadRequest
    {
        /// <summary>
        /// Имя файла изображения
        /// </summary>
        [Required(ErrorMessage = "Имя файла обязательно")]
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// MIME-тип формата файла (например, image/jpeg, image/png)
        /// </summary>
        [Required(ErrorMessage = "Формат файла обязателен")]
        public string FileFormat { get; set; } = string.Empty;

        /// <summary>
        /// Base64-кодированные данные изображения
        /// </summary>
        [Required(ErrorMessage = "Base64 данные обязательны")]
        public string Base64Data { get; set; } = string.Empty;
    }

    /// <summary>
    /// Запрос на валидацию изображения
    /// </summary>
    public class EditorImageValidationRequest
    {
        /// <summary>
        /// Base64-кодированные данные изображения
        /// </summary>
        [Required(ErrorMessage = "Base64 данные обязательны")]
        public string Base64Data { get; set; } = string.Empty;

        /// <summary>
        /// MIME-тип формата файла
        /// </summary>
        [Required(ErrorMessage = "Формат файла обязателен")]
        public string FileFormat { get; set; } = string.Empty;
    }

    /// <summary>
    /// Запрос на получение метаданных изображения
    /// </summary>
    public class EditorImageMetadataRequest
    {
        /// <summary>
        /// Base64-кодированные данные изображения
        /// </summary>
        [Required(ErrorMessage = "Base64 данные обязательны")]
        public string Base64Data { get; set; } = string.Empty;
    }

    /// <summary>
    /// Запрос на очистку старых изображений
    /// </summary>
    public class EditorImageCleanupRequest
    {
        /// <summary>
        /// Количество дней для определения старых файлов (по умолчанию 30)
        /// </summary>
        [Range(1, 365, ErrorMessage = "Количество дней должно быть от 1 до 365")]
        public int DaysOld { get; set; } = 30;
    }

    #endregion

    #region Response Models

    /// <summary>
    /// Ответ на загрузку изображения
    /// </summary>
    public class EditorImageUploadResponse
    {
        /// <summary>
        /// Успешность операции
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Полный URL загруженного изображения
        /// </summary>
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Относительный путь к изображению
        /// </summary>
        public string RelativePath { get; set; } = string.Empty;

        /// <summary>
        /// Уникальный идентификатор изображения
        /// </summary>
        public string ImageId { get; set; } = string.Empty;

        /// <summary>
        /// Размер файла в байтах
        /// </summary>
        public long FileSize { get; set; }
    }

    /// <summary>
    /// Ответ на валидацию изображения
    /// </summary>
    public class EditorValidationResponse
    {
        /// <summary>
        /// Результат валидации
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Сообщение о результате валидации
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Ширина изображения в пикселях
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Высота изображения в пикселях
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Размер файла в байтах
        /// </summary>
        public long FileSizeBytes { get; set; }
    }

    /// <summary>
    /// Ответ с метаданными изображения
    /// </summary>
    public class EditorImageMetadataResponse
    {
        /// <summary>
        /// Формат изображения (jpeg, png, gif и т.д.)
        /// </summary>
        public string Format { get; set; } = string.Empty;

        /// <summary>
        /// Ширина изображения в пикселях
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Высота изображения в пикселях
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Размер файла в байтах
        /// </summary>
        public long FileSizeBytes { get; set; }

        /// <summary>
        /// MIME-тип изображения
        /// </summary>
        public string MimeType { get; set; } = string.Empty;

        /// <summary>
        /// Является ли изображение анимированным
        /// </summary>
        public bool IsAnimated { get; set; }

        /// <summary>
        /// Длительность анимации в миллисекундах (для анимированных изображений)
        /// </summary>
        public int DurationMs { get; set; }
    }

    /// <summary>
    /// Ответ на очистку старых изображений
    /// </summary>
    public class EditorImageCleanupResponse
    {
        /// <summary>
        /// Количество удаленных файлов
        /// </summary>
        public int DeletedCount { get; set; }

        /// <summary>
        /// Освобожденное место на диске в байтах
        /// </summary>
        public long FreedSpaceBytes { get; set; }

        /// <summary>
        /// Сообщение о результате очистки
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// Конфигурация сервиса загрузки изображений
    /// </summary>
    public class EditorConfigResponse
    {
        /// <summary>
        /// Максимальный размер файла в байтах
        /// </summary>
        public long MaxFileSizeBytes { get; set; }

        /// <summary>
        /// Список поддерживаемых MIME-типов
        /// </summary>
        public List<string> SupportedFormats { get; set; } = new();

        /// <summary>
        /// Базовый URL для загрузки изображений
        /// </summary>
        public string UploadBaseUrl { get; set; } = string.Empty;

        /// <summary>
        /// Конфигурация обрезки изображений
        /// </summary>
        public EditorCropConfigResponse CropConfig { get; set; } = new();

        /// <summary>
        /// Конфигурация сжатия изображений
        /// </summary>
        public EditorCompressionConfigResponse CompressionConfig { get; set; } = new();
    }

    /// <summary>
    /// Конфигурация обрезки изображений
    /// </summary>
    public class EditorCropConfigResponse
    {
        /// <summary>
        /// Предопределенные размеры для обрезки
        /// </summary>
        public List<CropSize> PredefinedSizes { get; set; } = new();
    }

    /// <summary>
    /// Конфигурация сжатия изображений
    /// </summary>
    public class EditorCompressionConfigResponse
    {
        /// <summary>
        /// Включено ли сжатие
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Качество сжатия (1-100)
        /// </summary>
        public int Quality { get; set; }
    }

    /// <summary>
    /// Предопределенный размер для обрезки
    /// </summary>
    public class CropSize
    {
        /// <summary>
        /// Название размера
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Ширина в пикселях
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Высота в пикселях
        /// </summary>
        public int Height { get; set; }
    }

    #endregion

    #region Internal Models

    /// <summary>
    /// Результат загрузки изображения (для внутреннего использования)
    /// </summary>
    public class ImageUploadResult
    {
        /// <summary>
        /// Успешность операции
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Сообщение о результате
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Полный путь к файлу на сервере
        /// </summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// Относительный путь к файлу
        /// </summary>
        public string RelativePath { get; set; } = string.Empty;

        /// <summary>
        /// Полный URL для доступа к изображению
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Уникальный идентификатор изображения
        /// </summary>
        public string ImageId { get; set; } = string.Empty;

        /// <summary>
        /// Размер файла в байтах
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Метаданные изображения
        /// </summary>
        public ImageMetadata? Metadata { get; set; }
    }

    /// <summary>
    /// Метаданные изображения (для внутреннего использования)
    /// </summary>
    public class ImageMetadata
    {
        /// <summary>
        /// Ширина изображения в пикселях
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Высота изображения в пикселях
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Формат изображения (jpeg, png, gif и т.д.)
        /// </summary>
        public string Format { get; set; } = string.Empty;

        /// <summary>
        /// MIME-тип изображения
        /// </summary>
        public string MimeType { get; set; } = string.Empty;

        /// <summary>
        /// Является ли изображение анимированным (например, анимированный GIF)
        /// </summary>
        public bool IsAnimated { get; set; }

        /// <summary>
        /// Размер файла в байтах
        /// </summary>
        public long FileSizeBytes { get; set; }

        /// <summary>
        /// Длительность анимации в миллисекундах
        /// </summary>
        public int DurationMs { get; set; }
    }

    #endregion
}