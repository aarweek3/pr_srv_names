using FluentValidation;
using pr_srv_names.Pages.AGGREGATOR.DeveloperOfAggregator.Dtos;

namespace pr_srv_names.Pages.AGGREGATOR.DeveloperOfAggregator.Validators
{
    public class DeveloperOfAggregatorCreateValidator : AbstractValidator<DeveloperOfAggregatorCreateDto>
    {
        public DeveloperOfAggregatorCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
            RuleFor(x => x.SystemCode).NotEmpty().MaximumLength(100);
            RuleForEach(x => x.Localizations).SetValidator(new DeveloperOfAggregatorLocalizationValidator());
        }
    }

    public class DeveloperOfAggregatorUpdateValidator : AbstractValidator<DeveloperOfAggregatorUpdateDto>
    {
        public DeveloperOfAggregatorUpdateValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
            RuleFor(x => x.SystemCode).NotEmpty().MaximumLength(100);
            RuleForEach(x => x.Localizations).SetValidator(new DeveloperOfAggregatorLocalizationValidator());
        }
    }

    public class DeveloperOfAggregatorLocalizationValidator : AbstractValidator<DeveloperOfAggregatorLocalizationDto>
    {
        public DeveloperOfAggregatorLocalizationValidator()
        {
            RuleFor(x => x.LanguageOfAggregatorId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        }
    }

    public class DeveloperOfAggregatorPageRequestValidator : AbstractValidator<DeveloperOfAggregatorPageRequestDto>
    {
        public DeveloperOfAggregatorPageRequestValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).GreaterThan(0);
        }
    }
}
