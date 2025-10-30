using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Enums;
using DAL.Models.Base;

namespace DAL.Models
{
    /// <summary>
    /// Модель для хранения детальной информации об имени на конкретном языке
    /// НЕ наследует LocalizedEntity, т.к. имеет специфичную структуру
    /// Содержит полную характеристику имени: значение, происхождение, характер носителя
    /// </summary>
    [Table("NameDetails")]
    public class NameDetail : BaseEntity
    {
        // ==========================================
        // Унаследовано от BaseEntity:
        // - Id : int - уникальный идентификатор записи
        // - CreatedAt : DateTime - дата создания записи
        // - UpdatedAt : DateTime? - дата последнего обновления
        // - IsActive : bool - флаг активности (для мягкого удаления)
        // ==========================================

        // ==========================================
        // ОСНОВНАЯ ИНФОРМАЦИЯ ОБ ИМЕНИ
        // ==========================================

        /// <summary>
        /// Само имя на выбранном языке
        /// Примеры: "Александр" (ru), "Alexander" (en), "Alessandro" (it)
        /// ЭТО НЕ "название сущности" из LocalizedEntity!
        /// </summary>
        [StringLength(128)]
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        /// Полное имя или официальная форма имени
        /// Примеры: "Александр Сергеевич", "Alexander the Great"
        /// </summary>
        [StringLength(128)]
        public string? FullName { get; set; }

        /// <summary>
        /// Уменьшительно-ласкательная форма имени
        /// Примеры: "Саша", "Шура", "Алекс", "Санёк"
        /// </summary>
        [StringLength(128)]
        public string? Diminutiv { get; set; }

        /// <summary>
        /// Родовая принадлежность имени (мужское/женское)
        /// Примеры: "мужское", "женское", "унисекс"
        /// </summary>
        [StringLength(128)]
        public string? NameRod { get; set; }

        /// <summary>
        /// Парное женское имя (для мужских имён)
        /// Примеры: для "Александр" → "Александра", для "Евгений" → "Евгения"
        /// </summary>
        [StringLength(256)]
        public string? NameFemaleCouple { get; set; }

        /// <summary>
        /// Дни именин (день ангела) по церковному календарю
        /// Примеры: "12 сентября, 6 декабря", "January 5, March 12"
        /// </summary>
        [StringLength(256)]
        public string? NameDays { get; set; }

        /// <summary>
        /// Имя в православной традиции
        /// Примеры: для "Светлана" → "Фотиния", для "Виктория" → "Ника"
        /// </summary>
        [StringLength(128)]
        public string? OrthodoxName { get; set; }

        /// <summary>
        /// Другие формы и производные имени
        /// Примеры: "Саша, Шура, Ксандр, Алекс, Сандро"
        /// </summary>
        [StringLength(256)]
        public string? OtherFormName { get; set; }

        /// <summary>
        /// Синонимы и аналоги имени в разных культурах
        /// Примеры: "Александр = Искандер = Олександр = Сандро"
        /// </summary>
        [StringLength(256)]
        public string? Synonyms { get; set; }

        /// <summary>
        /// Отчество, образованное от имени
        /// Примеры: для "Александр" → "Александрович/Александровна"
        /// </summary>
        [StringLength(256)]
        public string? Surname { get; set; }

        /// <summary>
        /// Сокращенная форма имени
        /// Примеры: "Саша", "Алекс", "Санёк"
        /// </summary>
        [StringLength(64)]
        public string? Abbreviated { get; set; }

        // ==========================================
        // ХАРАКТЕРИСТИКИ И ЗНАЧЕНИЕ ИМЕНИ
        // ==========================================

        /// <summary>
        /// Пол, к которому относится имя
        /// Значения: Male (1) - мужское, Female (2) - женское, Unisex (3) - универсальное
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// Полное подробное описание значения имени
        /// Содержит историю, происхождение, известных носителей, культурный контекст
        /// Примеры: "Александр - древнегреческое имя, означающее 'защитник людей'..."
        /// </summary>
        [StringLength(1024)]
        public string? Description { get; set; } = string.Empty;

        /// <summary>
        /// Краткое описание имени для превью и карточек
        /// Сокращённая версия полного описания (2-3 предложения)
        /// Примеры: "Александр - греческое имя. Означает 'защитник людей'."
        /// </summary>
        public string? DescriptionShort { get; set; }

