using System;
using System.Collections.Generic;
using DAL.Models.GeneralModels;

namespace pr_srv_names.Pages.Shared.Seo.Extensions
{
    /// <summary>
    /// Расширения для работы с SEO данными. 
    /// Вынесено из модели для соблюдения чистоты архитектуры.
    /// </summary>
    public static class SeoDataExtensions
    {
        /// <summary>
        /// Расчитывает процент SEO оптимизации (0-100)
        /// </summary>
        public static int CalculateOptimizationScore(this SeoData seo)
        {
            if (seo == null) return 0;
            
            int score = 0;
            const int maxScore = 10;

            if (!string.IsNullOrEmpty(seo.MetaTitle)) score += 2;
            if (!string.IsNullOrEmpty(seo.MetaDescription)) score += 2;
            if (!string.IsNullOrEmpty(seo.UrlSlug)) score += 2;
            
            if (!string.IsNullOrEmpty(seo.OgTitle) && !string.IsNullOrEmpty(seo.OgDescription)) score += 2;
            if (!string.IsNullOrEmpty(seo.TwitterTitle) && !string.IsNullOrEmpty(seo.TwitterDescription)) score += 1;
            if (!string.IsNullOrEmpty(seo.SchemaJsonLd)) score += 1;

            return (int)Math.Round((double)score / maxScore * 100);
        }

        /// <summary>
        /// Выполняет полное автозаполнение связанных полей (OG, Twitter) на основе Meta тегов
        /// </summary>
        public static void AutoFillRelatedFields(this SeoData seo)
        {
            if (seo == null) return;

            // OG на основе Meta
            if (string.IsNullOrEmpty(seo.OgTitle)) seo.OgTitle = seo.MetaTitle;
            if (string.IsNullOrEmpty(seo.OgDescription)) seo.OgDescription = seo.MetaDescription;

            // Twitter на основе OG
            if (string.IsNullOrEmpty(seo.TwitterTitle)) seo.TwitterTitle = seo.OgTitle;
            if (string.IsNullOrEmpty(seo.TwitterDescription)) seo.TwitterDescription = seo.OgDescription;
            if (string.IsNullOrEmpty(seo.TwitterImage)) seo.TwitterImage = seo.OgImage;

            // Slug на основе Title (упрощенная версия)
            if (string.IsNullOrEmpty(seo.UrlSlug) && !string.IsNullOrEmpty(seo.MetaTitle))
            {
                seo.UrlSlug = seo.MetaTitle.ToLowerInvariant()
                    .Replace(" ", "-")
                    .Replace("_", "-")
                    .Trim('-');
            }
        }

        /// <summary>
        /// Формирует строку для мета-тега robots
        /// </summary>
        public static string GetRobotsContent(this SeoData seo)
        {
            if (seo == null) return "index, follow";
            
            var robots = new List<string>();
            robots.Add(seo.NoIndex ? "noindex" : "index");
            robots.Add(seo.NoFollow ? "nofollow" : "follow");

            return string.Join(", ", robots);
        }

        /// <summary>
        /// Генерирует список рекомендаций по улучшению SEO
        /// </summary>
        public static List<string> GetRecommendations(this SeoData seo)
        {
            var recs = new List<string>();
            if (seo == null) return recs;

            if (string.IsNullOrEmpty(seo.MetaTitle)) recs.Add("Добавьте заголовок (Meta Title)");
            if (string.IsNullOrEmpty(seo.MetaDescription)) recs.Add("Добавьте описание (Meta Description)");
            if (string.IsNullOrEmpty(seo.UrlSlug)) recs.Add("Настройте URL Slug");
            if (string.IsNullOrEmpty(seo.OgTitle)) recs.Add("Настройте Open Graph теги");
            if (string.IsNullOrEmpty(seo.SchemaJsonLd)) recs.Add("Добавьте структурированную разметку Schema.org");

            return recs;
        }
    }
}
