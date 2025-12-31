# UserSettings - Полная спецификация Backend + Frontend ТЗ

## 📋 ЧАСТЬ 1: BACKEND (РЕАЛИЗОВАНО)

### 1.1 Модель данных (Entity)

```csharp
// DAL/Models/UserSettings.cs
public class UserSettings : IUserSettings
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string UserId { get; set; } = string.Empty;
    
    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser? User { get; set; }

    // 1. ВНЕШНИЙ ВИД (UI / UX)
    public UiTheme Theme { get; set; } = UiTheme.System;
    public UiDensity Density { get; set; } = UiDensity.Comfortable;
    [MaxLength(10)]
    public string? PrimaryColor { get; set; }

    // 2. НАВИГАЦИЯ И LAYOUT
    public SidebarState SidebarState { get; set; } = SidebarState.Expanded;
    public NavigationBehavior NavigationBehavior { get; set; } = NavigationBehavior.RememberLastPage;

    // 3. СПИСКИ И ТАБЛИЦЫ
    public TableDensity TableDensity { get; set; } = TableDensity.Normal;
    public DefaultPageSizeOption DefaultPageSize { get; set; } = DefaultPageSizeOption.Size10;
    public bool ShowAdvancedFilters { get; set; } = false;

    // 4. ЛОКАЛИЗАЦИЯ
    [MaxLength(10)]
    public string Language { get; set; } = "ru-RU";
    [MaxLength(50)]
    public string TimeZone { get; set; } = "UTC";

    // 5. ДОСТУПНОСТЬ
    public AccessibilityLevel AccessibilityLevel { get; set; } = AccessibilityLevel.Standard;

    // 6. УВЕДОМЛЕНИЯ
    public NotificationLevel NotificationLevel { get; set; } = NotificationLevel.All;
    public NotificationChannel NotificationChannels { get; set; } = NotificationChannel.Email | NotificationChannel.InApp;

    // 7. БЕЗОПАСНОСТЬ И ПОВЕДЕНИЕ
    public SessionTerminationMode SessionTerminationMode { get; set; } = SessionTerminationMode.Manual;
    public LoginNotificationMode LoginNotificationMode { get; set; } = LoginNotificationMode.NewDeviceOnly;
}
```

### 1.2 Enums (DAL/Enums/Settings/)

```csharp
// UiTheme
public enum UiTheme { Light = 0, Dark = 1, System = 2 }

// UiDensity
public enum UiDensity { Compact = 0, Comfortable = 1, Spacious = 2 }

// SidebarState
public enum SidebarState { Collapsed = 0, Expanded = 1, Hidden = 2 }

// NavigationBehavior
public enum NavigationBehavior { DefaultPage = 0, RememberLastPage = 1 }

// TableDensity
public enum TableDensity { Compact = 0, Normal = 1, Comfortable = 2 }

// DefaultPageSizeOption
public enum DefaultPageSizeOption { Size10 = 10, Size25 = 25, Size50 = 50, Size100 = 100 }

// AccessibilityLevel
public enum AccessibilityLevel { Standard = 0, Enhanced = 1, Maximum = 2 }

// NotificationLevel
public enum NotificationLevel { None = 0, Critical = 1, Important = 2, All = 3 }

// NotificationChannel (Flags)
[Flags]
public enum NotificationChannel 
{ 
    None = 0, 
    Email = 1, 
    InApp = 2, 
    Sms = 4, 
    Push = 8 
}

// SessionTerminationMode
public enum SessionTerminationMode { Manual = 0, OnBrowserClose = 1, AfterInactivity = 2 }

// LoginNotificationMode
public enum LoginNotificationMode { Never = 0, NewDeviceOnly = 1, Always = 2 }
```

### 1.3 DTO (Data Transfer Objects)

