using FluentValidation;
using pr_srv_names.Pages.LanguageApp.Constants;

namespace pr_srv_names.Pages.LanguageApp.Extensions
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> ValidateLanguageCode<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Код языка обязателен.")
                .MaximumLength(LanguageAppValidationConstants.CodeMaxLength)
                .WithMessage($"Код языка не должен превышать {LanguageAppValidationConstants.CodeMaxLength} символов.");
        }

        public static IRuleBuilderOptions<T, string> ValidateShortCode<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Краткий код языка обязателен.")
                .MaximumLength(LanguageAppValidationConstants.ShortCodeMaxLength)
                .WithMessage($"Краткий код не должен превышать {LanguageAppValidationConstants.ShortCodeMaxLength} символов.");
        }

        public static IRuleBuilderOptions<T, string> ValidateTitle<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Название (EN) обязательно.")
                .MaximumLength(LanguageAppValidationConstants.TitleMaxLength)
                .WithMessage($"Название не должно превышать {LanguageAppValidationConstants.TitleMaxLength} символов.");
        }

        public static IRuleBuilderOptions<T, string> ValidateNativeTitle<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Название (Native) обязательно.")
                .MaximumLength(LanguageAppValidationConstants.NativeTitleMaxLength)
                .WithMessage($"Название не должно превышать {LanguageAppValidationConstants.NativeTitleMaxLength} символов.");
        }

        public static IRuleBuilderOptions<T, string> ValidateDirection<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Направление письма обязательно.")
                .Must(x => x == "ltr" || x == "rtl")
                .WithMessage("Направление письма должно быть 'ltr' или 'rtl'.");
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
                .MaximumLength(LanguageAppValidationConstants.IconKeyMaxLength)
                .WithMessage($"Ключ иконки не должен превышать {LanguageAppValidationConstants.IconKeyMaxLength} символов.");
        }
    }
}
