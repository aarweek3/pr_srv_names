using FluentValidation;
using pr_srv_names.Pages.AGGREGATOR.CategoryTagOfAggregator.Dtos;
using pr_srv_names.Pages.AGGREGATOR.TagOfAggregator.Dtos;

namespace pr_srv_names.Pages.AGGREGATOR.TagOfAggregator.Validators
{
    /// <summary>
    /// Валидатор для создания категории тегов.
    /// </summary>
    public class CategoryTagOfAggregatorCreateValidator : AbstractValidator<CategoryTagOfAggregatorCreateDto>
    {
        public CategoryTagOfAggregatorCreateValidator()
        {
            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug обязателен для заполнения")
                .MaximumLength(100).WithMessage("Slug не может превышать 100 символов")
                .Matches(@"^[a-z0-9-]+$").WithMessage("Slug может содержать только строчные латинские буквы, цифры и дефис");

            RuleFor(x => x.Localizations)
                .NotEmpty().WithMessage("Должна быть хотя бы одна локализация");

            RuleForEach(x => x.Localizations).ChildRules(loc =>
            {
                loc.RuleFor(l => l.Name)
                    .NotEmpty().WithMessage("Название в локализации обязательно")
                    .MaximumLength(100).WithMessage("Название не может превышать 100 символов");
                
                loc.RuleFor(l => l.LanguageOfAggregatorId)
                    .GreaterThan(0).WithMessage("ID языка должен быть указан");
            });
        }
    }

    /// <summary>
    /// Валидатор для обновления категории тегов.
    /// </summary>
    public class CategoryTagOfAggregatorUpdateValidator : AbstractValidator<CategoryTagOfAggregatorUpdateDto>
    {
        public CategoryTagOfAggregatorUpdateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            
            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug обязателен для заполнения")
                .MaximumLength(100).WithMessage("Slug не может превышать 100 символов")
                .Matches(@"^[a-z0-9-]+$").WithMessage("Slug может содержать только строчные латинские буквы, цифры и дефис");

            RuleFor(x => x.Localizations)
                .NotEmpty().WithMessage("Должна быть хотя бы одна локализация");
        }
    }

    /// <summary>
    /// Валидатор для создания тега.
    /// </summary>
    public class TagOfAggregatorCreateValidator : AbstractValidator<TagOfAggregatorCreateDto>
    {
        public TagOfAggregatorCreateValidator()
        {
            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug обязателен для заполнения")
                .MaximumLength(100).WithMessage("Slug не может превышать 100 символов")
                .Matches(@"^[a-z0-9-]+$").WithMessage("Slug может содержать только строчные латинские буквы, цифры и дефис");

            RuleFor(x => x.CategoryTagId)
                .GreaterThan(0).WithMessage("Категория тега должна быть указана");

            RuleFor(x => x.Localizations)
                .NotEmpty().WithMessage("Должна быть хотя бы одна локализация");

            RuleForEach(x => x.Localizations).ChildRules(loc =>
            {
                loc.RuleFor(l => l.Name)
                    .NotEmpty().WithMessage("Название в локализации обязательно")
                    .MaximumLength(100).WithMessage("Название не может превышать 100 символов");
            });
        }
    }

    /// <summary>
    /// Валидатор для обновления тега.
    /// </summary>
    public class TagOfAggregatorUpdateValidator : AbstractValidator<TagOfAggregatorUpdateDto>
    {
        public TagOfAggregatorUpdateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            
            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug обязателен для заполнения");

            RuleFor(x => x.CategoryTagId)
                .GreaterThan(0).WithMessage("Категория тега должна быть указана");
        }
    }
}
