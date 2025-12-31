Чек-лист реализации UserSettings
1. Уровень базы данных (DAL) ✅
 ✅ Контекст БД: Добавить DbSet<UserSettings> UserSettings в ApplicationDbContext.cs.
 ✅ Конфигурация: Настроить связь "Один-к-Одному" с ApplicationUser и каскадное удаление в OnModelCreating.
 ✅ Миграция: Создать и применить миграцию (Add-Migration AddUserSettings, Update-Database).
2. DTO и Маппинг ✅
 ✅ DTO: Создать UserSettingsDetailDto.cs (для чтения данных).
 ✅ DTO: Создать UserSettingsUpdateDto.cs (для обновления настроек).
 ✅ Маппинг: Создать папку Map и там создать класс для ручного маппинга UserSettingsMapper.cs.
3. Репозиторий (Repository) ✅
 ✅ Интерфейс: Создать IUserSettingsRepository.cs (методы: GetByUserIdAsync, AddAsync и др.).
 ✅ Реализация: Написать UserSettingsRepository.cs.
 ✅ IoC: Зарегистрировать репозиторий в DI контейнере (Program.cs).
 ✅ UnitOfWork: Добавить в IUnitOfWork и UnitOfWork.
4. Сервис (Service Layer) ✅
 ✅ Валидация: Написать валидатор FluentValidation для UserSettingsUpdateDto.
 ✅ Интерфейс: Создать IUserSettingsService.cs.
 ✅ Реализация: Написать UserSettingsService.cs:
✅ Внедрение зависимостей (Logger, Mapper, UnitOfWork, Repository, Validator).
✅ Реализация метода GetSettingsAsync (c CorrelationId и логированием).
✅ Реализация метода UpdateSettingsAsync (c валидацией и транзакцией).
 ✅ IoC: Зарегистрировать сервис и валидаторы в DI (ServicesExtensions.cs).
5. Интеграция (регистрация пользователя) ✅
 ✅ Обновление регистрации: Добавить создание дефолтных настроек в метод регистрации пользователя (AuthService.RegisterAsync).
6. API (Controller) ✅
 ✅ Контроллер: Создать SettingsController.cs.
 ✅ Эндпоинты:
✅ GET /api/settings — Получение настроек текущего пользователя.
✅ PUT /api/settings — Обновление настроек.
✅ PATCH /api/settings — Частичное обновление настроек.
✅ POST /api/settings/reset — Сброс к дефолтным значениям.
✅ GET /api/settings/user/{userId} — Получение настроек по UserId (Admin).
✅ GET /api/settings/exists — Проверка существования настроек.
 ✅ Swagger: Добавить XML-комментарии и атрибуты для документации.