```csharp
// UserSettingsDetailDto (для чтения)
public class UserSettingsDetailDto
{
    public UiTheme Theme { get; set; }
    public UiDensity Density { get; set; }
    public string? PrimaryColor { get; set; }
    public SidebarState SidebarState { get; set; }
    public NavigationBehavior NavigationBehavior { get; set; }
    public TableDensity TableDensity { get; set; }
    public DefaultPageSizeOption DefaultPageSize { get; set; }
    public bool ShowAdvancedFilters { get; set; }
    public string Language { get; set; } = string.Empty;
    public string TimeZone { get; set; } = string.Empty;
    public AccessibilityLevel AccessibilityLevel { get; set; }
    public NotificationLevel NotificationLevel { get; set; }
    public NotificationChannel NotificationChannels { get; set; }
    public SessionTerminationMode SessionTerminationMode { get; set; }
    public LoginNotificationMode LoginNotificationMode { get; set; }
}

// UserSettingsUpdateDto (для обновления)
public class UserSettingsUpdateDto
{
    [Required]
    public UiTheme Theme { get; set; }
    
    [Required]
    public UiDensity Density { get; set; }
    
    [MaxLength(10)]
    [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")]
    public string? PrimaryColor { get; set; }
    
    // ... остальные поля аналогично DetailDto
}
```

### 1.4 API Endpoints (SettingsController)

#### **GET /api/settings**
Получение настроек текущего пользователя

**Request:** Нет параметров (UserId берётся из JWT)

**Response 200:**
```json
{
  "success": true,
  "data": {
    "theme": 2,
    "density": 1,
    "primaryColor": "#1890ff",
    "sidebarState": 1,
    "navigationBehavior": 1,
    "tableDensity": 1,
    "defaultPageSize": 10,
    "showAdvancedFilters": false,
    "language": "ru-RU",
    "timeZone": "UTC",
    "accessibilityLevel": 0,
    "notificationLevel": 3,
    "notificationChannels": 3,
    "sessionTerminationMode": 0,
    "loginNotificationMode": 1
  }
}
```

**Response 401:** Пользователь не авторизован

---

#### **PUT /api/settings**
Обновление настроек текущего пользователя

**Request Body:**
```json
{
  "theme": 1,
  "density": 0,
  "primaryColor": "#ff4d4f",
  "sidebarState": 0,
  "navigationBehavior": 0,
  "tableDensity": 0,
  "defaultPageSize": 25,
  "showAdvancedFilters": true,
  "language": "en-US",
  "timeZone": "America/New_York",
  "accessibilityLevel": 1,
  "notificationLevel": 2,
  "notificationChannels": 1,
  "sessionTerminationMode": 1,
  "loginNotificationMode": 2
}
```

**Response 200:**
```json
{
  "success": true,
  "data": { /* обновлённые настройки */ }
}
```

**Response 400:** Валидация не пройдена
```json
{
  "success": false,
  "errors": [
    { "field": "primaryColor", "message": "Цвет должен быть в формате HEX" }
  ]
}
```

---

#### **PATCH /api/settings**
Частичное обновление настроек (аналогично PUT)

---

#### **POST /api/settings/reset**
Сброс настроек к дефолтным значениям

**Request:** Нет параметров

**Response 200:**
```json
{
  "success": true,
  "data": { /* дефолтные настройки */ }
}
```

---

#### **GET /api/settings/exists**
Проверка существования настроек

**Response 200:**
```json
{
  "success": true,
  "exists": true
}
```

---

#### **GET /api/settings/user/{userId}** (Admin only)
Получение настроек пользователя по ID

**Response 200:** Аналогично GET /api/settings
**Response 403:** Недостаточно прав

---

### 1.5 Логика работы Backend

1. **При регистрации нового пользователя:**
   - Автоматически создаётся запись `UserSettings` с дефолтными значениями
   - Связь с `ApplicationUser` через `UserId`

2. **При первом обращении к настройкам:**
   - Если настройки не найдены, создаются автоматически (Lazy Creation)
   - Возвращаются дефолтные значения из модели

3. **При обновлении настроек:**
   - Валидация через FluentValidation
   - Логирование изменений (старые → новые значения)
   - Транзакция через UnitOfWork

4. **При удалении пользователя:**
   - Настройки удаляются каскадно (OnDelete: Cascade)

---

## 📋 ЧАСТЬ 2: FRONTEND ТЗ

### 2.1 Общая концепция

**Цель:** Создать интерфейс управления настройками пользователя с разделением на категории (вкладки) и мгновенным применением изменений.

**Расположение:** Отдельная страница `/settings` или модальное окно, доступное из меню профиля.