        /// <summary>
        /// Краткое значение или перевод имени (одна фраза)
        /// Используется для быстрого отображения в списках
        /// Примеры: "Защитник людей", "Defender of the people", "Божий дар"
        /// </summary>
        public string? ShortMeaning { get; set; }

        /// <summary>
        /// Происхождение имени (культурное и языковое)
        /// Примеры: "Древнегреческое", "Greek", "Еврейское", "Славянское"
        /// </summary>
        public string? Origin { get; set; }

        /// <summary>
        /// Этимология имени - лингвистическое происхождение и значение корней
        /// Примеры: "От греч. 'alexo' (защищать) + 'andros' (человек, мужчина)"
        /// </summary>
        [StringLength(1024)]
        public string? Etymology { get; set; }

        /// <summary>
        /// Значение имени - развернутое толкование
        /// Примеры: "Имя означает 'защитник людей', символизирует силу и мужество"
        /// </summary>
        [StringLength(1024)]
        public string? Meaning { get; set; }

        /// <summary>
        /// Значение имени - краткая версия
        /// Примеры: "Защитник", "Победитель", "Мудрая"
        /// </summary>
        [StringLength(256)]
        public string? NameMeaning { get; set; }

        /// <summary>
        /// История происхождения имени
        /// Рассказ о том, как и когда появилось имя, его эволюция
        /// </summary>
        [StringLength(1024)]
        public string? OriginHistory { get; set; }

        /// <summary>
        /// Страна происхождения имени
        /// Примеры: "Греция", "Россия", "Израиль", "Франция"
        /// </summary>
        [StringLength(128)]
        public string? Country { get; set; }

        /// <summary>
        /// Произношение имени (транскрипция)
        /// Примеры: "[а-лек-САН-др]", "[ˌæl.ɪɡˈzæn.dɚ]"
        /// </summary>
        [StringLength(512)]
        public string? Pronunciation { get; set; }

        /// <summary>
        /// Производные формы от имени
        /// Список всех вариантов и форм имени
        /// </summary>
        [StringLength(512)]
        public string? FormsName { get; set; }

        // ==========================================
        // АСТРОЛОГИЯ И ЭЗОТЕРИКА
        // ==========================================

        /// <summary>
        /// Благоприятный день недели для носителей имени
        /// Примеры: "Вторник", "Tuesday", "Среда и пятница"
        /// </summary>
        [StringLength(64)]
        public string? LuckyDay { get; set; }

        /// <summary>
        /// Подробное описание благоприятного дня
        /// Объяснение, почему этот день считается удачным
        /// </summary>
        [StringLength(512)]
        public string? LuckyDayDescription { get; set; }

        // ==========================================
        // ХАРАКТЕР И ЛИЧНОСТЬ
        // ==========================================

        /// <summary>
        /// Черты характера носителей имени
        /// Основные психологические характеристики
        /// Примеры: "Смелый, решительный, справедливый"
        /// </summary>
        [StringLength(256)]
        public string? CharacterTraits { get; set; }

        /// <summary>
        /// Характер взрослого человека с этим именем
        /// Подробное описание личностных качеств в зрелом возрасте
        /// </summary>
        [StringLength(2056)]
        public string? AdultCharacter { get; set; }

        /// <summary>
        /// Характер подростка с этим именем
        /// Особенности поведения и личности в подростковом возрасте
        /// </summary>
        [StringLength(512)]
        public string? Teenager { get; set; }

        /// <summary>
        /// Общая характеристика личности
        /// Целостное описание личностных качеств носителя имени
        /// </summary>
        [StringLength(512)]
        public string? Personality { get; set; }

        /// <summary>
        /// Основные черты личности
        /// Ключевые особенности характера
        /// </summary>
        [StringLength(512)]
        public string? MainFeatures { get; set; }

        /// <summary>
        /// Позитивные характеристики
        /// Сильные стороны и достоинства
        /// Примеры: "Храбрость, честность, надёжность"
        /// </summary>
        [StringLength(512)]
        public string? PositiveCharacteristic { get; set; }

        /// <summary>
        /// Негативные характеристики
        /// Слабые стороны и недостатки
        /// Примеры: "Упрямство, вспыльчивость, импульсивность"
        /// </summary>
        [StringLength(512)]
        public string? NegativeCharacteristic { get; set; }

