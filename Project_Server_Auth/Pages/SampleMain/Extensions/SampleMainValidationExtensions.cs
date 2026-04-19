using FluentValidation;
using pr_srv_names.Pages.SampleMain.Constants;

namespace pr_srv_names.Pages.SampleMain.Extensions
{
    public static class SampleMainValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> ValidateName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Техническое название обязательно.")
                .MaximumLength(SampleMainValidationConstants.NameMaxLength)
                .WithMessage($"Название не должно превышать {SampleMainValidationConstants.NameMaxLength} символов.");
        }

        public static IRuleBuilderOptions<T, string?> ValidateSystemCode<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(SampleMainValidationConstants.SystemCodeMaxLength)
                .WithMessage($"Системный код не должен превышать {SampleMainValidationConstants.SystemCodeMaxLength} символов.");
        }

        public static IRuleBuilderOptions<T, string?> ValidateLocalizedName<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(SampleMainValidationConstants.NameMaxLength)
                .WithMessage($"Локализованное название не должно превышать {SampleMainValidationConstants.NameMaxLength} символов.");
        }

        public static IRuleBuilderOptions<T, string?> ValidateLocalizedDescription<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(SampleMainValidationConstants.DescriptionMaxLength)
                .WithMessage($"Локализованное описание не должно превышать {SampleMainValidationConstants.DescriptionMaxLength} символов.");
        }
    }
}
