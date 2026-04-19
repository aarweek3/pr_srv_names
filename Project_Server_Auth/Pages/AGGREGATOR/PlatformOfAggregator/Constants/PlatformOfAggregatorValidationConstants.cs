using DAL.Constants;

namespace pr_srv_names.Pages.AGGREGATOR.PlatformOfAggregator.Constants
{
    /// <summary>
    /// Константы валидации для платформы (ОС) агрегатора.
    /// Построено на базе глобальных констант PlatformConstants.
    /// </summary>
    public static class PlatformOfAggregatorValidationConstants
    {
        // Максимальная длина названия платформы
        public const int NameMaxLength = PlatformConstants.NameMaxLength;

        // Максимальная длина системного кода (slug)
        public const int CodeMaxLength = PlatformConstants.CodeMaxLength;

        // Максимальная длина семейства (desktop, mobile)
        public const int FamilyMaxLength = PlatformConstants.FamilyMaxLength;

        // Максимальная длина описания
        public const int DescriptionMaxLength = PlatformConstants.DescriptionMaxLength;

        // Сортировка
        public const int MinSortOrder = PlatformConstants.MinSortOrder;
        public const int MaxSortOrder = PlatformConstants.MaxSortOrder;

        // Стандартные параметры пагинации
        public const int MaxPageSize = 100;
        public const int SearchTermMaxLength = 100;
    }
}
