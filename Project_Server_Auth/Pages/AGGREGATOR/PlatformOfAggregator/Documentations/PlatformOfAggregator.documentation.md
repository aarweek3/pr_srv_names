# Техническая документация: PlatformOfAggregator (SEO Edition)

Модуль управления платформами (ОС) в Агрегаторе. Реализован согласно «Образцовой модели SEO», обеспечивая поддержку многоязычности и продвижения контента.

## 📂 Структура файлов

| Слой | Путь к файлу |
| :--- | :--- |
| **Model (DAL)** | [PlatformOfAggregator.cs](file:///d:/_PROGECT/pr_srv_names/DAL/Models/Aggregator/PlatformOfAggregator.cs) |
| **Localization** | [PlatformOfAggregatorLocalization.cs](file:///d:/_PROGECT/pr_srv_names/DAL/Models/Aggregator/Localizations/PlatformOfAggregatorLocalization.cs) |
| **Repository** | [IPlatformOfAggregatorRepository.cs](file:///d:/_PROGECT/pr_srv_names/DAL/Repositories/Interfaces/IPlatformOfAggregatorRepository.cs) |
| **Service** | [PlatformOfAggregatorService.cs](file:///d:/_PROGECT/pr_srv_names/Project_Server_Auth/Pages/AGGREGATOR/PlatformOfAggregator/Services/PlatformOfAggregatorService.cs) |
| **DTO** | [PlatformOfAggregatorDto.cs](file:///d:/_PROGECT/pr_srv_names/Project_Server_Auth/Pages/AGGREGATOR/PlatformOfAggregator/Dtos/PlatformOfAggregatorDto.cs) |
| **Controller** | [PlatformOfAggregatorController.cs](file:///d:/_PROGECT/pr_srv_names/Project_Server_Auth/Controllers/PlatformOfAggregatorController.cs) |

## 🛠 Архитектурные особенности

### 1. SEO и Локализация
Каждая платформа поддерживает неограниченное количество языковых версий. Для каждого языка доступны:
- **Контент**: Название, краткое описание и HTML-текст (для лендингов платформ).
- **SEO-метаданные**: Полный набор полей `SeoData` (Title, Description, OG, Canonical и т.д.).
- **Изображение**: Возможность задать локализованную иконку/баннер (`UrlPicture`).

### 2. Статистика (ProgramsCount)
В списке платформ автоматически вычисляется поле `ProgramsCount`. Это количество программ, связанных с данной платформой. Расчет происходит на уровне БД через `Include` и `MapFrom`, что гарантирует высокую производительность.

### 3. Навигация и URL
- **SystemCode**: Уникальный код платформы (например, `windows`, `macos`), используемый в URL и для технической идентификации.
- **SortOrder**: Определяет порядок отображения в меню и списках на фронтенде.

## 📡 API Endpoints (v1)

| Метод | Путь | Описание |
| :--- | :--- | :--- |
| **GET** | `/api/v1/aggregator/platforms` | Получить список с пагинацией и поиском. |
| **GET** | `/api/v1/aggregator/platforms/{id}` | Детальная информация со всеми переводами. |
| **POST** | `/api/v1/aggregator/platforms` | Создание новой платформы. |
| **PUT** | `/api/v1/aggregator/platforms/{id}` | Обновление платформы и SEO-данных. |
| **DELETE** | `/api/v1/aggregator/platforms/{id}` | Удаление (включая каскадное удаление SEO). |

## 🧪 Валидация
Валидация реализована через **FluentValidation**. Проверяются:
- Уникальность `Name` и `SystemCode`.
- Обязательное наличие перевода хотя бы для одного языка (при создании).
- Максимальная длина строк согласно `PlatformConstants`.

---

## Чек-лист завершения (Phase 3)
- [x] Репозиторий DAL обновлен.
- [x] Сервисный слой реализован с поддержкой CorrelationId.
- [x] Настроен маппинг в папке `Mappings`.
- [x] Подключена валидация в папке `Validators`.
- [x] Контроллер готов к интеграции с UI.
