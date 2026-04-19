using FluentValidation;
using pr_srv_names.Pages.Platform.Dtos;
using pr_srv_names.Pages.Shared.Seo.Validators;
using DAL.Constants;

namespace pr_srv_names.Pages.Platform.Services
{
    public class PlatformTranslationDtoValidator : AbstractValidator<PlatformTranslationDto>
    {
        public PlatformTranslationDtoValidator()
        {
            RuleFor(x => x.LanguageId).GreaterThan(0).WithMessage("Id языка должен быть положительным.");
            RuleFor(x => x.Name).NotEmpty().MaximumLength(PlatformConstants.NameMaxLength);
            RuleFor(x => x.Description).MaximumLength(PlatformConstants.DescriptionMaxLength);
            
            RuleFor(x => x.SeoData)
                .SetValidator(new SeoDataDtoValidator()!)
                .When(x => x.SeoData != null);
        }
    }

    public class PlatformCreateDtoValidator : AbstractValidator<PlatformCreateDto>
    {
        public PlatformCreateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(PlatformConstants.NameMaxLength);
            RuleFor(x => x.Code).NotEmpty().MaximumLength(PlatformConstants.CodeMaxLength);
            RuleFor(x => x.Family).MaximumLength(PlatformConstants.FamilyMaxLength);
            
            RuleFor(x => x.Translations)
                .ForEach(t => t.SetValidator(new PlatformTranslationDtoValidator()));
        }
    }

    public class PlatformUpdateDtoValidator : AbstractValidator<PlatformUpdateDto>
    {
        public PlatformUpdateDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(PlatformConstants.NameMaxLength);
            RuleFor(x => x.Code).NotEmpty().MaximumLength(PlatformConstants.CodeMaxLength);
            RuleFor(x => x.Family).MaximumLength(PlatformConstants.FamilyMaxLength);
            
            RuleFor(x => x.Translations)
                .ForEach(t => t.SetValidator(new PlatformTranslationDtoValidator()));
        }
    }

    public class PlatformPageRequestDtoValidator : AbstractValidator<PlatformPageRequestDto>
    {
        public PlatformPageRequestDtoValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        }
    }
}
