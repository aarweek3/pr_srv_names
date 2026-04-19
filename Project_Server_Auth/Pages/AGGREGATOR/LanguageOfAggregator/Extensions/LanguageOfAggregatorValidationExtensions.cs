using FluentValidation;
using pr_srv_names.Pages.Aggregator.LanguageOfAggregator.Constants;

namespace pr_srv_names.Pages.Aggregator.LanguageOfAggregator.Extensions
{
    public static class LanguageOfAggregatorValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> ValidateLanguageCode<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Код языка обязателен.")
                .MaximumLength(LanguageOfAggregatorValidationConstants.CodeMaxLength)
                .WithMessage($"Код языка не должен превышать {LanguageOfAggregatorValidationConstants.CodeMaxLength} символов.");
        }

        public static IRuleBuilderOptions<T, string> ValidateShortCode<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Краткий код языка обязателен.")
                .MaximumLength(LanguageOfAggregatorValidationConstants.ShortCodeMaxLength)
                .WithMessage($"Краткий код не должен превышать {LanguageOfAggregatorValidationConstants.ShortCodeMaxLength} символов.");
        }

        public static IRuleBuilderOptions<T, string> ValidateTitle<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Название (EN) обязательно.")
                .MaximumLength(LanguageOfAggregatorValidationConstants.TitleMaxLength)
                .WithMessage($"Название не должно превышать {LanguageOfAggregatorValidationConstants.TitleMaxLength} символов.");
        }

        public static IRuleBuilderOptions<T, string> ValidateNativeTitle<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Название (Native) обязательно.")
                .MaximumLength(LanguageOfAggregatorValidationConstants.NativeTitleMaxLength)
                .WithMessage($"Название не должно превышать {LanguageOfAggregatorValidationConstants.NativeTitleMaxLength} символов.");
        }

        public static IRuleBuilderOptions<T, int> ValidateSortOrder<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThanOrEqualTo(0)
                .WithMessage("Порядок сортировки должен быть неотрицательным.");
        }

        public static IRuleBuilderOptions<T, int?> ValidateSortOrder<T>(this IRuleBuilder<T, int?> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThanOrEqualTo(0)
                .WithMessage("Порядок сортировки должен быть неотрицательным.");
        }

        public static IRuleBuilderOptions<T, string?> ValidateIconKey<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(LanguageOfAggregatorValidationConstants.IconKeyMaxLength)
                .WithMessage($"Ключ иконки не должен превышать {LanguageOfAggregatorValidationConstants.IconKeyMaxLength} символов.");
        }
    }
}
