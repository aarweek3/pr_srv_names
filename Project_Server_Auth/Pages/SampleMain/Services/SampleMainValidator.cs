using FluentValidation;
using pr_srv_names.Pages.SampleMain.Dtos;
using pr_srv_names.Pages.SampleMain.Extensions;
using pr_srv_names.Pages.SampleMain.Constants;

namespace pr_srv_names.Pages.SampleMain.Services
{
    public class SampleMainDescriptionDtoValidator : AbstractValidator<SampleMainDescriptionDto>
    {
        public SampleMainDescriptionDtoValidator()
        {
            RuleFor(x => x.LanguageAppId).GreaterThan(0).WithMessage("Id языка должен быть положительным.");
            RuleFor(x => x.Name).ValidateLocalizedName();
            RuleFor(x => x.Description).ValidateLocalizedDescription();
        }
    }

    public class SampleMainCreateRequestDtoValidator : AbstractValidator<SampleMainCreateRequestDto>
    {
        public SampleMainCreateRequestDtoValidator()
        {
            RuleFor(x => x.Name).ValidateName();
            RuleFor(x => x.SystemCode).ValidateSystemCode();
            
            RuleFor(x => x.Descriptions)
                .NotEmpty().WithMessage("Необходимо добавить хотя бы один перевод.")
                .ForEach(description => description.SetValidator(new SampleMainDescriptionDtoValidator()));
        }
    }

    public class SampleMainUpdateRequestDtoValidator : AbstractValidator<SampleMainUpdateRequestDto>
    {
        public SampleMainUpdateRequestDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id записи должен быть положительным.");
            RuleFor(x => x.Name).ValidateName();
            RuleFor(x => x.SystemCode).ValidateSystemCode();
            
            RuleFor(x => x.Descriptions)
                .NotEmpty().WithMessage("Необходимо добавить хотя бы один перевод.")
                .ForEach(description => description.SetValidator(new SampleMainDescriptionDtoValidator()));
        }
    }

    public class SampleMainPageRequestDtoValidator : AbstractValidator<SampleMainPageRequestDto>
    {
        public SampleMainPageRequestDtoValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).InclusiveBetween(1, SampleMainValidationConstants.MaxPageSize);
            RuleFor(x => x.SearchTerm).MaximumLength(SampleMainValidationConstants.SearchTermMaxLength);
        }
    }
}