**Технологии:**
- Angular 18+
- Angular Material / Ng-Zorro (на выбор)
- RxJS для реактивности
- Signals для состояния

---

### 2.2 Структура UI

#### **Макет страницы:**

```
┌─────────────────────────────────────────────────────┐
│  [← Назад]    Настройки профиля    [Сбросить всё]  │
├─────────────────────────────────────────────────────┤
│                                                     │
│  ┌─────────────┬──────────────────────────────┐   │
│  │ Внешний вид │                              │   │
│  │ Навигация   │   КОНТЕНТ ВКЛАДКИ            │   │
│  │ Таблицы     │                              │   │
│  │ Локализация │                              │   │
│  │ Доступность │                              │   │
│  │ Уведомления │                              │   │
│  │ Безопасность│                              │   │
│  └─────────────┴──────────────────────────────┘   │
│                                                     │
│                    [Сохранить изменения]            │
└─────────────────────────────────────────────────────┘
```

---

### 2.3 Вкладки и контролы

#### **Вкладка 1: Внешний вид (UI/UX)**

**Контролы:**

1. **Тема интерфейса** (Radio Group / Segmented Control)
   - Светлая (Light)
   - Тёмная (Dark)
   - Системная (System)
   - **Действие:** Мгновенное применение темы

2. **Плотность интерфейса** (Radio Group)
   - Компактная (Compact)
   - Комфортная (Comfortable)
   - Просторная (Spacious)
   - **Действие:** Изменение отступов/размеров элементов

