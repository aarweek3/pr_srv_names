using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using DAL.Models.Aggregator.Base;
using DAL.Models.Aggregator.Enums;
using DAL.Models.Aggregator.Localizations;

namespace DAL.Models.Aggregator
{
    // Сущность "Программа" для Агрегатора
    [Table("programs_of_aggregator")]
    [Index(nameof(Slug), IsUnique = true)]
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

        // Порядок сортировки в каталоге
        public int SortOrder { get; set; } = 0;

        // Флаг необходимости ручной проверки
        public bool NeedsReview { get; set; } = false;

        // Флаг активности в системе (базовая доступность)
        public bool IsActive { get; set; } = true;

        // Защита от удаления (системная программа)
        public bool IsSystem { get; set; } = false;

        // Статус публикации программы
        public ProgramStatus Status { get; set; } = ProgramStatus.Draft;

        /// <summary>
        /// Суммарное количество загрузок (агрегат из MarketData).
        /// Актуализируется через UpdateAggregatedStats() или БД-триггер.
        /// </summary>
        public long? TotalDownloads { get; set; }

        /// <summary>
        /// Средний рейтинг программы (агрегат из MarketData).
        /// Актуализируется через UpdateAggregatedStats() или БД-триггер.
        /// </summary>
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

        // История снапшотов (цена, рейтинг, скачивания)
        public virtual ICollection<ProgramSnapshotOfAggregator> Snapshots { get; set; }
            = new List<ProgramSnapshotOfAggregator>();

        // Редиректы для старых URL
        public virtual ICollection<ProgramSlugRedirectOfAggregator> SlugRedirects { get; set; }
            = new List<ProgramSlugRedirectOfAggregator>();

        // История версий программы
        public virtual ICollection<VersionOfAggregator> Versions { get; set; }
            = new List<VersionOfAggregator>();

        #region Domain Methods

        /// <summary>
        /// Пересчитывает агрегированные показатели (Downloads, Rating) на основе связанных коллекций.
        /// Должен вызываться после изменения MarketData.
        /// </summary>
        public void UpdateAggregatedStats()
        {
            if (MarketData == null || !MarketData.Any())
            {
                TotalDownloads = null;
                AverageRating = null;
                RatingCount = null;
                return;
            }

            TotalDownloads = MarketData.Sum(m => m.DownloadCountExact ?? 0);
            
            var ratedEntries = MarketData.Where(m => m.RatingNormalized.HasValue).ToList();
            if (ratedEntries.Any())
            {
                AverageRating = ratedEntries.Average(m => m.RatingNormalized!.Value);
                RatingCount = ratedEntries.Count;
            }
            else
            {
                AverageRating = null;
                RatingCount = null;
            }
        }

        #endregion
    }
}
