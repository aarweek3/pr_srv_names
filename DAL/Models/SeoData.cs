using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// Модель для хранения ВСЕХ SEO-данных, которая теперь является зависимой сущностью
    /// в 1:1 связи с родителями (Baba, Alba) и связана с NameMain и Language.
    /// УЛУЧШЕНО: добавлены ограничения длины, аудит поля, валидация и методы
    /// </summary>
    public class SeoData
    {
        [Key] public int Id { get; set; }

        // =================================================================
        // СВЯЗИ С NameMain и Language
        // =================================================================

        /// <summary>
        /// Внешний ключ на таблицу Names
        /// </summary>
        [Required]
        [ForeignKey("NameMain")]
        public int NameMainId { get; set; }

        /// <summary>
        /// Навигационное свойство - ссылка на основное имя
        /// </summary>
        public virtual NameMain NameMain { get; set; } = null!;

        /// <summary>
        /// Внешний ключ на таблицу Languages
        /// </summary>
        [Required]
        [ForeignKey("Language")]
        public int LanguageId { get; set; }

        /// <summary>
        /// Навигационное свойство - ссылка на язык
        /// </summary>
        public virtual Language Language { get; set; } = null!;

        // =================================================================
        // ОСНОВНЫЕ META ТЕГИ
        // =================================================================

        /// <summary>
        /// SEO заголовок (рекомендуется до 70 символов)
        /// </summary>
        [MaxLength(70)]
        public string? MetaTitle { get; set; }

        /// <summary>
        /// META описание (рекомендуется до 160 символов)
        /// </summary>
        [MaxLength(160)]
        public string? MetaDescription { get; set; }

        /// <summary>
        /// META ключевые слова (устаревшее, но иногда используется)
        /// </summary>
        [MaxLength(200)]
        public string? MetaKeywords { get; set; }

        /// <summary>
        /// ЧПУ URL (человеко-понятный URL)
        /// </summary>
        [MaxLength(200)]
        public string? UrlSlug { get; set; }

        /// <summary>
        /// Каноническая ссылка
        /// </summary>
        [MaxLength(300)]
        public string? CanonicalUrl { get; set; }

        // =================================================================
        // OPEN GRAPH ТЕГИ
        // =================================================================

        /// <summary>
        /// Open Graph заголовок
        /// </summary>
        [MaxLength(100)]
        public string? OgTitle { get; set; }

        /// <summary>
        /// Open Graph описание
        /// </summary>
        [MaxLength(200)]
        public string? OgDescription { get; set; }

        /// <summary>
        /// Open Graph изображение
        /// </summary>
        [MaxLength(500)]
        public string? OgImage { get; set; }

        /// <summary>
        /// Open Graph тип контента
        /// </summary>
        [MaxLength(50)]
        public string? OgType { get; set; } = "article";

        /// <summary>
        /// Open Graph URL
        /// </summary>
        [MaxLength(300)]
        public string? OgUrl { get; set; }

        // =================================================================
        // TWITTER CARD ТЕГИ
        // =================================================================

        /// <summary>
        /// Twitter Card тип
        /// </summary>
        [MaxLength(100)]
        public string? TwitterCard { get; set; } = "summary_large_image";

        /// <summary>
        /// Twitter заголовок
        /// </summary>
        [MaxLength(100)]
        public string? TwitterTitle { get; set; }

        /// <summary>
        /// Twitter описание
        /// </summary>
        [MaxLength(200)]
        public string? TwitterDescription { get; set; }

        /// <summary>
        /// Twitter изображение
        /// </summary>
        [MaxLength(500)]
        public string? TwitterImage { get; set; }

        // =================================================================
        // ИЗОБРАЖЕНИЯ И ALT ТЕГИ
        // =================================================================

        [MaxLength(500)] public string? ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Alt текст изображения (рекомендуется до 125 символов)
        /// </summary>
        [MaxLength(125)]
        public string? ImageAltText { get; set; }

        [MaxLength(200)] public string? ImageCaption { get; set; }

        // =================================================================
        // STRUCTURED DATA (SCHEMA.ORG)
        // =================================================================

        /// <summary>
        /// Тип схемы Schema.org (Article, NewsArticle, BlogPosting, etc.)
        /// </summary>
        [MaxLength(50)]
        public string? SchemaType { get; set; }

        /// <summary>
        /// JSON-LD разметка для поисковых систем
        /// </summary>
        [Column(TypeName = "jsonb")]
        public string? SchemaJsonLd { get; set; }

        // =================================================================
        // АВТОР И ПУБЛИКАЦИЯ
        // =================================================================

        [MaxLength(100)] public string? AuthorName { get; set; } = string.Empty;

        [MaxLength(100)] public string? PublisherName { get; set; } = string.Empty;

        public DateTime? PublishedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [MaxLength(100)] public string? ArticleSection { get; set; } = string.Empty;

        // =================================================================
        // ДОПОЛНИТЕЛЬНЫЕ SEO ПОЛЯ
        // =================================================================

        /// <summary>
        /// Запрет индексации страницы (robots noindex)
        /// </summary>
        public bool NoIndex { get; set; } = false;

        /// <summary>
        /// Запрет следования по ссылкам (robots nofollow)
        /// </summary>
        public bool NoFollow { get; set; } = false;

        /// <summary>
        /// Приоритет страницы для sitemap.xml (0-10)
        /// </summary>
        [Range(0, 10)]
        public int? Priority { get; set; } = 5;

        /// <summary>
        /// Географический регион
        /// </summary>
        [MaxLength(100)]
        public string? Region { get; set; }

        // =================================================================
        // АУДИТ ПОЛЯ
        // =================================================================

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        [MaxLength(450)] public string? CreatedBy { get; set; }

        [MaxLength(450)] public string? UpdatedBy { get; set; }

        // =================================================================
        // COMPUTED PROPERTIES
        // =================================================================

        /// <summary>
        /// Содержимое robots meta тега
        /// </summary>
        [NotMapped]
        public string RobotsContent => GetRobotsContent();

        /// <summary>
        /// Проверяет, оптимизирована ли страница для SEO
        /// </summary>
        [NotMapped]
        public bool IsOptimized =>
            !string.IsNullOrEmpty(MetaTitle) &&
            !string.IsNullOrEmpty(MetaDescription) &&
            !string.IsNullOrEmpty(UrlSlug);

        /// <summary>
        /// Длина SEO заголовка
        /// </summary>
        [NotMapped]
        public int TitleLength => MetaTitle?.Length ?? 0;

        /// <summary>
        /// Длина SEO описания
        /// </summary>
        [NotMapped]
        public int DescriptionLength => MetaDescription?.Length ?? 0;

        /// <summary>
        /// Проверяет, настроены ли Open Graph теги
        /// </summary>
        [NotMapped]
        public bool HasOpenGraph =>
            !string.IsNullOrEmpty(OgTitle) &&
            !string.IsNullOrEmpty(OgDescription);

        /// <summary>
        /// Проверяет, настроены ли Twitter Card теги
        /// </summary>
        [NotMapped]
        public bool HasTwitterCard =>
            !string.IsNullOrEmpty(TwitterTitle) &&
            !string.IsNullOrEmpty(TwitterDescription);

        /// <summary>
        /// Проверяет, настроена ли структурированная разметка
        /// </summary>
        [NotMapped]
        public bool HasStructuredData => !string.IsNullOrEmpty(SchemaJsonLd);

        /// <summary>
        /// Процент SEO оптимизации (0-100)
        /// </summary>
        [NotMapped]
        public int OptimizationScore => CalculateOptimizationScore();

        // =================================================================
        // МЕТОДЫ
        // =================================================================

        private string GetRobotsContent()
        {
            var robots = new List<string>();

            if (NoIndex) robots.Add("noindex");
            else robots.Add("index");

            if (NoFollow) robots.Add("nofollow");
            else robots.Add("follow");

            return string.Join(", ", robots);
        }

        private int CalculateOptimizationScore()
        {
            int score = 0;
            int maxScore = 10;

            // Основные SEO элементы (60%)
            if (!string.IsNullOrEmpty(MetaTitle)) score += 2;
            if (!string.IsNullOrEmpty(MetaDescription)) score += 2;
            if (!string.IsNullOrEmpty(UrlSlug)) score += 2;

            // Open Graph (20%)
            if (HasOpenGraph) score += 2;

            // Twitter Card (10%)
            if (HasTwitterCard) score += 1;

            // Структурированные данные (10%)
            if (HasStructuredData) score += 1;

            return (int)Math.Round((double)score / maxScore * 100);
        }

        /// <summary>
        /// Валидация SEO данных
        /// </summary>
        public List<string> ValidateSeoData()
        {
            var errors = new List<string>();

            // Проверка длины заголовка
            if (TitleLength > 70)
                errors.Add($"SEO заголовок слишком длинный ({TitleLength}/70 символов)");
            else if (TitleLength < 30)
                errors.Add($"SEO заголовок слишком короткий ({TitleLength}/30+ символов)");

            // Проверка длины описания
            if (DescriptionLength > 160)
                errors.Add($"META описание слишком длинное ({DescriptionLength}/160 символов)");
            else if (DescriptionLength < 120)
                errors.Add($"META описание слишком короткое ({DescriptionLength}/120+ символов)");

            // Проверка URL slug
            if (!string.IsNullOrEmpty(UrlSlug))
            {
                if (UrlSlug.Contains(" "))
                    errors.Add("URL slug не должен содержать пробелы");

                if (UrlSlug.Length > 200)
                    errors.Add("URL slug слишком длинный");
            }

            // Проверка Alt текста
            if (!string.IsNullOrEmpty(ImageAltText) && ImageAltText.Length > 125)
                errors.Add("Alt текст изображения слишком длинный");

            return errors;
        }

        /// <summary>
        /// Генерирует URL slug из заголовка
        /// </summary>
        public void GenerateSlugFromTitle()
        {
            if (string.IsNullOrEmpty(MetaTitle))
                return;

            UrlSlug = MetaTitle.ToLowerInvariant()
                .Replace(" ", "-")
                .Replace("_", "-")
                .Trim('-');

            MarkAsUpdated();
        }

        /// <summary>
        /// Автозаполнение Open Graph из основных полей
        /// </summary>
        public void FillOpenGraphFromMeta()
        {
            if (string.IsNullOrEmpty(OgTitle) && !string.IsNullOrEmpty(MetaTitle))
                OgTitle = MetaTitle;

            if (string.IsNullOrEmpty(OgDescription) && !string.IsNullOrEmpty(MetaDescription))
                OgDescription = MetaDescription;

            MarkAsUpdated();
        }

        /// <summary>
        /// Автозаполнение Twitter Card из Open Graph
        /// </summary>
        public void FillTwitterFromOpenGraph()
        {
            if (string.IsNullOrEmpty(TwitterTitle) && !string.IsNullOrEmpty(OgTitle))
                TwitterTitle = OgTitle;

            if (string.IsNullOrEmpty(TwitterDescription) && !string.IsNullOrEmpty(OgDescription))
                TwitterDescription = OgDescription;

            if (string.IsNullOrEmpty(TwitterImage) && !string.IsNullOrEmpty(OgImage))
                TwitterImage = OgImage;

            MarkAsUpdated();
        }

        /// <summary>
        /// Полное автозаполнение всех связанных полей
        /// </summary>
        public void AutoFillRelatedFields()
        {
            FillOpenGraphFromMeta();
            FillTwitterFromOpenGraph();

            if (string.IsNullOrEmpty(UrlSlug))
                GenerateSlugFromTitle();

            MarkAsUpdated();
        }

        /// <summary>
        /// Обновляет метку времени изменения
        /// </summary>
        public void MarkAsUpdated(string? updatedBy = null)
        {
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updatedBy;
            ModifiedDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Получает рекомендации по улучшению SEO
        /// </summary>
        public List<string> GetSeoRecommendations()
        {
            var recommendations = new List<string>();

            if (string.IsNullOrEmpty(MetaTitle))
                recommendations.Add("Добавьте SEO заголовок");

            if (string.IsNullOrEmpty(MetaDescription))
                recommendations.Add("Добавьте META описание");

            if (string.IsNullOrEmpty(UrlSlug))
                recommendations.Add("Добавьте ЧПУ URL");

            if (!HasOpenGraph)
                recommendations.Add("Настройте Open Graph теги для социальных сетей");

            if (!HasTwitterCard)
                recommendations.Add("Настройте Twitter Card теги");

            if (!HasStructuredData)
                recommendations.Add("Добавьте структурированную разметку Schema.org");

            return recommendations;
        }
    }
}