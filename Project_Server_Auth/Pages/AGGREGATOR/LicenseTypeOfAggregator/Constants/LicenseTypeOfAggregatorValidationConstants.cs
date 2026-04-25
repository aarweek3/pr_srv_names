using DAL.Constants;

namespace pr_srv_names.Pages.AGGREGATOR.LicenseTypeOfAggregator.Constants
{
    /// <summary>
    /// Константы валидации для типов лицензий агрегатора.
    /// </summary>
    public static class LicenseTypeOfAggregatorValidationConstants
    {
        public const int NameMaxLength = LicenseTypeConstants.NameMaxLength;
        public const int CodeMaxLength = LicenseTypeConstants.CodeMaxLength;
        public const int DescriptionMaxLength = LicenseTypeConstants.DescriptionMaxLength;

        public const int MinSortOrder = LicenseTypeConstants.MinSortOrder;
        public const int MaxSortOrder = LicenseTypeConstants.MaxSortOrder;

        public const int MaxPageSize = 100;
        public const int SearchTermMaxLength = 100;
    }
}
