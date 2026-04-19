# Руководство: Sample Multi-Lang

**Sample Multi-Lang** — это эталонная реализация менеджера сущностей с полной поддержкой многоязычности (Localization). Данный модуль демонстрирует, как в системе Aurora Admin реализовано управление контентом на разных языках.

## 1. Архитектура модуля

Модуль построен на паттерне **State-Service**, что обеспечивает четкое разделение логики отображения, управления состоянием и API-запросов.

*   **SampleMainManagerComponent**: Основной контейнер. Содержит селектор языка, поиск и пагинацию.
*   **SampleMainStateService**: Хранилище состояния в стиле RxJS. Управляет загрузкой данных, фильтрацией по языку и модальными окнами.
*   **SampleMainApiService**: Инкапсулирует HTTP-запросы к бэкенду.

## 2. Как работает многоязычность

Система Aurora использует централизованный список языков, который управляется через `LanguageService`.

**Процесс фильтрации по языку:**
1. При загрузке страницы выбирается язык по умолчанию (`isDefault`).
2. ID выбранного языка передается в `SampleMainStateService.setLanguage()`.
3. Бэкенд-эндпоинт `GET /api/v1/samples-main` принимает `languageId` в качестве параметра запроса и возвращает переводы именно для этого языка.

## 3. CRUD операции (Создание, Редактирование, Удаление)

*   **Создание**: При добавлении новой записи открывается `SampleMainModalComponent`. Можно вводить текст на текущем выбранном языке.
*   **Редактирование**: При открытии записи на редактирование бэкенд возвращает `SampleMainDetailDto`, который содержит данные для всех доступных языков. Модальное окно позволяет переключаться между вкладками языков для редактирования конкретных переводов.
*   **Удаление**: Удаление основной записи влечет за собой каскадное удаление всех её переводов в базе данных.

## 4. Бэкенд-реализация

На стороне сервера за логику отвечает `SampleMainController`. Ключевые особенности:

*   **DTO**: Используются специализированные объекты для передачи только необходимых данных (`PagedResponse`, `DetailDto`, `CreateRequest`).
*   **Валидация**: Техническое имя (Code) должно быть уникальным. Все переводы проверяются на корректность.
*   **Логирование**: Каждая операция сопровождается `CorrelationId` для удобства отладки.

---

> [!TIP]
> **Для разработчиков**: Данный модуль является шаблоном. Если вам нужно создать новый справочник с поддержкой языков, рекомендуется копировать архитектуру именно из этих файлов.

## 📂 Пути к исходному коду

### Frontend (Angular)
| Компонент | Путь к файлу | Действие |
| :--- | :--- | :--- |
| **Manager** | `D:\_PROGECT\pr_aurora_admin\src\app\pages\sample-manager-simple-language\components\sample-main-manager\sample-main-manager.component.ts` | [📂 Открыть](file:///D:/_PROGECT/pr_aurora_admin/src/app/pages/sample-manager-simple-language/components/sample-main-manager/sample-main-manager.component.ts) |
| **State** | `D:\_PROGECT\pr_aurora_admin\src\app\pages\sample-manager-simple-language\services\sample-main-state.service.ts` | [📂 Открыть](file:///D:/_PROGECT/pr_aurora_admin/src/app/pages/sample-manager-simple-language/services/sample-main-state.service.ts) |
| **API** | `D:\_PROGECT\pr_aurora_admin\src\app\pages\sample-manager-simple-language\services\sample-main-api.service.ts` | [📂 Открыть](file:///D:/_PROGECT/pr_aurora_admin/src/app/pages/sample-manager-simple-language/services/sample-main-api.service.ts) |

### Backend (.NET)
| Элемент | Путь к файлу | Действие |
| :--- | :--- | :--- |
| **Controller** | `D:\_PROGECT\pr_srv_names\Project_Server_Auth\Controllers\SampleMainController.cs` | [📂 Открыть](file:///D:/_PROGECT/pr_srv_names/Project_Server_Auth/Controllers/SampleMainController.cs) |
| **Service** | `D:\_PROGECT\pr_srv_names\Project_Server_Auth\Pages\SampleMain\Services\SampleMainService.cs` | [📂 Открыть](file:///D:/_PROGECT/pr_srv_names/Project_Server_Auth/Pages/SampleMain/Services/SampleMainService.cs) |
| **Interface** | `D:\_PROGECT\pr_srv_names\Project_Server_Auth\Pages\SampleMain\Interfaces\ISampleMainService.cs` | [📂 Открыть](file:///D:/_PROGECT/pr_srv_names/Project_Server_Auth/Pages/SampleMain/Interfaces/ISampleMainService.cs) |
