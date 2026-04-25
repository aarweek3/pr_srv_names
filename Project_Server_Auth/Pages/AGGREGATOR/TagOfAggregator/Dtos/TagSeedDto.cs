using System.Collections.Generic;

namespace pr_srv_names.Pages.AGGREGATOR.TagOfAggregator.Dtos
{
    /// <summary>
    /// DTO для сидинга категорий из JSON (Maintenance Standard v3.5)
    /// </summary>
    public class CategoryTagSeedDto
    {
        public string Slug { get; set; }
        public string IconName { get; set; }
        public string Color { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public Dictionary<string, TagLocalizationSeedDto> Translations { get; set; }
        public List<TagSeedDto> Tags { get; set; }
    }

    /// <summary>
    /// DTO для сидинга тегов из JSON
    /// </summary>
    public class TagSeedDto
    {
        public string Slug { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public string IconName { get; set; }
        public int SortOrder { get; set; }
        public bool IsFeature { get; set; }
        public Dictionary<string, TagLocalizationSeedDto> Translations { get; set; }
    }

    /// <summary>
    /// Локализация для сидинга
    /// </summary>
    public class TagLocalizationSeedDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string H1Title { get; set; }
    }
}
