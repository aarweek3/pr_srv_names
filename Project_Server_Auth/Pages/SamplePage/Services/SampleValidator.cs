using FluentValidation;
using pr_srv_names.Pages.Sample.Dtos;
using pr_srv_names.Pages.Sample.Extensions;

namespace pr_srv_names.Pages.Sample.Services
{
    /// <summary>
    /// Валидатор для SampleCreateRequestDto.
    /// </summary>
    public class SampleCreateRequestDtoValidator : AbstractValidator<SampleCreateRequestDto>
    {
        public SampleCreateRequestDtoValidator()
        {
            RuleFor(x => x.Name).ValidateSampleName();

            RuleFor(x => x.Description)
                .ValidateSampleDescription()
                .When(x => !string.IsNullOrWhiteSpace(x.Description));
        }
    }

    /// <summary>
    /// Валидатор для SampleUpdateRequestDto.
    /// </summary>
    public class SampleUpdateRequestDtoValidator : AbstractValidator<SampleUpdateRequestDto>
    {
        public SampleUpdateRequestDtoValidator()
        {
            RuleFor(x => x.Id).ValidateSampleId();
            RuleFor(x => x.Name).ValidateSampleName();

            RuleFor(x => x.Description)
                .ValidateSampleDescription()
                .When(x => !string.IsNullOrWhiteSpace(x.Description));
        }
    }

    /// <summary>
    /// Валидатор для SampleDetailDto.
    /// </summary>
    public class SampleDetailDtoValidator : AbstractValidator<SampleDetailDto>
    {
        public SampleDetailDtoValidator()
        {
            RuleFor(x => x.Id).ValidateSampleId();
            RuleFor(x => x.Name).ValidateSampleName();

            RuleFor(x => x.Description)
                .ValidateSampleDescription()
                .When(x => !string.IsNullOrWhiteSpace(x.Description));
        }
    }

    /// <summary>
    /// Валидатор для SamplePageRequestDto.
    /// </summary>
    public class SamplePageRequestDtoValidator : AbstractValidator<SamplePageRequestDto>
    {
        public SamplePageRequestDtoValidator()
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