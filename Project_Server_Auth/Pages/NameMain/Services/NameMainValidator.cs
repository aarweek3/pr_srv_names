using FluentValidation;
using pr_srv_names.Pages.NameMain.Dtos;
using pr_srv_names.Pages.NameMain.Extensions;

namespace pr_srv_names.Pages.NameMain.Services
{
    /// <summary>
    /// Валидатор для NameMainCreateRequestDto.
    /// </summary>
    public class NameMainCreateRequestDtoValidator : AbstractValidator<NameMainCreateRequestDto>
    {
        public NameMainCreateRequestDtoValidator()
        {
            RuleFor(x => x.Name).ValidateNameMainName();

            RuleFor(x => x.Description)
                .ValidateNameMainDescription()
                .When(x => !string.IsNullOrWhiteSpace(x.Description));
        }
    }

    /// <summary>
    /// Валидатор для NameMainUpdateRequestDto.
    /// </summary>
    public class NameMainUpdateRequestDtoValidator : AbstractValidator<NameMainUpdateRequestDto>
    {
        public NameMainUpdateRequestDtoValidator()
        {
            RuleFor(x => x.Id).ValidateNameMainId();
            RuleFor(x => x.Name).ValidateNameMainName();

            RuleFor(x => x.Description)
                .ValidateNameMainDescription()
                .When(x => !string.IsNullOrWhiteSpace(x.Description));
        }
    }

    /// <summary>
    /// Валидатор для NameMainDetailDto.
    /// </summary>
    public class NameMainDetailDtoValidator : AbstractValidator<NameMainDetailDto>
    {
        public NameMainDetailDtoValidator()
        {
            RuleFor(x => x.Id).ValidateNameMainId();
            RuleFor(x => x.Name).ValidateNameMainName();

            RuleFor(x => x.Description)
                .ValidateNameMainDescription()
                .When(x => !string.IsNullOrWhiteSpace(x.Description));
        }
    }

    /// <summary>
    /// Валидатор для NameMainPageRequestDto.
    /// </summary>
    public class NameMainPageRequestDtoValidator : AbstractValidator<NameMainPageRequestDto>
    {
        public NameMainPageRequestDtoValidator()
        {
            RuleFor(x => x.PageNumber).ValidatePageNumber();
            RuleFor(x => x.PageSize).ValidatePageSize();

            RuleFor(x => x.SearchTerm)
                .ValidateSearchTerm()
                .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm));

            // Enum'ы автоматически валидируются на уровне десериализации
            // Добавляем явную валидацию для большей ясности
            RuleFor(x => x.SortBy)
                .IsInEnum()
                .WithMessage("Недопустимое значение для поля сортировки.");

            RuleFor(x => x.SortDirection)
                .IsInEnum()
                .WithMessage("Недопустимое значение для направления сортировки.");
        }
    }
}