# План реализации: Система управления тегами (Aurora v3.5)

Этот план описывает шаги по внедрению надежной, локализованной и иерархической системы тегов (CategoryTag и Tag), как указано в `tag.md` и на основе "Золотого эталона" из `developer-help.component.ts`.

## Статус выполнения проекта

| Этап | Что надо сделать | Статус (Сделано) |
| :--- | :--- | :--- |
| **1. DAL** | Создание моделей `CategoryTag` и `Tag`, настройка индексов в БД | **Сделано** (созданы модели, Enums и конфигурация) |
| **2. Backend Logic** | Реализация DTO, AutoMapper и сервисов (`TagService`) | В процессе |
| **3. Frontend State** | Разработка Store/Service на Angular Signals | Ожидание |
| **4. Frontend UI** | Создание компонентов списка и форм управления | Ожидание |

## Детальный план изменений

### 1. Backend: Уровень доступа к данным (DAL)

| Файл / Задача | Описание изменений | Статус |
| :--- | :--- | :--- |
| [CategoryTagOfAggregator.cs](file:///d:/_PROGECT/pr_srv_names/DAL/Models/Aggregator/CategoryTagOfAggregator.cs) | Поля: Slug, IconPath, Color, SortOrder. Наследование от FullAuditable. | **Сделано** |
| [CategoryTagOfAggregatorLocalization.cs](file:///d:/_PROGECT/pr_srv_names/DAL/Models/Aggregator/Localizations/CategoryTagOfAggregatorLocalization.cs) | Поля: Name, Description. | **Сделано** |
| [TagOfAggregator.cs](file:///d:/_PROGECT/pr_srv_names/DAL/Models/Aggregator/TagOfAggregator.cs) | Добавлены CategoryTagId, Color, IconPath, IsFeature, TagType. | **Сделано** |
| [TagOfAggregatorLocalization.cs](file:///d:/_PROGECT/pr_srv_names/DAL/Models/Aggregator/Localizations/TagOfAggregatorLocalization.cs) | Добавлены SEO-поля: MetaTitle, MetaDescription, H1Title. | **Сделано** |
| [AggregatorModelConfiguration.cs](file:///d:/_PROGECT/pr_srv_names/DAL/Configurations/AggregatorModelConfiguration.cs) | Настройка Fluent API, индексов Slug и правил удаления. | **Сделано** |

---

### 2. Backend: Сервисный уровень и API

| Файл / Задача | Описание изменений | Статус |
| :--- | :--- | :--- |
| **DTOs** | Созданы модели данных для API (Item, Detail, Create, Update, Localization). | **Сделано** |
| **Mappings** | Реализована логика наследования свойств от категории в AutoMapper (`DisplayColor`, `DisplayIcon`). | **Сделано** |
| **Interfaces** | Описаны контракты `ITagOfAggregatorService` и `ICategoryTagOfAggregatorService`. | **Сделано** |
| [CategoryTagService.cs](file:///d:/_PROGECT/pr_srv_names/Project_Server_Auth/Pages/AGGREGATOR/CategoryTagOfAggregator/Services/CategoryTagOfAggregatorService.cs) | Базовый CRUD и защита от удаления категорий с тегами. | **Сделано** |
| [TagService.cs](file:///d:/_PROGECT/pr_srv_names/Project_Server_Auth/Pages/AGGREGATOR/TagOfAggregator/Services/TagService.cs) | Реализация сквозного поиска, логики наследования и статуса перевода. | **Сделано** |
| **Controllers** | `TagOfAggregatorController` и `CategoryTagOfAggregatorController`. | **Сделано** |
| [Validators](file:///d:/_PROGECT/pr_srv_names/Project_Server_Auth/Pages/AGGREGATOR/TagOfAggregator/Validators/TagOfAggregatorValidators.cs) | FluentValidation для DTO (Standard v3.5). | В процессе |
| **Maintenance** | Методы `SeedFromJson` и `Clear` в контроллерах и сервисах. | В процессе |
| [tags-seed.json](file:///d:/_PROGECT/pr_srv_names/Project_Server_Auth/Pages/AGGREGATOR/TagOfAggregator/Jsons/tags-seed.json) | Файл с эталонными данными для инициализации. | **Сделано** |

---

### 3. Frontend: Состояние и UI

| Задача | Описание | Статус |
| :--- | :--- | :--- |
| **tag-state.service.ts** | Управление сигналами, пагинацией и поиском. | Надо сделать |
| **TagManagerComponent** | Интерфейс списка с фильтрами и цветными индикаторами. | Надо сделать |
| **TagFormComponent** | Форма редактирования с вводом HEX и загрузкой SVG. | Надо сделать |

---

## Решенные технические вопросы

- [x] **Цветовые пресеты**: Разрешен свободный ввод HEX-кодов.
- [x] **Иконки**: Реализована поддержка загрузки пользовательских SVG с шардированием путей.
