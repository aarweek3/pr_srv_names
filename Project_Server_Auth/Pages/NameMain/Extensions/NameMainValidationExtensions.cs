using FluentValidation;
using pr_srv_names.Pages.NameMain.Constants;

namespace pr_srv_names.Pages.NameMain.Extensions
{
    /// <summary>
    /// Extension методы для общих правил валидации namemain
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// Применяет общие правила валидации для названия namemain
        /// Проверяет, что название не пустое и имеет длину от 1 до NameMainNameMaxLength.
        /// </summary>
        public static IRuleBuilderOptions<T, string> ValidateNameMainName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Название namemain обязательно.")
                .Length(1, NameMainValidationConstants.NameMainNameMaxLength)
                .WithMessage(
                    $"Название namemain должно содержать от 1 до {NameMainValidationConstants.NameMainNameMaxLength} символов.")
                .Matches(@"^[A-Za-z0-9\s\u0400-\u04FF.,!?()-]+$")
                .WithMessage("Введен недопустимый символ.");
        }

        /// <summary>
        /// Применяет общие правила валидации для описания namemain
        /// Позволяет любые символы, включая пробелы, ограничивая только максимальную длину.
        /// </summary>
        public static IRuleBuilderOptions<T, string?> ValidateNameMainDescription<T>(
            this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(NameMainValidationConstants.NameMainDescriptionMaxLength)
                .WithMessage(
                    $"Описание namemain не должно превышать {NameMainValidationConstants.NameMainDescriptionMaxLength} символов.");
        }

        /// <summary>
        /// Применяет правила валидации для идентификатора namemain
        /// Проверяет, что идентификатор больше 0.
        /// </summary>
        public static IRuleBuilderOptions<T, int> ValidateNameMainId<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0)
                .WithMessage("Идентификатор namemain должен быть положительным.");
        }

        /// <summary>
        /// Применяет правила валидации для поискового запроса
        /// Проверяет, что длина не превышает SearchTermMaxLength.
        /// </summary>
        public static IRuleBuilderOptions<T, string?> ValidateSearchTerm<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(NameMainValidationConstants.SearchTermMaxLength)
                .WithMessage(
                    $"Поисковый запрос не должен превышать {NameMainValidationConstants.SearchTermMaxLength} символов.");
        }

        /// <summary>
        /// Применяет правила валидации для номера страницы
        /// Проверяет, что номер страницы больше или равен 1.
        /// </summary>
        public static IRuleBuilderOptions<T, int> ValidatePageNumber<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThanOrEqualTo(1)
                .WithMessage("Номер страницы должен быть положительным.");
        }

        /// <summary>
        /// Применяет правила валидации для размера страницы
        /// Проверяет, что размер страницы находится в диапазоне от 1 до NameMainMaxPageSize.
        /// </summary>
        public static IRuleBuilderOptions<T, int> ValidatePageSize<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .InclusiveBetween(1, NameMainValidationConstants.NameMainMaxPageSize)
                .WithMessage($"Размер страницы должен быть от 1 до {NameMainValidationConstants.NameMainMaxPageSize}.");
        }

        // Методы ValidateSortBy и ValidateSortDirection удалены, 
        // так как enum'ы автоматически ограничивают возможные значения
    }
}