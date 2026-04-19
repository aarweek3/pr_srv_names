using System;

namespace pr_srv_names.Pages.Shared.Seo.Dtos
{
    /// <summary>
    /// Универсальный DTO для SEO данных. Вынесено в отдельный модуль.
    /// </summary>
    public class SeoDataDto
    {
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }
        public string? UrlSlug { get; set; }
        public string? CanonicalUrl { get; set; }

        public string? OgTitle { get; set; }
        public string? OgDescription { get; set; }
        public string? OgImage { get; set; }
        public string? OgType { get; set; }
        public string? OgUrl { get; set; }

        public string? TwitterCard { get; set; }
        public string? TwitterTitle { get; set; }
        public string? TwitterDescription { get; set; }
        public string? TwitterImage { get; set; }

        public string? ImageUrl { get; set; }
        public string? ImageAltText { get; set; }
        public string? ImageCaption { get; set; }

        public string? SchemaType { get; set; }
        public string? SchemaJsonLd { get; set; }

        public string? AuthorName { get; set; }
        public string? PublisherName { get; set; }
        public DateTime? PublishedDate { get; set; }

        public bool NoIndex { get; set; }
        public bool NoFollow { get; set; }
        public int? Priority { get; set; }
        public string? Region { get; set; }
    }
}