3. **Основной цвет** (Color Picker)
   - Выбор HEX цвета (#RRGGBB)
   - Предустановленные палитры
   - **Действие:** Применение акцентного цвета к кнопкам, ссылкам

---

#### **Вкладка 2: Навигация и Layout**

**Контролы:**

1. **Состояние сайдбара** (Radio Group)
   - Свёрнут (Collapsed)
   - Развёрнут (Expanded)
   - Скрыт (Hidden)

2. **Поведение при входе** (Radio Group)
   - Открывать главную страницу (DefaultPage)
   - Запоминать последнюю страницу (RememberLastPage)

---

#### **Вкладка 3: Списки и таблицы**

**Контролы:**

1. **Плотность таблиц** (Radio Group)
   - Компактная (Compact)
   - Нормальная (Normal)
   - Комфортная (Comfortable)

2. **Записей на странице** (Select / Dropdown)
   - 10, 25, 50, 100

3. **Расширенные фильтры** (Toggle / Switch)
   - Показывать по умолчанию (true/false)

---

#### **Вкладка 4: Локализация**

**Контролы:**

1. **Язык интерфейса** (Select)
   - ru-RU (Русский)
   - en-US (English)
   - Другие языки

2. **Часовой пояс** (Select с поиском)
   - UTC
   - Europe/Moscow
   - America/New_York
   - Список всех часовых поясов

---

#### **Вкладка 5: Доступность**

**Контролы:**

1. **Уровень доступности** (Radio Group)
   - Стандартный (Standard)
   - Повышенный (Enhanced) - увеличенные шрифты
   - Максимальный (Maximum) - высокий контраст + крупные элементы

---

#### **Вкладка 6: Уведомления**

**Контролы:**

1. **Уровень важности** (Radio Group)
   - Не показывать (None)
   - Только критичные (Critical)
   - Важные и критичные (Important)
   - Все уведомления (All)

2. **Каналы доставки** (Checkbox Group)
   - ☑ Email
   - ☑ В приложении (InApp)
   - ☐ SMS
   - ☐ Push-уведомления

---

#### **Вкладка 7: Безопасность и поведение**

**Контролы:**

1. **Завершение сессии** (Radio Group)
   - Вручную (Manual)
   - При закрытии браузера (OnBrowserClose)
   - После неактивности (AfterInactivity)

2. **Уведомления о входе** (Radio Group)
   - Никогда (Never)
   - Только с новых устройств (NewDeviceOnly)
   - Всегда (Always)

---

### 2.4 Поведение UI

#### **Сохранение изменений:**

**Вариант 1: Автосохранение (рекомендуется)**
- Каждое изменение контрола отправляет PATCH запрос
- Показывается индикатор "Сохранение..." → "Сохранено ✓"
- Debounce 500ms для текстовых полей

**Вариант 2: Ручное сохранение**
- Кнопка "Сохранить изменения" внизу страницы
- Активна только при наличии изменений
- Отправляет PUT запрос со всеми настройками

#### **Сброс настроек:**
- Кнопка "Сбросить всё" в шапке
- Диалог подтверждения: "Вы уверены? Все настройки будут сброшены к дефолтным"
- POST /api/settings/reset

#### **Индикация состояния:**
- Загрузка: Skeleton / Spinner
- Сохранение: Индикатор прогресса
- Ошибка: Toast notification с текстом ошибки

---

### 2.5 TypeScript модели (Frontend)

```typescript
// models/user-settings.model.ts

export enum UiTheme {
  Light = 0,
  Dark = 1,
  System = 2
}

export enum UiDensity {
  Compact = 0,
  Comfortable = 1,
  Spacious = 2
}

export enum SidebarState {
  Collapsed = 0,
  Expanded = 1,
  Hidden = 2
}

export enum NavigationBehavior {
  DefaultPage = 0,
  RememberLastPage = 1
}

export enum TableDensity {
  Compact = 0,
  Normal = 1,
  Comfortable = 2
}

export enum DefaultPageSizeOption {
  Size10 = 10,
  Size25 = 25,
  Size50 = 50,
  Size100 = 100
}

export enum AccessibilityLevel {
  Standard = 0,
  Enhanced = 1,
  Maximum = 2
}

export enum NotificationLevel {
  None = 0,
  Critical = 1,
  Important = 2,
  All = 3
}

export enum NotificationChannel {
  None = 0,
  Email = 1,
  InApp = 2,
  Sms = 4,
  Push = 8
}

export enum SessionTerminationMode {
  Manual = 0,
  OnBrowserClose = 1,
  AfterInactivity = 2
}

export enum LoginNotificationMode {
  Never = 0,
  NewDeviceOnly = 1,
  Always = 2
}

export interface UserSettings {
  theme: UiTheme;
  density: UiDensity;
  primaryColor: string | null;
  sidebarState: SidebarState;
  navigationBehavior: NavigationBehavior;
  tableDensity: TableDensity;
  defaultPageSize: DefaultPageSizeOption;
  showAdvancedFilters: boolean;
  language: string;
  timeZone: string;
  accessibilityLevel: AccessibilityLevel;
  notificationLevel: NotificationLevel;
  notificationChannels: NotificationChannel;
  sessionTerminationMode: SessionTerminationMode;
  loginNotificationMode: LoginNotificationMode;
}
```

---

### 2.6 Angular Service

```typescript
// services/user-settings.service.ts

@Injectable({ providedIn: 'root' })
export class UserSettingsService {
  private readonly apiUrl = '/api/settings';
  
  // Signal для хранения текущих настроек
  settings = signal<UserSettings | null>(null);
  
  // Signal для состояния загрузки
  loading = signal<boolean>(false);

  constructor(private http: HttpClient) {}

  // Загрузка настроек
  loadSettings(): Observable<UserSettings> {
    this.loading.set(true);
    return this.http.get<ApiResponse<UserSettings>>(this.apiUrl).pipe(
      map(response => response.data),
      tap(settings => {
        this.settings.set(settings);
        this.applySettings(settings); // Применить к UI
        this.loading.set(false);
      }),
      catchError(error => {
        this.loading.set(false);
        return throwError(() => error);
      })
    );
  }

  // Обновление настроек
  updateSettings(settings: Partial<UserSettings>): Observable<UserSettings> {
    return this.http.put<ApiResponse<UserSettings>>(this.apiUrl, settings).pipe(
      map(response => response.data),
      tap(updatedSettings => {
        this.settings.set(updatedSettings);
        this.applySettings(updatedSettings);
      })
    );
  }

  // Сброс к дефолтным
  resetToDefaults(): Observable<UserSettings> {
    return this.http.post<ApiResponse<UserSettings>>(`${this.apiUrl}/reset`, {}).pipe(
      map(response => response.data),
      tap(settings => {
        this.settings.set(settings);
        this.applySettings(settings);
      })
    );
  }

  // Применение настроек к UI
  private applySettings(settings: UserSettings): void {
    // Применить тему
    this.applyTheme(settings.theme);
    
    // Применить плотность
    this.applyDensity(settings.density);
    
    // Применить цвет
    if (settings.primaryColor) {
      this.applyPrimaryColor(settings.primaryColor);
    }
    
    // Применить состояние сайдбара
    this.applySidebarState(settings.sidebarState);
  }

  private applyTheme(theme: UiTheme): void {
    const body = document.body;
    body.classList.remove('light-theme', 'dark-theme');
    
    if (theme === UiTheme.System) {
      const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
      body.classList.add(prefersDark ? 'dark-theme' : 'light-theme');
    } else {
      body.classList.add(theme === UiTheme.Dark ? 'dark-theme' : 'light-theme');
    }
  }

  private applyDensity(density: UiDensity): void {
    const body = document.body;
    body.classList.remove('density-compact', 'density-comfortable', 'density-spacious');
    
    switch (density) {
      case UiDensity.Compact:
        body.classList.add('density-compact');
        break;
      case UiDensity.Comfortable:
        body.classList.add('density-comfortable');
        break;
      case UiDensity.Spacious:
        body.classList.add('density-spacious');
        break;
    }
  }

  private applyPrimaryColor(color: string): void {
    document.documentElement.style.setProperty('--primary-color', color);
  }

  private applySidebarState(state: SidebarState): void {
    // Логика управления сайдбаром через сервис или EventEmitter
  }
}

interface ApiResponse<T> {
  success: boolean;
  data: T;
}
```

---

## 📋 ЧАСТЬ 3: ЧЕК-ЛИСТ FRONTEND

### 3.1 Подготовка

- [ ] Создать модели TypeScript для всех Enum'ов
- [ ] Создать интерфейс `UserSettings`
- [ ] Создать сервис `UserSettingsService`
- [ ] Настроить HTTP Interceptor для автоматической отправки JWT

---

### 3.2 Компоненты

- [ ] **SettingsPageComponent** - главная страница настроек
  - [ ] Routing: `/settings`
  - [ ] Tabs для категорий
  - [ ] Кнопки "Сбросить всё" и "Сохранить"

- [ ] **AppearanceTabComponent** - вкладка "Внешний вид"
  - [ ] Radio Group для темы
  - [ ] Radio Group для плотности
  - [ ] Color Picker для основного цвета

- [ ] **NavigationTabComponent** - вкладка "Навигация"
  - [ ] Radio Group для состояния сайдбара
  - [ ] Radio Group для поведения при входе

- [ ] **TablesTabComponent** - вкладка "Таблицы"
  - [ ] Radio Group для плотности таблиц
  - [ ] Select для размера страницы
  - [ ] Toggle для расширенных фильтров

- [ ] **LocalizationTabComponent** - вкладка "Локализация"
  - [ ] Select для языка
  - [ ] Select для часового пояса

- [ ] **AccessibilityTabComponent** - вкладка "Доступность"
  - [ ] Radio Group для уровня доступности

- [ ] **NotificationsTabComponent** - вкладка "Уведомления"
  - [ ] Radio Group для уровня важности
  - [ ] Checkbox Group для каналов доставки

- [ ] **SecurityTabComponent** - вкладка "Безопасность"
  - [ ] Radio Group для режима завершения сессии
  - [ ] Radio Group для уведомлений о входе

---

### 3.3 Функциональность

- [ ] **Загрузка настроек при инициализации**
  - [ ] GET /api/settings при открытии страницы
  - [ ] Показ Skeleton во время загрузки
  - [ ] Обработка ошибки 401 (редирект на логин)

- [ ] **Сохранение изменений**
  - [ ] Автосохранение (debounce 500ms) ИЛИ
  - [ ] Ручное сохранение по кнопке
  - [ ] Индикация "Сохранение..." → "Сохранено ✓"
  - [ ] Обработка ошибок валидации (400)

- [ ] **Сброс к дефолтам**
  - [ ] Диалог подтверждения
  - [ ] POST /api/settings/reset
  - [ ] Обновление UI после сброса

- [ ] **Применение настроек к UI**
  - [ ] Смена темы (light/dark/system)
  - [ ] Изменение плотности (CSS классы)
  - [ ] Применение основного цвета (CSS переменные)
  - [ ] Управление сайдбаром

---

### 3.4 UX улучшения

- [ ] **Валидация на клиенте**
  - [ ] Проверка формата HEX цвета
  - [ ] Проверка формата языка (ru-RU)
  - [ ] Обязательные поля

- [ ] **Индикация изменений**
  - [ ] Маркер "несохранённые изменения" на вкладках
  - [ ] Предупреждение при уходе со страницы (CanDeactivate Guard)

- [ ] **Responsive дизайн**
  - [ ] Адаптация под мобильные устройства
  - [ ] Tabs → Accordion на маленьких экранах

- [ ] **Accessibility**
  - [ ] ARIA labels для всех контролов
  - [ ] Keyboard navigation
  - [ ] Focus management

---

### 3.5 Тестирование

- [ ] **Unit тесты**
  - [ ] UserSettingsService (loadSettings, updateSettings, resetToDefaults)
  - [ ] Компоненты (изменение контролов, сохранение)

- [ ] **E2E тесты**
  - [ ] Загрузка настроек
  - [ ] Изменение темы и проверка применения
  - [ ] Сохранение и перезагрузка страницы
  - [ ] Сброс к дефолтам

---

## 📋 ЧАСТЬ 4: ВОПРОСЫ ДЛЯ ОБСУЖДЕНИЯ

### 4.1 UI/UX решения

1. **Автосохранение vs Ручное сохранение?**
   - Автосохранение: Удобнее, но больше запросов к API
   - Ручное: Меньше запросов, но нужна кнопка "Сохранить"
   - **Ваше мнение?**

2. **Расположение страницы настроек:**
   - Отдельная страница `/settings` (рекомендуется)
   - Модальное окно из меню профиля
   - Drawer (боковая панель)
   - **Ваше мнение?**

3. **Применение темы:**
   - Мгновенное (при изменении Radio Group)
   - После сохранения
   - **Ваше мнение?**

4. **Валидация:**
   - Только на бэкенде (проще)
   - Дублировать на фронтенде (лучше UX)
   - **Ваше мнение?**

---

### 4.2 Технические вопросы

1. **Хранение настроек на фронтенде:**
   - Только в Signal (перезагрузка при каждом визите)
   - Signal + LocalStorage (кэширование)
   - **Ваше мнение?**

2. **Обработка NotificationChannel (Flags enum):**
   - Отправлять как число (3 = Email | InApp)
   - Отправлять как массив строк ["Email", "InApp"]
   - **Ваше мнение?**

3. **Интернационализация (i18n):**
   - Использовать @angular/localize
   - Использовать ngx-translate
   - Пока не нужно
   - **Ваше мнение?**

---

### 4.3 Приоритизация функций

**Какие вкладки реализовать в первую очередь?**

1. ☐ Внешний вид (Theme, Density, Color) - **Критично для UX**
2. ☐ Навигация (Sidebar, Navigation Behavior)
3. ☐ Таблицы (Table Density, Page Size)
4. ☐ Локализация (Language, TimeZone)
5. ☐ Доступность (Accessibility Level)
6. ☐ Уведомления (Notification Level, Channels)
7. ☐ Безопасность (Session, Login Notifications)

**Ваш порядок приоритетов?**

---

## 📋 ЧАСТЬ 5: СЛЕДУЮЩИЕ ШАГИ

После обсуждения и согласования ТЗ:

1. **Создать структуру файлов:**
   ```
   src/app/features/settings/
   ├── models/
   │   ├── user-settings.model.ts
   │   └── enums.ts
   ├── services/
   │   └── user-settings.service.ts
   ├── components/
   │   ├── settings-page/
   │   ├── appearance-tab/
   │   ├── navigation-tab/
   │   └── ...
   └── settings.routes.ts
   ```

2. **Реализовать базовую функциональность:**
   - Загрузка настроек
   - Отображение в UI
   - Сохранение изменений

3. **Добавить продвинутые фичи:**
   - Автосохранение
   - Валидация
   - Индикация состояния

4. **Тестирование и полировка**

---

**Готов обсудить любой из пунктов детально!** 🚀
