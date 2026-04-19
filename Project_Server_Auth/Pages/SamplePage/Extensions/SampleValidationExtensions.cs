using FluentValidation;
using pr_srv_names.Pages.Sample.Constants;

namespace pr_srv_names.Pages.Sample.Extensions
{
    /// <summary>
    /// Extension методы для общих правил валидации sample
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// Применяет общие правила валидации для названия sample
        /// Проверяет, что название не пустое и имеет длину от 1 до SampleNameMaxLength.
        /// </summary>
        public static IRuleBuilderOptions<T, string> ValidateSampleName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Название sample обязательно.")
                .Length(1, SampleValidationConstants.SampleNameMaxLength)
                .WithMessage(
                    $"Название sample должно содержать от 1 до {SampleValidationConstants.SampleNameMaxLength} символов.")
                .Matches(@"^[A-Za-z0-9\s\u0400-\u04FF.,!?()-]+$")
                .WithMessage("Введен недопустимый символ.");
        }

        /// <summary>
        /// Применяет общие правила валидации для описания sample
        /// Разрешает любые символы, включая ведущие и конечные пробелы, ограничивает только максимальную длину.
        /// </summary>
        public static IRuleBuilderOptions<T, string?> ValidateSampleDescription<T>(
            this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(SampleValidationConstants.SampleDescriptionMaxLength)
                .WithMessage(
                    $"Описание sample не должно превышать {SampleValidationConstants.SampleDescriptionMaxLength} символов.");
        }

        /// <summary>
        /// Применяет правила валидации для идентификатора sample
        /// Проверяет, что идентификатор больше 0.
        /// </summary>
        public static IRuleBuilderOptions<T, int> ValidateSampleId<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0)
                .WithMessage("Идентификатор sample должен быть положительным.");
        }

        /// <summary>
        /// Применяет правила валидации для поискового запроса
        /// Проверяет, что длина не превышает SearchTermMaxLength.
        /// </summary>
        public static IRuleBuilderOptions<T, string?> ValidateSearchTerm<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(SampleValidationConstants.SearchTermMaxLength)
                .WithMessage(
                    $"Поисковый запрос не должен превышать {SampleValidationConstants.SearchTermMaxLength} символов.");
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
        /// Проверяет, что размер страницы находится в диапазоне от 1 до SampleMaxPageSize.
        /// </summary>
        public static IRuleBuilderOptions<T, int> ValidatePageSize<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .InclusiveBetween(1, SampleValidationConstants.SampleMaxPageSize)
                .WithMessage($"Размер страницы должен быть от 1 до {SampleValidationConstants.SampleMaxPageSize}.");
        }

        // Методы ValidateSortBy и ValidateSortDirection удалены, 
        // так как enum'ы автоматически ограничивают возможные значения
    }
}