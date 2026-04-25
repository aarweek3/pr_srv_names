using FluentValidation;
using pr_srv_names.Pages.AGGREGATOR.LicenseTypeOfAggregator.Dtos;
using pr_srv_names.Pages.AGGREGATOR.LicenseTypeOfAggregator.Constants;

namespace pr_srv_names.Pages.AGGREGATOR.LicenseTypeOfAggregator.Validators
{
    /// <summary>
    /// Валидатор для создания типа лицензии.
    /// </summary>
    public class LicenseTypeOfAggregatorCreateDtoValidator : AbstractValidator<LicenseTypeOfAggregatorCreateDto>
    {
        public LicenseTypeOfAggregatorCreateDtoValidator()
        {
            RuleFor(x => x.CanonicalName)
                .NotEmpty().WithMessage("Каноническое название обязательно.")
                .MaximumLength(LicenseTypeOfAggregatorValidationConstants.NameMaxLength)
                .WithMessage($"Название не должно превышать {LicenseTypeOfAggregatorValidationConstants.NameMaxLength} символов.");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug обязателен.")
                .MaximumLength(LicenseTypeOfAggregatorValidationConstants.CodeMaxLength)
                .WithMessage($"Slug не должен превышать {LicenseTypeOfAggregatorValidationConstants.CodeMaxLength} символов.");

            RuleFor(x => x.SortOrder)
                .InclusiveBetween(LicenseTypeOfAggregatorValidationConstants.MinSortOrder, LicenseTypeOfAggregatorValidationConstants.MaxSortOrder)
                .WithMessage($"Порядок сортировки должен быть в диапазоне от {LicenseTypeOfAggregatorValidationConstants.MinSortOrder} до {LicenseTypeOfAggregatorValidationConstants.MaxSortOrder}.");

            RuleForEach(x => x.Localizations).SetValidator(new LicenseTypeOfAggregatorLocalizationDtoValidator());
        }
    }

    /// <summary>
    /// Валидатор для обновления типа лицензии.
    /// </summary>
    public class LicenseTypeOfAggregatorUpdateDtoValidator : AbstractValidator<LicenseTypeOfAggregatorUpdateDto>
    {
        public LicenseTypeOfAggregatorUpdateDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Идентификатор обязателен.");

            RuleFor(x => x.CanonicalName)
                .MaximumLength(LicenseTypeOfAggregatorValidationConstants.NameMaxLength)
                .When(x => !string.IsNullOrEmpty(x.CanonicalName));

            RuleFor(x => x.Slug)
                .MaximumLength(LicenseTypeOfAggregatorValidationConstants.CodeMaxLength)
                .When(x => !string.IsNullOrEmpty(x.Slug));

            RuleForEach(x => x.Localizations).SetValidator(new LicenseTypeOfAggregatorLocalizationDtoValidator());
        }
    }

    /// <summary>
    /// Валидатор локализации.
    /// </summary>
    public class LicenseTypeOfAggregatorLocalizationDtoValidator : AbstractValidator<LicenseTypeOfAggregatorLocalizationDto>
    {
        public LicenseTypeOfAggregatorLocalizationDtoValidator()
        {
            RuleFor(x => x.LanguageOfAggregatorId).NotEmpty().WithMessage("ID языка обязателен.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Название в локализации обязательно.").MaximumLength(255);
            RuleFor(x => x.UrlPicture).MaximumLength(500);
        }
    }

    /// <summary>
    /// Валидатор запроса страницы.
    /// </summary>
    public class LicenseTypeOfAggregatorPageRequestDtoValidator : AbstractValidator<LicenseTypeOfAggregatorPageRequestDto>
    {
        public LicenseTypeOfAggregatorPageRequestDtoValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("Номер страницы должен быть больше 0.");
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100).WithMessage("Размер страницы должен быть от 1 до 100.");
        }
    }
}
