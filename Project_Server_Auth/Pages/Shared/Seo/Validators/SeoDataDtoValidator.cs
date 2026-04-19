using FluentValidation;
using pr_srv_names.Pages.Shared.Seo.Dtos;

namespace pr_srv_names.Pages.Shared.Seo.Validators
{
    public class SeoDataDtoValidator : AbstractValidator<SeoDataDto>
    {
        public SeoDataDtoValidator()
        {
            RuleFor(x => x.MetaTitle)
                .MaximumLength(70).WithMessage("SEO заголовок не должен превышать 70 символов.")
                .MinimumLength(5).When(x => !string.IsNullOrEmpty(x.MetaTitle)).WithMessage("SEO заголовок слишком короткий.");
            
            RuleFor(x => x.MetaDescription)
                .MaximumLength(160).WithMessage("META описание не должен превышать 160 символов.")
                .MinimumLength(10).When(x => !string.IsNullOrEmpty(x.MetaDescription)).WithMessage("META описание слишком короткое.");
            
            RuleFor(x => x.UrlSlug)
                .MaximumLength(200).WithMessage("URL slug не должен превышать 200 символов.")
                .Matches(@"^[a-z0-9-]+$").When(x => !string.IsNullOrEmpty(x.UrlSlug))
                .WithMessage("URL slug должен содержать только строчные буквы, цифры и дефис.");
            
            RuleFor(x => x.Priority)
                .InclusiveBetween(0, 10).WithMessage("Приоритет должен быть в диапазоне от 0 до 10.");

            RuleFor(x => x.OgImage)
                .MaximumLength(500).WithMessage("Ссылка на OG изображение слишком длинная.");
        }
    }
}
