using FluentValidation;
using pr_srv_names.Pages.AGGREGATOR.CategoryOfAggregator.Dtos;

namespace pr_srv_names.Pages.AGGREGATOR.CategoryOfAggregator.Validators
{
    public class CategoryOfAggregatorCreateValidator : AbstractValidator<CategoryOfAggregatorCreateDto>
    {
        public CategoryOfAggregatorCreateValidator()
        {
            RuleFor(x => x.CanonicalName).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Slug).NotEmpty().MaximumLength(100);
        }
    }

    public class CategoryOfAggregatorUpdateValidator : AbstractValidator<CategoryOfAggregatorUpdateDto>
    {
        public CategoryOfAggregatorUpdateValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.CanonicalName).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Slug).NotEmpty().MaximumLength(100);
        }
    }

    public class CategoryOfAggregatorPageRequestValidator : AbstractValidator<CategoryOfAggregatorPageRequestDto>
    {
        public CategoryOfAggregatorPageRequestValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
        }
    }
}
