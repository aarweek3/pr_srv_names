using FluentValidation;
using pr_srv_names.Pages.Aggregator.LanguageOfAggregator.Dtos;
using pr_srv_names.Pages.Aggregator.LanguageOfAggregator.Extensions;

namespace pr_srv_names.Pages.Aggregator.LanguageOfAggregator.Services
{
    public class CreateLanguageOfAggregatorDtoValidator : AbstractValidator<CreateLanguageOfAggregatorDto>
    {
        public CreateLanguageOfAggregatorDtoValidator()
        {
            RuleFor(x => x.Code).ValidateLanguageCode();
            RuleFor(x => x.ShortCode).ValidateShortCode();
            RuleFor(x => x.Title).ValidateTitle();
            RuleFor(x => x.NativeTitle).ValidateNativeTitle();
            RuleFor(x => x.SortOrder).ValidateSortOrder();
            RuleFor(x => x.IconKey).ValidateIconKey();
        }
    }

    public class UpdateLanguageOfAggregatorDtoValidator : AbstractValidator<UpdateLanguageOfAggregatorDto>
    {
        public UpdateLanguageOfAggregatorDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            
            RuleFor(x => x.Code).ValidateLanguageCode()
                .When(x => x.Code != null);
                
            RuleFor(x => x.ShortCode).ValidateShortCode()
                .When(x => x.ShortCode != null);
                
            RuleFor(x => x.Title).ValidateTitle()
                .When(x => x.Title != null);
                
            RuleFor(x => x.NativeTitle).ValidateNativeTitle()
                .When(x => x.NativeTitle != null);
                
            RuleFor(x => x.SortOrder).ValidateSortOrder()
                .When(x => x.SortOrder.HasValue);
                
            RuleFor(x => x.IconKey).ValidateIconKey()
                .When(x => x.IconKey != null);
        }
    }
}
