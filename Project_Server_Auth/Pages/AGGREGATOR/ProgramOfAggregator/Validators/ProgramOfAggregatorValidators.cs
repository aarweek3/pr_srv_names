using FluentValidation;
using pr_srv_names.Pages.AGGREGATOR.ProgramOfAggregator.Dtos;

namespace pr_srv_names.Pages.AGGREGATOR.ProgramOfAggregator.Validators
{
    public class ProgramOfAggregatorCreateValidator : AbstractValidator<ProgramOfAggregatorCreateDto>
    {
        public ProgramOfAggregatorCreateValidator()
        {
            RuleFor(x => x.CanonicalName).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Slug).NotEmpty().MaximumLength(100);
            RuleFor(x => x.CategoryOfAggregatorId).NotEmpty();
        }
    }

    public class ProgramOfAggregatorUpdateValidator : AbstractValidator<ProgramOfAggregatorUpdateDto>
    {
        public ProgramOfAggregatorUpdateValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.CanonicalName).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Slug).NotEmpty().MaximumLength(100);
            RuleFor(x => x.CategoryOfAggregatorId).NotEmpty();
        }
    }

    public class ProgramOfAggregatorPageRequestValidator : AbstractValidator<ProgramOfAggregatorPageRequestDto>
    {
        public ProgramOfAggregatorPageRequestValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
        }
    }

    public class VersionOfAggregatorCreateValidator : AbstractValidator<VersionOfAggregatorCreateDto>
    {
        public VersionOfAggregatorCreateValidator()
        {
            RuleFor(x => x.ProgramOfAggregatorId).NotEmpty();
            RuleFor(x => x.VersionNumber).NotEmpty().MaximumLength(50);
        }
    }

    public class VersionOfAggregatorUpdateValidator : AbstractValidator<VersionOfAggregatorUpdateDto>
    {
        public VersionOfAggregatorUpdateValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.ProgramOfAggregatorId).NotEmpty();
            RuleFor(x => x.VersionNumber).NotEmpty().MaximumLength(50);
        }
    }
}
