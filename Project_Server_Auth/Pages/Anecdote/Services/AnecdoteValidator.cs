using FluentValidation;
using pr_srv_names.Pages.Anecdote.Dtos;
using pr_srv_names.Pages.Anecdote.Extensions;

namespace pr_srv_names.Pages.Anecdote.Services
{
    /// <summary>
    /// Валидатор для AnecdoteCreateRequestDto
    /// Проверяет корректность данных при создании новой анекдоты.
    /// </summary>
    public class AnecdoteCreateRequestDtoValidator : AbstractValidator<AnecdoteCreateRequestDto>
    {
        public AnecdoteCreateRequestDtoValidator()
        {
            // Проверяет имя анекдоты на соответствие заданным правилам (например, длину или формат).
            RuleFor(x => x.Name).ValidateAnecdoteName();

            // Проверяет описание анекдоты, если оно предоставлено, используя кастомные правила.
            // Валидация выполняется только для непустых значений, включая строки с пробелами.
            RuleFor(x => x.Description)
                .ValidateAnecdoteDescription()
                .When(x => !string.IsNullOrWhiteSpace(x.Description));

            // Проверяет идентификатор основного имени (NameMainId) на соответствие заданным требованиям.
            RuleFor(x => x.NameMainId).ValidateNameMainId();

            // Проверяет идентификатор языка (LanguageId) на соответствие заданным требованиям.
            RuleFor(x => x.LanguageId).ValidateLanguageId();
        }
    }

    /// <summary>
    /// Валидатор для AnecdoteUpdateRequestDto
    /// Проверяет корректность данных при обновлении существующей анекдоты.
    /// </summary>
    public class AnecdoteUpdateRequestDtoValidator : AbstractValidator<AnecdoteUpdateRequestDto>
    {
        public AnecdoteUpdateRequestDtoValidator()
        {
            // Проверяет идентификатор анекдоты на соответствие заданным правилам.
            RuleFor(x => x.Id).ValidateAnecdoteId();

            // Проверяет имя анекдоты на соответствие заданным правилам (например, длину или формат).
            RuleFor(x => x.Name).ValidateAnecdoteName();

            // Проверяет описание анекдоты, если оно предоставлено, используя кастомные правила.
            // Валидация выполняется только для непустых значений, включая строки с пробелами.
            RuleFor(x => x.Description)
                .ValidateAnecdoteDescription()
                .When(x => !string.IsNullOrWhiteSpace(x.Description));

            // Проверяет идентификатор основного имени (NameMainId) на соответствие заданным требованиям.
            RuleFor(x => x.NameMainId).ValidateNameMainId();

            // Проверяет идентификатор языка (LanguageId) на соответствие заданным требованиям.
            RuleFor(x => x.LanguageId).ValidateLanguageId();
        }
    }

    /// <summary>
    /// Валидатор для AnecdoteDetailDto
    /// Проверяет корректность данных детального представления анекдоты.
    /// </summary>
    public class AnecdoteDetailDtoValidator : AbstractValidator<AnecdoteDetailDto>
    {
        public AnecdoteDetailDtoValidator()
        {
            // Проверяет идентификатор анекдоты на соответствие заданным правилам.
            RuleFor(x => x.Id).ValidateAnecdoteId();

            // Проверяет имя анекдоты на соответствие заданным правилам (например, длину или формат).
            RuleFor(x => x.Name).ValidateAnecdoteName();

            // Проверяет описание анекдоты, если оно предоставлено, используя кастомные правила.
            // Валидация выполняется только для непустых значений, включая строки с пробелами.
            RuleFor(x => x.Description)
                .ValidateAnecdoteDescription()
                .When(x => !string.IsNullOrWhiteSpace(x.Description));

            // Проверяет идентификатор основного имени (NameMainId) на соответствие заданным требованиям.
            RuleFor(x => x.NameMainId).ValidateNameMainId();

            // Проверяет идентификатор языка (LanguageId) на соответствие заданным требованиям.
            RuleFor(x => x.LanguageId).ValidateLanguageId();
        }
    }

    /// <summary>
    /// Валидатор для AnecdotePageRequestDto
    /// Проверяет корректность данных запроса постраничного списка анекдот.
    /// </summary>
    public class AnecdotePageRequestDtoValidator : AbstractValidator<AnecdotePageRequestDto>
    {
        public AnecdotePageRequestDtoValidator()
        {
            // Проверяет номер страницы на соответствие заданным правилам (например, положительное число).
            RuleFor(x => x.PageNumber).ValidatePageNumber();

            // Проверяет размер страницы на соответствие заданным правилам (например, допустимый диапазон).
            RuleFor(x => x.PageSize).ValidatePageSize();

            // Проверяет термин поиска, если он предоставлен, используя кастомные правила.
            // Валидация выполняется только для непустых значений, включая строки с пробелами.
            RuleFor(x => x.SearchTerm)
                .ValidateSearchTerm()
                .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm));

            // Проверяет идентификатор основного имени (NameMainId), если он предоставлен.
            // Должно быть положительным числом.
            RuleFor(x => x.NameMainId)
                .GreaterThan(0)
                .WithMessage("Идентификатор имени (NameMainId) должен быть положительным.")
                .When(x => x.NameMainId.HasValue);

            // Проверяет идентификатор языка (LanguageId), если он предоставлен.
            // Должно быть положительным числом.
            RuleFor(x => x.LanguageId)
                .GreaterThan(0)
                .WithMessage("Идентификатор языка (LanguageId) должен быть положительным.")
                .When(x => x.LanguageId.HasValue);

            // Проверяет поле сортировки (SortBy) на принадлежность перечислению (enum).
            RuleFor(x => x.SortBy)
                .IsInEnum()
                .WithMessage("Недопустимое значение для поля сортировки.");

            // Проверяет направление сортировки (SortDirection) на принадлежность перечислению (enum).
            RuleFor(x => x.SortDirection)
                .IsInEnum()
                .WithMessage("Недопустимое значение для направления сортировки.");
        }
    }
}