namespace pr_srv_names.Pages.Language.Constants
{
    /// <summary>
    /// Константы для валидации language.
    /// </summary>
    public static class LanguageValidationConstants
    {
        /// <summary>
        /// Максимальная длина названия language.
        /// </summary>
        public const int LanguageNameMaxLength = 100;
        /// <summary>
        /// Максимальная длина описания language.
        /// </summary>
        public const int LanguageDescriptionMaxLength = 500;
        /// <summary>
        /// Максимальный размер страницы для пагинации.
        /// </summary>
        public const int LanguageMaxPageSize = 100;
        /// <summary>
        /// Максимальная длина поискового запроса.
        /// </summary>
        public const int SearchTermMaxLength = 100;
        /// <summary>
        /// Длина кода языка (ISO 639-1, двухбуквенный код).
        /// </summary>
        public const int LanguageCodeLength = 2;
        /// <summary>
        /// Длина кода флага (ISO 3166-1 alpha-2).
        /// </summary>
        public const int FlagCodeLength = 2;
    }
}