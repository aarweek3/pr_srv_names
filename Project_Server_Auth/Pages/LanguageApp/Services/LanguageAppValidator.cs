using FluentValidation;
using pr_srv_names.Pages.LanguageApp.Dtos;
using pr_srv_names.Pages.LanguageApp.Extensions;

namespace pr_srv_names.Pages.LanguageApp.Services
{
    public class CreateLanguageAppDtoValidator : AbstractValidator<CreateLanguageAppDto>
    {
        public CreateLanguageAppDtoValidator()
        {
            RuleFor(x => x.Code).ValidateLanguageCode();
            RuleFor(x => x.ShortCode).ValidateShortCode();
            RuleFor(x => x.Title).ValidateTitle();
            RuleFor(x => x.NativeTitle).ValidateNativeTitle();
            RuleFor(x => x.Direction).ValidateDirection();
            RuleFor(x => x.SortOrder).ValidateSortOrder();
            RuleFor(x => x.IconKey).ValidateIconKey();
        }
    }

    public class UpdateLanguageAppDtoValidator : AbstractValidator<UpdateLanguageAppDto>
    {
        public UpdateLanguageAppDtoValidator()
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
                
            RuleFor(x => x.Direction).ValidateDirection()
                .When(x => x.Direction != null);
                
            RuleFor(x => x.SortOrder).ValidateSortOrder()
                .When(x => x.SortOrder.HasValue);
                
            RuleFor(x => x.IconKey).ValidateIconKey()
                .When(x => x.IconKey != null);
        }
    }
}
