namespace pr_srv_names.Pages.Anecdote.Constants
{
    /// <summary>
    /// Константы для валидации анекдотов
    /// </summary>
    public static class AnecdoteValidationConstants
    {
        /// <summary>
        /// Максимальная длина названия анекдота
        /// </summary>
        public const int AnecdoteNameMaxLength = 200;

        /// <summary>
        /// Максимальная длина описания анекдота
        /// </summary>
        public const int AnecdoteDescriptionMaxLength = 2000;

        /// <summary>
        /// Максимальный размер страницы для пагинации
        /// </summary>
        public const int AnecdoteMaxPageSize = 100;

        /// <summary>
        /// Максимальная длина поискового запроса
        /// </summary>
        public const int SearchTermMaxLength = 100;
    }
}
