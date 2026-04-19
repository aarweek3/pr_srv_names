using FluentValidation;
using Project_Server_Auth.Pages.CategoryRepository.Dtos;

namespace Project_Server_Auth.Pages.CategoryRepository.Validators
{
    public class IconCategoryCreateValidator : AbstractValidator<IconCategoryCreateDto>
    {
        public IconCategoryCreateValidator()
        {
            RuleFor(x => x.DisplayName)
                .NotEmpty().WithMessage("Отображаемое имя обязательно")
                .MaximumLength(100).WithMessage("Имя не должно превышать 100 символов");

            RuleFor(x => x.FolderName)
                .NotEmpty().WithMessage("Имя папки/раздела обязательно")
                .MaximumLength(100).WithMessage("Техническое имя не должно превышать 100 символов");
        }
    }

    public class IconCategoryUpdateValidator : AbstractValidator<IconCategoryUpdateDto>
    {
        public IconCategoryUpdateValidator()
        {
            RuleFor(x => x.DisplayName)
                .NotEmpty().WithMessage("Отображаемое имя обязательно")
                .MaximumLength(100).WithMessage("Имя не должно превышать 100 символов");
        }
    }
}
