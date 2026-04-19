namespace pr_srv_names.Pages.Icons.Models
{
    /// <summary>
    /// Модель запроса для массового переименования файлов
    /// </summary>
    public class BulkRenameRequest
    {
        /// <summary>
        /// Список имен файлов для переименования (например, ["bold.svg", "italic.svg"])
        /// </summary>
        public List<string> FileNames { get; set; } = new();

        /// <summary>
        /// Исходная папка на диске (относительно корня иконок). Если пуста, файлы считаются загруженными.
        /// </summary>
        public string? SourceFolder { get; set; }

        /// <summary>
        /// Целевая папка для переименованных файлов (например, "actions")
        /// </summary>
        public string TargetFolder { get; set; } = string.Empty;

        /// <summary>
        /// Префикс для добавления к именам файлов (например, "av_e_")
        /// </summary>
        public string Prefix { get; set; } = string.Empty;

        /// <summary>
        /// Создавать ли папку, если она не существует (по умолчанию true)
        /// </summary>
        public bool CreateFolderIfNotExists { get; set; } = true;

        /// <summary>
        /// Перезаписывать ли существующие файлы (по умолчанию true)
        /// </summary>
        public bool OverwriteExisting { get; set; } = true;

        /// <summary>
        /// Выполнять копирование вместо перемещения (по умолчанию false - перемещение/переименование)
        /// </summary>
        public bool IsCopy { get; set; } = false;
    }

    /// <summary>
    /// Результат операции массового переименования
    /// </summary>
    public class BulkRenameResult
    {
        /// <summary>
        /// Общее количество файлов в запросе
        /// </summary>
        public int TotalFiles { get; set; }

        /// <summary>
        /// Количество успешно переименованных файлов
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// Количество файлов с ошибками
        /// </summary>
        public int FailedCount { get; set; }

        /// <summary>
        /// Детали каждой операции переименования
        /// </summary>
        public List<RenameOperation> Operations { get; set; } = new();
    }

    /// <summary>
    /// Детали одной операции переименования
    /// </summary>
    public class RenameOperation
    {
        /// <summary>
        /// Исходное имя файла
        /// </summary>
        public string OriginalName { get; set; } = string.Empty;

        /// <summary>
        /// Новое имя файла
        /// </summary>
        public string NewName { get; set; } = string.Empty;

        /// <summary>
        /// Успешно ли выполнена операция
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Сообщение об ошибке (если есть)
        /// </summary>
        public string? ErrorMessage { get; set; }
    }

    /// <summary>
    /// Представляет элемент файловой системы для браузера файлов
    /// </summary>
    public class FileSystemItem
    {
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string Type { get; set; } = "file"; // "file", "folder" or "drive"
        public string? Extension { get; set; }
        public long Size { get; set; }
    }
}
