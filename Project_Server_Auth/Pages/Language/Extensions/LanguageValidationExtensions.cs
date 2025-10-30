using FluentValidation;
using pr_srv_names.Pages.Language.Constants;
using System.Text.RegularExpressions;

namespace pr_srv_names.Pages.Language.Extensions
{
    /// <summary>
    /// Extension методы для общих правил валидации language
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// Применяет общие правила валидации для названия language
        /// Проверяет, что название не пустое и имеет длину от 1 до LanguageNameMaxLength.
        /// </summary>
        public static IRuleBuilderOptions<T, string> ValidateLanguageName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Название language обязательно.")
                .Length(1, LanguageValidationConstants.LanguageNameMaxLength)
                .WithMessage(
                    $"Название language должно содержать от 1 до {LanguageValidationConstants.LanguageNameMaxLength} символов.")
                .Matches(@"^[A-Za-z0-9\s\u0400-\u04FF.,!?()-]+$")
                .WithMessage("Введен недопустимый символ.");
        }

        /// <summary>
        /// Применяет общие правила валидации для описания language
        /// Позволяет любые символы, включая пробелы, ограничивая только максимальную длину.
        /// </summary>
        public static IRuleBuilderOptions<T, string?> ValidateLanguageDescription<T>(
            this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(LanguageValidationConstants.LanguageDescriptionMaxLength)
                .WithMessage(
                    $"Описание language не должно превышать {LanguageValidationConstants.LanguageDescriptionMaxLength} символов.");
        }

        /// <summary>
        /// Применяет правила валидации для идентификатора language
        /// Проверяет, что идентификатор больше 0.
        /// </summary>
        public static IRuleBuilderOptions<T, int> ValidateLanguageId<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0)
                .WithMessage("Идентификатор language должен быть положительным.");
        }

        /// <summary>
        /// Применяет правила валидации для поискового запроса
        /// Проверяет, что длина не превышает SearchTermMaxLength.
        /// </summary>
        public static IRuleBuilderOptions<T, string?> ValidateSearchTerm<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(LanguageValidationConstants.SearchTermMaxLength)
                .WithMessage(
                    $"Поисковый запрос не должен превышать {LanguageValidationConstants.SearchTermMaxLength} символов.");
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
        /// Проверяет, что размер страницы находится в диапазоне от 1 до LanguageMaxPageSize.
        /// </summary>
        public static IRuleBuilderOptions<T, int> ValidatePageSize<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .InclusiveBetween(1, LanguageValidationConstants.LanguageMaxPageSize)
                .WithMessage($"Размер страницы должен быть от 1 до {LanguageValidationConstants.LanguageMaxPageSize}.");
        }

        /// <summary>
        /// Применяет правила валидации для кода языка (ISO 639-1)
        /// </summary>
        public static IRuleBuilderOptions<T, string> ValidateLanguageCode<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Код языка обязателен.")
                .Length(LanguageValidationConstants.LanguageCodeLength)
                .WithMessage(
                    $"Код языка должен содержать ровно {LanguageValidationConstants.LanguageCodeLength} символа.")
                .Matches(@"^[a-z]{2}$")
                .WithMessage(
                    "Код языка должен быть двухбуквенным кодом по стандарту ISO 639-1 (например, 'en', 'ru').");
        }

        /// <summary>
        /// Применяет правила валидации для кода флага (ISO 3166-1 alpha-2)
        /// </summary>
        public static IRuleBuilderOptions<T, string?> ValidateFlagCode<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .Must((rootObject, flagCode, context) =>
                {
                    if (string.IsNullOrEmpty(flagCode))
                        return true; // null или пустая строка - допустимо

                    if (flagCode.Length != LanguageValidationConstants.FlagCodeLength)
                    {
                        context.MessageFormatter.AppendArgument("FlagCodeLength",
                            LanguageValidationConstants.FlagCodeLength);
                        return false;
                    }

                    return Regex.IsMatch(flagCode, @"^[a-z]{2}$");
                })
                .WithMessage($"Код флага должен содержать ровно {LanguageValidationConstants.FlagCodeLength} символа.")
                .WithMessage(
                    "Код флага должен быть двухбуквенным кодом по стандарту ISO 3166-1 alpha-2 (например, 'gb', 'ru').");
        }
    }
}