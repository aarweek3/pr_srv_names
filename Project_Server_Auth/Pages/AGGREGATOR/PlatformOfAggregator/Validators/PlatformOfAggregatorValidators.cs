using FluentValidation;
using pr_srv_names.Pages.AGGREGATOR.PlatformOfAggregator.Dtos;
using pr_srv_names.Pages.AGGREGATOR.PlatformOfAggregator.Constants;

namespace pr_srv_names.Pages.AGGREGATOR.PlatformOfAggregator.Validators
{
    /// <summary>
    /// Валидатор для создания платформы.
    /// </summary>
    public class PlatformOfAggregatorCreateDtoValidator : AbstractValidator<PlatformOfAggregatorCreateDto>
    {
        public PlatformOfAggregatorCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Название платформы обязательно.")
                .MaximumLength(PlatformOfAggregatorValidationConstants.NameMaxLength)
                .WithMessage($"Название не должно превышать {PlatformOfAggregatorValidationConstants.NameMaxLength} символов.");

            RuleFor(x => x.SystemCode)
                .NotEmpty().WithMessage("Системный код обязателен.")
                .MaximumLength(PlatformOfAggregatorValidationConstants.CodeMaxLength)
                .WithMessage($"Код не должен превышать {PlatformOfAggregatorValidationConstants.CodeMaxLength} символов.");

            RuleFor(x => x.SortOrder)
                .InclusiveBetween(PlatformOfAggregatorValidationConstants.MinSortOrder, PlatformOfAggregatorValidationConstants.MaxSortOrder)
                .WithMessage($"Порядок сортировки должен быть в диапазоне от {PlatformOfAggregatorValidationConstants.MinSortOrder} до {PlatformOfAggregatorValidationConstants.MaxSortOrder}.");

            RuleForEach(x => x.Localizations).SetValidator(new PlatformOfAggregatorLocalizationDtoValidator());
        }
    }

    /// <summary>
    /// Валидатор для обновления платформы.
    /// </summary>
    public class PlatformOfAggregatorUpdateDtoValidator : AbstractValidator<PlatformOfAggregatorUpdateDto>
    {
        public PlatformOfAggregatorUpdateDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Идентификатор обязателен.");

            RuleFor(x => x.Name)
                .MaximumLength(PlatformOfAggregatorValidationConstants.NameMaxLength)
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.SystemCode)
                .MaximumLength(PlatformOfAggregatorValidationConstants.CodeMaxLength)
                .When(x => !string.IsNullOrEmpty(x.SystemCode));

            RuleForEach(x => x.Localizations).SetValidator(new PlatformOfAggregatorLocalizationDtoValidator());
        }
    }

    /// <summary>
    /// Валидатор локализации.
    /// </summary>
    public class PlatformOfAggregatorLocalizationDtoValidator : AbstractValidator<PlatformOfAggregatorLocalizationDto>
    {
        public PlatformOfAggregatorLocalizationDtoValidator()
        {
            RuleFor(x => x.LanguageOfAggregatorId).NotEmpty().WithMessage("ID языка обязателен.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Название в локализации обязательно.").MaximumLength(255);
            RuleFor(x => x.UrlPicture).MaximumLength(500);
        }
    }

    /// <summary>
    /// Валидатор запроса страницы.
    /// </summary>
    public class PlatformOfAggregatorPageRequestDtoValidator : AbstractValidator<PlatformOfAggregatorPageRequestDto>
    {
        public PlatformOfAggregatorPageRequestDtoValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("Номер страницы должен быть больше 0.");
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100).WithMessage("Размер страницы должен быть от 1 до 100.");
        }
    }
}
