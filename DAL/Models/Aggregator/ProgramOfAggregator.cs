using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Models.Aggregator.Base;
using DAL.Models.Aggregator.Localizations;

/* Сущность PlatformOfAggregator, которая находится в DAL\Models\Aggregator\PlatformOfAggregator.cs.
   Она специально предназначена для хранения списка ОС: Windows, macOS, Linux, Android, iOS и других. 
   Константы (Системные коды)
   В проекте уже определены стандартные коды для платформ в файле DAL\Constants\PlatformConstants.cs: 
   windows, macos, linux, android, ios, web */

namespace DAL.Models.Aggregator
{
    // Сущность "Программа" для Агрегатора
    [Table("programs_of_aggregator")]
    public class ProgramOfAggregator : FullAuditableEntityOfAggregator
    {
        // Каноническое (системное) название программы
        [Required, MaxLength(255)]
        public string CanonicalName { get; set; } = string.Empty;

        // ЧПУ (URL-префикс) для программы
        [Required, MaxLength(100)]
        public string Slug { get; set; } = string.Empty;

        // ID основной категории
        public int CategoryOfAggregatorId { get; set; }
        // Навигационное свойство: Категория
        public virtual CategoryOfAggregator CategoryOfAggregator { get; set; } = null!;

        // ID подкатегории (необязательно)
        public int? SubCategoryOfAggregatorId { get; set; }
        // Навигационное свойство: Подкатегория
        public virtual CategoryOfAggregator? SubCategoryOfAggregator { get; set; }

        // ID разработчика
        public int? DeveloperOfAggregatorId { get; set; }
        // Навигационное свойство: Разработчик
        public virtual DeveloperOfAggregator? DeveloperOfAggregator { get; set; }

        // Путь к иконке программы
        public string? IconPath { get; set; }
        // Флаг активности в системе
        public bool IsActive { get; set; } = true;

        // Общее количество загрузок
        public long? TotalDownloads { get; set; }
        // Средний рейтинг программы
        public double? AverageRating { get; set; }
        // Количество проголосовавших
        public int? RatingCount { get; set; }

        // Коллекция локализаций (названия, описания на разных языках)
        public virtual ICollection<ProgramOfAggregatorLocalization> Localizations { get; set; }
            = new List<ProgramOfAggregatorLocalization>();

        // Список поддерживаемых платформ и ссылок на скачивание
        public virtual ICollection<ProgramPlatformOfAggregator> ProgramPlatforms { get; set; }
            = new List<ProgramPlatformOfAggregator>();

        // Маркетинговые и SEO данные программы
        public virtual ICollection<ProgramMarketDataOfAggregator> MarketData { get; set; }
            = new List<ProgramMarketDataOfAggregator>();

        // Скриншоты программы
        public virtual ICollection<ScreenshotOfAggregator> Screenshots { get; set; }
            = new List<ScreenshotOfAggregator>();

        // Теги, связанные с программой
        public virtual ICollection<ProgramTagOfAggregator> ProgramTags { get; set; }
            = new List<ProgramTagOfAggregator>();

        // Видеоролики (YouTube и др.)
        public virtual ICollection<VideoOfAggregator> Videos { get; set; }
            = new List<VideoOfAggregator>();

        // История версий программы
        public virtual ICollection<VersionOfAggregator> Versions { get; set; }
            = new List<VersionOfAggregator>();
    }
}
