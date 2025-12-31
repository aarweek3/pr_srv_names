using FluentValidation;
using pr_srv_names.Pages.UserSetting.Dtos;

namespace pr_srv_names.Pages.UserSetting.Validators
{
    /// <summary>
    /// Валидатор для UserSettingsUpdateDto.
    /// </summary>
    public class UserSettingsUpdateDtoValidator : AbstractValidator<UserSettingsUpdateDto>
    {
        public UserSettingsUpdateDtoValidator()
        {
            // 1. ВНЕШНИЙ ВИД (UI / UX)
            RuleFor(x => x.Theme)
                .IsInEnum()
                .WithMessage("Недопустимое значение для темы интерфейса.");

            RuleFor(x => x.Density)
                .IsInEnum()
                .WithMessage("Недопустимое значение для плотности интерфейса.");

            RuleFor(x => x.PrimaryColor)
                .Matches(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")
                .WithMessage("Цвет должен быть в формате HEX (#RRGGBB или #RGB).")
                .When(x => !string.IsNullOrWhiteSpace(x.PrimaryColor));

            // 2. НАВИГАЦИЯ И LAYOUT
            RuleFor(x => x.SidebarState)
                .IsInEnum()
                .WithMessage("Недопустимое значение для состояния сайдбара.");

            RuleFor(x => x.NavigationBehavior)
                .IsInEnum()
                .WithMessage("Недопустимое значение для поведения навигации.");

            // 3. СПИСКИ И ТАБЛИЦЫ
            RuleFor(x => x.TableDensity)
                .IsInEnum()
                .WithMessage("Недопустимое значение для плотности таблиц.");

            RuleFor(x => x.DefaultPageSize)
                .IsInEnum()
                .WithMessage("Недопустимое значение для размера страницы.");

            // 4. ЛОКАЛИЗАЦИЯ
            RuleFor(x => x.Language)
                .NotEmpty()
                .WithMessage("Язык обязателен.")
                .MaximumLength(10)
                .WithMessage("Код языка не может превышать 10 символов.")
                .Matches(@"^[a-z]{2}-[A-Z]{2}$")
                .WithMessage("Язык должен быть в формате 'ru-RU' или 'en-US'.");

            RuleFor(x => x.TimeZone)
                .NotEmpty()
                .WithMessage("Часовой пояс обязателен.")
                .MaximumLength(50)
                .WithMessage("Часовой пояс не может превышать 50 символов.");

            // 5. ДОСТУПНОСТЬ
            RuleFor(x => x.AccessibilityLevel)
                .IsInEnum()
                .WithMessage("Недопустимое значение для уровня доступности.");

            // 6. УВЕДОМЛЕНИЯ
            RuleFor(x => x.NotificationLevel)
                .IsInEnum()
                .WithMessage("Недопустимое значение для уровня уведомлений.");

            // NotificationChannels - это Flags enum, проверяем что значение валидно
            RuleFor(x => x.NotificationChannels)
                .Must(BeValidNotificationChannels)
                .WithMessage("Недопустимое значение для каналов уведомлений.");

            // 7. БЕЗОПАСНОСТЬ И ПОВЕДЕНИЕ
            RuleFor(x => x.SessionTerminationMode)
                .IsInEnum()
                .WithMessage("Недопустимое значение для режима завершения сессии.");

            RuleFor(x => x.LoginNotificationMode)
                .IsInEnum()
                .WithMessage("Недопустимое значение для режима уведомлений о входе.");
        }

        /// <summary>
        /// Валидация Flags enum для NotificationChannels
        /// </summary>
        private bool BeValidNotificationChannels(DAL.Enums.Settings.NotificationChannel channels)
        {
            // Проверяем, что значение не выходит за пределы всех возможных флагов
            var allFlags = DAL.Enums.Settings.NotificationChannel.Email |
                          DAL.Enums.Settings.NotificationChannel.InApp |
                          DAL.Enums.Settings.NotificationChannel.Sms |
                          DAL.Enums.Settings.NotificationChannel.Push;

            return (channels & ~allFlags) == 0;
        }
    }
}
