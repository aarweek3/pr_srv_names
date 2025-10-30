using FluentValidation;
using pr_srv_names.Pages.Anecdote.Constants;

namespace pr_srv_names.Pages.Anecdote.Extensions
{
    /// <summary>
    /// Extension методы для общих правил валидации анекдотов
    /// Содержит статические методы расширения для настройки валидации DTO-объектов анекдотов.
    /// </summary>
    public static class AnecdoteValidationExtensions
    {
        /// <summary>
        /// Применяет общие правила валидации для названия анекдота
        /// Проверяет, что название не пустое и имеет длину от 1 до AnecdoteNameMaxLength.
        /// </summary>
        public static IRuleBuilderOptions<T, string> ValidateAnecdoteName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Название анекдота обязательно.") // Обязательное поле
                .Length(1, AnecdoteValidationConstants.AnecdoteNameMaxLength) // Ограничивает длину
                .WithMessage(
                    $"Название анекдота должно содержать от 1 до {AnecdoteValidationConstants.AnecdoteNameMaxLength} символов.");
        }

        /// <summary>
        /// Применяет общие правила валидации для описания анекдота
        /// Позволяет любые символы, включая пробелы, ограничивая только максимальную длину.
        /// </summary>
        public static IRuleBuilderOptions<T, string?> ValidateAnecdoteDescription<T>(
            this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(AnecdoteValidationConstants
                    .AnecdoteDescriptionMaxLength) // Ограничивает максимальную длину
                .WithMessage(
                    $"Описание анекдота не должно превышать {AnecdoteValidationConstants.AnecdoteDescriptionMaxLength} символов.");
        }

        /// <summary>
        /// Применяет правила валидации для идентификатора анекдота
        /// Проверяет, что идентификатор больше 0.
        /// </summary>
        public static IRuleBuilderOptions<T, int> ValidateAnecdoteId<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0) // Идентификатор должен быть положительным
                .WithMessage("Идентификатор анекдота должен быть положительным.");
        }

        /// <summary>
        /// Применяет правила валидации для NameMainId
        /// Проверяет, что идентификатор основного имени больше 0.
        /// </summary>
        public static IRuleBuilderOptions<T, int> ValidateNameMainId<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0) // Идентификатор должен быть положительным
                .WithMessage("Идентификатор имени (NameMainId) должен быть положительным.");
        }

        /// <summary>
        /// Применяет правила валидации для LanguageId
        /// Проверяет, что идентификатор языка больше 0.
        /// </summary>
        public static IRuleBuilderOptions<T, int> ValidateLanguageId<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0) // Идентификатор должен быть положительным
                .WithMessage("Идентификатор языка (LanguageId) должен быть положительным.");
        }

        /// <summary>
        /// Применяет правила валидации для поискового запроса
        /// Проверяет, что длина не превышает SearchTermMaxLength.
        /// </summary>
        public static IRuleBuilderOptions<T, string?> ValidateSearchTerm<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(AnecdoteValidationConstants.SearchTermMaxLength) // Ограничивает максимальную длину
                .WithMessage(
                    $"Поисковый запрос не должен превышать {AnecdoteValidationConstants.SearchTermMaxLength} символов.");
        }

        /// <summary>
        /// Применяет правила валидации для номера страницы
        /// Проверяет, что номер страницы больше или равен 1.
        /// </summary>
        public static IRuleBuilderOptions<T, int> ValidatePageNumber<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThanOrEqualTo(1) // Номер страницы должен быть положительным
                .WithMessage("Номер страницы должен быть положительным.");
        }

        /// <summary>
        /// Применяет правила валидации для размера страницы
        /// Проверяет, что размер страницы находится в диапазоне от 1 до AnecdoteMaxPageSize.
        /// </summary>
        public static IRuleBuilderOptions<T, int> ValidatePageSize<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .InclusiveBetween(1, AnecdoteValidationConstants.AnecdoteMaxPageSize) // Диапазон допустимых значений
                .WithMessage($"Размер страницы должен быть от 1 до {AnecdoteValidationConstants.AnecdoteMaxPageSize}.");
        }
    }
}