        /// <summary>
        /// Психологический тип личности
        /// Примеры: "Сангвиник", "Экстраверт", "Лидер"
        /// </summary>
        [StringLength(512)]
        public string? Type { get; set; }

        // ==========================================
        // ПСИХОЛОГИЧЕСКИЕ ХАРАКТЕРИСТИКИ
        // ==========================================

        /// <summary>
        /// Особенности психики
        /// Психологический портрет носителя имени
        /// </summary>
        [StringLength(512)]
        public string? Psyche { get; set; }

        /// <summary>
        /// Сила воли
        /// Характеристика волевых качеств
        /// Примеры: "Сильная воля", "Целеустремленность"
        /// </summary>
        [StringLength(512)]
        public string? Will { get; set; }

        /// <summary>
        /// Уровень возбудимости нервной системы
        /// Эмоциональная реактивность
        /// </summary>
        [StringLength(512)]
        public string? Excitability { get; set; }

        /// <summary>
        /// Скорость реакции на события
        /// Быстрота принятия решений
        /// </summary>
        [StringLength(512)]
        public string? ReactionSpeed { get; set; }

        /// <summary>
        /// Интуиция
        /// Способность к интуитивному познанию
        /// </summary>
        [StringLength(512)]
        public string? Intuition { get; set; }

        /// <summary>
        /// Интеллектуальные способности
        /// Характеристика умственных способностей
        /// </summary>
        [StringLength(512)]
        public string? Intellect { get; set; }

        /// <summary>
        /// Восприимчивость и чувствительность
        /// Эмоциональная отзывчивость
        /// </summary>
        [StringLength(512)]
        public string? Sensitivity { get; set; }

        /// <summary>
        /// Нравственные качества
        /// Моральные принципы и ценности
        /// </summary>
        [StringLength(512)]
        public string? Morality { get; set; }

        /// <summary>
        /// Уровень активности
        /// Энергичность и деятельность
        /// </summary>
        [StringLength(512)]
        public string? Activity { get; set; }

        /// <summary>
        /// Общительность и коммуникабельность
        /// Способность к социальному взаимодействию
        /// </summary>
        [StringLength(512)]
        public string? Sociability { get; set; }

        /// <summary>
        /// Энергетика имени
        /// Энергетическое влияние имени на носителя
        /// </summary>
        [StringLength(512)]
        public string? Energy { get; set; }

        // ==========================================
        // НУМЕРОЛОГИЯ И ЭЗОТЕРИЧЕСКИЙ АНАЛИЗ
        // ==========================================

        /// <summary>
        /// Характеристика имени по Борису Хигиру
        /// Анализ по методике известного российского антропонимиста
        /// </summary>
        [StringLength(512)]
        public string? ByHigir { get; set; }

        /// <summary>
        /// Характеристика имени по Менделееву
        /// Анализ на основе системы Дмитрия Менделеева
        /// </summary>
        [StringLength(512)]
        public string? ByMendeleev { get; set; }

        /// <summary>
        /// Фоносемантический анализ имени
        /// Исследование звучания имени и его подсознательного восприятия
        /// Как звуки имени влияют на восприятие личности
        /// </summary>
        [StringLength(1024)]
        public string? PhonosemanticAnalysis { get; set; }

        // ==========================================
        // ПРОФЕССИЯ И КАРЬЕРА
        // ==========================================

        /// <summary>
        /// Профессии и бизнес
        /// Рекомендуемые профессии и направления деятельности
        /// </summary>
        [StringLength(1024)]
        public string? ProfessionAndBusiness { get; set; }

        /// <summary>
        /// Профессия и карьера
        /// Карьерные склонности и профессиональная реализация
        /// </summary>
        [StringLength(1024)]
        public string? ProfessionAndCareer { get; set; }

        /// <summary>
        /// Бизнес
        /// Предпринимательские способности и рекомендации
        /// </summary>
        [StringLength(512)]
        public string? Business { get; set; }

        /// <summary>
        /// Поле деятельности
        /// Наиболее подходящие сферы профессиональной реализации
        /// </summary>
        [StringLength(512)]
        public string? FieldOfActivity { get; set; }

