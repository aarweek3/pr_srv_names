using FluentValidation;
using pr_srv_names.Pages.Language.Dtos;
using pr_srv_names.Pages.Language.Extensions;

namespace pr_srv_names.Pages.Language.Services
{
    /// <summary>
    /// Валидатор для LanguageCreateRequestDto.
    /// </summary>
    public class LanguageCreateRequestDtoValidator : AbstractValidator<LanguageCreateRequestDto>
    {
        public LanguageCreateRequestDtoValidator()
        {
            RuleFor(x => x.Code).ValidateLanguageCode();
            RuleFor(x => x.Name).ValidateLanguageName();
            RuleFor(x => x.Description)
                .ValidateLanguageDescription()
                .When(x => !string.IsNullOrWhiteSpace(x.Description));
            RuleFor(x => x.FlagCode)
                .ValidateFlagCode()
                .When(x => !string.IsNullOrWhiteSpace(x.FlagCode));
        }
    }

    /// <summary>
    /// Валидатор для LanguageUpdateRequestDto.
    /// </summary>
    public class LanguageUpdateRequestDtoValidator : AbstractValidator<LanguageUpdateRequestDto>
    {
        public LanguageUpdateRequestDtoValidator()
        {
            RuleFor(x => x.Id).ValidateLanguageId();
            RuleFor(x => x.Code).ValidateLanguageCode();
            RuleFor(x => x.Name).ValidateLanguageName();
            RuleFor(x => x.Description)
                .ValidateLanguageDescription()
                .When(x => !string.IsNullOrWhiteSpace(x.Description));
            RuleFor(x => x.FlagCode)
                .ValidateFlagCode()
                .When(x => !string.IsNullOrWhiteSpace(x.FlagCode));
        }
    }

    /// <summary>
    /// Валидатор для LanguageDetailDto.
    /// </summary>
    public class LanguageDetailDtoValidator : AbstractValidator<LanguageDetailDto>
    {
        public LanguageDetailDtoValidator()
        {
            RuleFor(x => x.Id).ValidateLanguageId();
            RuleFor(x => x.Code).ValidateLanguageCode();
            RuleFor(x => x.Name).ValidateLanguageName();
            RuleFor(x => x.Description)
                .ValidateLanguageDescription()
                .When(x => !string.IsNullOrWhiteSpace(x.Description));
            RuleFor(x => x.FlagCode)
                .ValidateFlagCode()
                .When(x => !string.IsNullOrWhiteSpace(x.FlagCode));
        }
    }

    /// <summary>
    /// Валидатор для LanguagePageRequestDto.
    /// </summary>
    public class LanguagePageRequestDtoValidator : AbstractValidator<LanguagePageRequestDto>
    {
        public LanguagePageRequestDtoValidator()
        {
            RuleFor(x => x.PageNumber).ValidatePageNumber();
            RuleFor(x => x.PageSize).ValidatePageSize();
            RuleFor(x => x.SearchTerm)
                .ValidateSearchTerm()
                .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm));
            RuleFor(x => x.SortBy)
                .IsInEnum()
                .WithMessage("Недопустимое значение для поля сортировки.");
            RuleFor(x => x.SortDirection)
                .IsInEnum()
                .WithMessage("Недопустимое значение для направления сортировки.");
        }
    }
}