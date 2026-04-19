using FluentValidation;
using pr_srv_names.Pages.SampleMain.Constants;
using pr_srv_names.Pages.SampleMain.Extensions;
using pr_srv_names.Pages.SampleMainSeo.Dtos;
using pr_srv_names.Pages.Shared.Seo.Dtos;
using pr_srv_names.Pages.Shared.Seo.Validators;

namespace pr_srv_names.Pages.SampleMainSeo.Services
{

    public class SampleMainDescriptionSeoDtoValidator : AbstractValidator<SampleMainDescriptionSeoDto>
    {
        public SampleMainDescriptionSeoDtoValidator()
        {
            RuleFor(x => x.LanguageAppId).GreaterThan(0).WithMessage("Id языка должен быть положительным.");
            RuleFor(x => x.Name).ValidateLocalizedName();
            RuleFor(x => x.Description).ValidateLocalizedDescription();
            RuleFor(x => x.UrlPicture).MaximumLength(500).WithMessage("URL изображения не должен превышать 500 символов.");
            
            RuleFor(x => x.SeoData)
                .SetValidator(new SeoDataDtoValidator()!)
                .When(x => x.SeoData != null);
        }
    }

    public class SampleMainSeoCreateDtoValidator : AbstractValidator<SampleMainSeoCreateDto>
    {
        public SampleMainSeoCreateDtoValidator()
        {
            RuleFor(x => x.Name).ValidateName();
            RuleFor(x => x.SystemCode).ValidateSystemCode();
            RuleFor(x => x.UrlPictureMain).MaximumLength(500).WithMessage("URL изображения не должен превышать 500 символов.");
            
            RuleFor(x => x.Descriptions)
                .ForEach(description => description.SetValidator(new SampleMainDescriptionSeoDtoValidator()));
        }
    }

    public class SampleMainSeoUpdateDtoValidator : AbstractValidator<SampleMainSeoUpdateDto>
    {
        public SampleMainSeoUpdateDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id записи должен быть положительным.");
            RuleFor(x => x.Name).ValidateName();
            RuleFor(x => x.SystemCode).ValidateSystemCode();
            RuleFor(x => x.UrlPictureMain).MaximumLength(500).WithMessage("URL изображения не должен превышать 500 символов.");
            
            RuleFor(x => x.Descriptions)
                .ForEach(description => description.SetValidator(new SampleMainDescriptionSeoDtoValidator()));
        }
    }

    public class SampleMainSeoPageRequestDtoValidator : AbstractValidator<SampleMainSeoPageRequestDto>
    {
        public SampleMainSeoPageRequestDtoValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).InclusiveBetween(1, SampleMainValidationConstants.MaxPageSize);
            RuleFor(x => x.SearchTerm).MaximumLength(SampleMainValidationConstants.SearchTermMaxLength);
        }
    }
}