        // ==========================================
        // ЗДОРОВЬЕ И УВЛЕЧЕНИЯ
        // ==========================================

        /// <summary>
        /// Здоровье
        /// Характеристика здоровья и медицинские рекомендации
        /// Потенциальные слабые места организма
        /// </summary>
        [StringLength(1024)]
        public string? Health { get; set; }

        /// <summary>
        /// Увлечения и хобби
        /// Склонности к определённым видам досуга и хобби
        /// Примеры: "Спорт, чтение, путешествия"
        /// </summary>
        [StringLength(1024)]
        public string? Hobbies { get; set; }

        // ==========================================
        // ЛЮБОВЬ, СЕКС И ОТНОШЕНИЯ
        // ==========================================

        /// <summary>
        /// Секс и любовь (общая характеристика)
        /// Особенности сексуальной и романтической сферы
        /// </summary>
        [StringLength(1024)]
        public string? SexAndLove { get; set; }

        /// <summary>
        /// Любовь и сексуальность (развернутое описание)
        /// Подробная характеристика любовных отношений
        /// </summary>
        [StringLength(1024)]
        public string? LoveAndSexuality { get; set; }

        /// <summary>
        /// Любовь
        /// Особенности проявления любовных чувств
        /// </summary>
        [StringLength(512)]
        public string? Love { get; set; }

        /// <summary>
        /// Сексуальность
        /// Характеристика сексуальной сферы
        /// </summary>
        [StringLength(512)]
        public string? Sexuality { get; set; }

        // ==========================================
        // БРАК И СЕМЬЯ
        // ==========================================

        /// <summary>
        /// Семья и брак (общая характеристика)
        /// Особенности семейных отношений
        /// </summary>
        [StringLength(1024)]
        public string? FamilyAndMarriage { get; set; }

        /// <summary>
        /// Брак и семья (развернутое описание)
        /// Подробная характеристика семейной жизни
        /// </summary>
        [StringLength(1024)]
        public string? MarriageAndFamily { get; set; }

        /// <summary>
        /// Брак
        /// Характеристика брачных отношений
        /// </summary>
        [StringLength(512)]
        public string? Marriage { get; set; }

        /// <summary>
        /// Поведение в браке
        /// Особенности поведения в семейной жизни
        /// Роль в семье, отношение к партнеру и детям
        /// </summary>
        [StringLength(1024)]
        public string? MarriageBehavior { get; set; }

        /// <summary>
        /// Судьба имени в любви и браке
        /// Предсказания и характеристики любовной судьбы
        /// </summary>
        [StringLength(512)]
        public string? Fate { get; set; }

        // ==========================================
        // СОВМЕСТИМОСТЬ
        // ==========================================

        /// <summary>
        /// Гармония с другими именами
        /// Список имён, с которыми хорошая совместимость
        /// Примеры: "Мария, Екатерина, Анна"
        /// </summary>
        [StringLength(1024)]
        public string? HarmonyWith { get; set; }

        /// <summary>
        /// Несовместимость с другими именами
        /// Список имён, с которыми плохая совместимость
        /// Примеры: "Ольга, Татьяна, Ирина"
        /// </summary>
        [StringLength(1024)]
        public string? IncompatibilityWith { get; set; }

        // ==========================================
        // Связи - ПРЯМЫЕ, не через LocalizedEntity
        // ==========================================

        /// <summary>
        /// Внешний ключ на таблицу Names
        /// Указывает, к какому базовому имени относится эта детальная информация
        /// </summary>
        [Required]
        [ForeignKey("NameMain")]
        public int NameMainId { get; set; }

        /// <summary>
        /// Навигационное свойство - ссылка на основное имя (NameMain)
        /// Позволяет получить базовую информацию об имени через EF Core
        /// </summary>
        public virtual NameMain NameMain { get; set; } = null!;

        /// <summary>
        /// Внешний ключ на таблицу Languages
        /// Указывает, на каком языке написана вся информация в этой записи
        /// </summary>
        [Required]
        [ForeignKey("Language")]
        public int LanguageId { get; set; }

        /// <summary>
        /// Навигационное свойство - ссылка на язык (Language)
        /// Позволяет получить информацию о языке через EF Core
        /// </summary>
        public virtual Language Language { get; set; } = null!;
    }
}